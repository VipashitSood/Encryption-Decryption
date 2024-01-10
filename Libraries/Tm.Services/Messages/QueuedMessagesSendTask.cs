using System;
using System.Collections.Generic;
using System.Linq;
using Tm.Core.Domain.Messages;
using Tm.Services.Logging;
using Tm.Services.Tasks;

namespace Tm.Services.Messages
{
    /// <summary>
    /// Represents a task for sending queued message 
    /// </summary>
    public partial class QueuedMessagesSendTask : IScheduleTask
    {
        #region Fields

        private readonly IEmailAccountService _emailAccountService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IQueuedEmailService _queuedEmailService;
        #endregion

        #region Ctor

        public QueuedMessagesSendTask(IEmailAccountService emailAccountService,
            IEmailSender emailSender,
            ILogger logger,
            IQueuedEmailService queuedEmailService)
        {
            _emailAccountService = emailAccountService;
            _emailSender = emailSender;
            _logger = logger;
            _queuedEmailService = queuedEmailService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            IList<int> queueEmailIds = new List<int>();

            var maxTries = 3;
            var queuedEmails = _queuedEmailService.SearchEmails(null, null, null, null,
                true, true, maxTries, false, 0, 500);
            foreach (var queuedEmail in queuedEmails)
            {
                var bcc = string.IsNullOrWhiteSpace(queuedEmail.Bcc)
                            ? null
                            : queuedEmail.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cc = string.IsNullOrWhiteSpace(queuedEmail.CC)
                            ? null
                            : queuedEmail.CC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    if (!queueEmailIds.Any(x => x == queuedEmail.Id))
                    {
                        _emailSender.SendEmail(_emailAccountService.GetEmailAccountById(queuedEmail.EmailAccountId),
                            queuedEmail.Subject,
                            queuedEmail.Body,
                           queuedEmail.From,
                           queuedEmail.FromName,
                           queuedEmail.To,
                           queuedEmail.ToName,
                           queuedEmail.ReplyTo,
                           queuedEmail.ReplyToName,
                           bcc,
                           cc,
                           queuedEmail.AttachmentFilePath,
                           queuedEmail.AttachmentFileName,
                           queuedEmail.AttachedDownloadId);

                        queuedEmail.SentOnUtc = DateTime.UtcNow;
                    }
                }
                catch (Exception exc)
                {
                    _logger.Error($"Error sending e-mail. {exc.Message}", exc);
                }
                finally
                {
                    queuedEmail.SentTries += 1;
                    _queuedEmailService.UpdateQueuedEmail(queuedEmail);

                    queueEmailIds.Add(queuedEmail.Id);
                    //queued email log

                    //var queuedemaillog = new QueuedEmailLog
                    //{
                    //    QueuedEmailId = queuedEmail.Id,
                    //    To = queuedEmail.To,
                    //    ToName = queuedEmail.ToName,
                    //    Subject = queuedEmail.Subject,
                    //    Body = queuedEmail.Body,
                    //    CreatedOnUtc = DateTime.UtcNow,
                    //    SentOnUtc = DateTime.UtcNow,
                    //};

                    //_queuedEmailLogService.InsertQueuedEmailLog(queuedemaillog);
                }
            }
        }

        #endregion
    }
}