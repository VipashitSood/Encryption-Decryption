using System;
using Tm.Core.Domain.Schedules;
using Tm.Data;
using Tm.Services.Caching.Extensions;

namespace Tm.Services.Schedules
{
    /// <summary>
    /// Schedule service
    /// </summary>
    public partial class ScheduleService : IScheduleService
    {
        #region Fields

        private readonly IRepository<Schedule> _scheduleRepository;

        #endregion

        #region Ctor

        public ScheduleService(IRepository<Schedule> scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        public virtual void DeleteSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Delete(schedule);
        }

        /// <summary>
        /// Gets a schedule 
        /// </summary>
        /// <param name="scheduleId">Schedule identifier</param>
        /// <returns>Schedule</returns>
        public virtual Schedule GetScheduleById(int scheduleId)
        {
            if (scheduleId == 0)
                return null;

            return _scheduleRepository.ToCachedGetById(scheduleId);
        }

        /// <summary>
        /// Inserts a schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        public virtual void InsertSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Insert(schedule);
        }

        /// <summary>
        /// Updates the schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        public virtual void UpdateSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Update(schedule);
        }

        #endregion
    }
}