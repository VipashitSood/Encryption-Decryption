using API.Models.BaseModels;
using System;

namespace API.Models.ProjectDetail
{
    public class MeetingModel : BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        ///  Gets or sets the ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the ExternalKickOff
        /// </summary>
        public DateTime? ExternalKickOff { get; set; }
        /// <summary>
        /// Gets or sets the InternalKickOff
        /// </summary>
        public DateTime? InternalKickOff { get; set; }
        /// <summary>
        /// Gets or sets the PlannedUAT
        /// </summary>
        public DateTime? PlannedUAT { get; set; }
        /// <summary>
        /// Gets or sets the PlannedLive
        /// </summary>
        public DateTime? PlannedLive { get; set; }
        /// <summary>
        /// Gets or sets the ActualUAT
        /// </summary>
        public DateTime? ActualUAT { get; set; }
        /// <summary>
        /// Gets or sets the ActualLive
        /// </summary>
        public DateTime? ActualLive { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonUAT
        /// </summary>
        public string DelayReasonUAT { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonLive
        /// </summary>
        public string DelayReasonLive { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
