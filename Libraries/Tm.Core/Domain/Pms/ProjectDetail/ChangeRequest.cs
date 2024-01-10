using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class ChangeRequest: BaseEntity
    {
        /// <summary>
        /// Gets or sets the OrderId
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the CRName
        /// </summary>
        public string CRName { get; set; }
        /// <summary>
        /// Gets or sets the EstimatedDuration
        /// </summary>
        public int EstimatedDuration { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedEfforts
        /// </summary>
        public int EstimatedEfforts { get; set; }
        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Gets or sets the EstTotalHours
        /// </summary>
        public int EstTotalHours { get; set; }
        /// <summary>
        /// Gets or sets the PlannedStartDate
        /// </summary>
        public DateTime PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets the PlannedEndDate
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualStartDate
        /// </summary>
        public DateTime ActualStartDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualEndDate
        /// </summary>
        public DateTime ActualEndDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonStartDate
        /// </summary>
        public string DelayReasonStartDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonEndDate
        /// </summary>
        public string DelayReasonEndDate { get; set; }
        /// <summary>
        /// Gets or sets the Attachment
        /// </summary>
        public string Attachment { get; set; }
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
