﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tm.Core;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Tasks;
using Tm.Core.Http;
using Tm.Core.Infrastructure;
using Tm.Services.Localization;
using Tm.Services.Logging;

namespace Tm.Services.Tasks
{
    /// <summary>
    /// Represents task thread
    /// </summary>
    public partial class TaskThread : IDisposable
    {
        #region Fields

        private static readonly string _scheduleTaskUrl;
        private static readonly int? _timeout;

        private readonly Dictionary<string, string> _tasks;
        private Timer _timer;
        private bool _disposed = false;

        #endregion

        #region Ctor

        static TaskThread()
        {
            _scheduleTaskUrl = $"{EngineContext.Current.Resolve<IStoreContext>().CurrentStore.Url}{"api/"}{TmTaskDefaults.ScheduleTaskPath}";
            _timeout = EngineContext.Current.Resolve<CommonSettings>().ScheduleTaskRunTimeout;
        }

        internal TaskThread()
        {
            _tasks = new Dictionary<string, string>();
            Seconds = 10 * 60;
        }

        #endregion

        #region Utilities

        private void Run()
        {
            if (Seconds <= 0)
                return;

            StartedUtc = DateTime.UtcNow;
            IsRunning = true;
            HttpClient client = null;

            foreach (var taskName in _tasks.Keys)
            {
                var taskType = _tasks[taskName];
                try
                {
                    //create and configure client
                    client = EngineContext.Current.Resolve<IHttpClientFactory>().CreateClient(TmHttpDefaults.DefaultHttpClient);
                    if (_timeout.HasValue)
                        client.Timeout = TimeSpan.FromMilliseconds(_timeout.Value);

                    //send post data
                    var data = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>(nameof(taskType), taskType) });
                    client.PostAsync(_scheduleTaskUrl, data).Wait();
                    //try
                    //{
                    //    var scheduleTaskService = EngineContext.Current.Resolve<IScheduleTaskService>();
                    //    var scheduleTask = scheduleTaskService.GetTaskByType(taskType);
                    //    if (scheduleTask != null)
                    //    {
                    //        var task = new Task(scheduleTask);
                    //        task.Execute();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    var serviceScopeFactory = EngineContext.Current.Resolve<IServiceScopeFactory>();
                    //    using var scope = serviceScopeFactory.CreateScope();
                    //    // Resolve
                    //    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

                    //    logger.Error("Task execute error log:", ex);
                    //}
                }
                catch (Exception ex)
                {
                    var serviceScopeFactory = EngineContext.Current.Resolve<IServiceScopeFactory>();
                    using var scope = serviceScopeFactory.CreateScope();
                    // Resolve
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                    var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
                    var storeContext = scope.ServiceProvider.GetRequiredService<IStoreContext>();

                    var message = ex.InnerException?.GetType() == typeof(TaskCanceledException) ? localizationService.GetResource("ScheduleTasks.TimeoutError") : ex.Message;

                    message = string.Format(localizationService.GetResource("ScheduleTasks.Error"), taskName,
                        message, taskType, storeContext.CurrentStore.Name, _scheduleTaskUrl);

                    logger.Error(message, ex);
                }
                finally
                {
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
                    }
                }
            }

            IsRunning = false;
        }

        private void TimerHandler(object state)
        {
            try
            {
                _timer.Change(-1, -1);
                Run();

                if (RunOnlyOnce)
                    Dispose();
                else
                    _timer.Change(Interval, Interval);
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                lock (this)
                {
                    _timer?.Dispose();
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// Inits a timer
        /// </summary>
        public void InitTimer()
        {
            if (_timer == null)
                _timer = new Timer(TimerHandler, null, InitInterval, Interval);
        }

        /// <summary>
        /// Adds a task to the thread
        /// </summary>
        /// <param name="task">The task to be added</param>
        public void AddTask(ScheduleTask task)
        {
            if (!_tasks.ContainsKey(task.Name))
                _tasks.Add(task.Name, task.Type);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interval in seconds at which to run the tasks
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Get or set the interval before timer first start 
        /// </summary>
        public int InitSeconds { get; set; }

        /// <summary>
        /// Get or sets a datetime when thread has been started
        /// </summary>
        public DateTime StartedUtc { get; private set; }

        /// <summary>
        /// Get or sets a value indicating whether thread is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the interval (in milliseconds) at which to run the task
        /// </summary>
        public int Interval
        {
            get
            {
                //if somebody entered more than "2147483" seconds, then an exception could be thrown (exceeds int.MaxValue)
                var interval = Seconds * 1000;
                if (interval <= 0)
                    interval = int.MaxValue;
                return interval;
            }
        }

        /// <summary>
        /// Gets the due time interval (in milliseconds) at which to begin start the task
        /// </summary>
        public int InitInterval
        {
            get
            {
                //if somebody entered less than "0" seconds, then an exception could be thrown
                var interval = InitSeconds * 1000;
                if (interval <= 0)
                    interval = 0;
                return interval;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the thread would be run only once (on application start)
        /// </summary>
        public bool RunOnlyOnce { get; set; }

        #endregion
    }
}