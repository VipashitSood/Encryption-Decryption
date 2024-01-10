using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class Meetings: BaseEntity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? ExternalKickOff { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? InternalKickOff { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? PlannedUAT { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? PlannedLive { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? ActualUAT { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public DateTime? ActualLive { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public string DelayReasonUAT { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
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
        /// <summary>
        /// Gets or sets the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
