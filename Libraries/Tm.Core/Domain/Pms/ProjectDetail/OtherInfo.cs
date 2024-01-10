using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class OtherInfo : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the TotalAmount
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// Gets or sets the TotalHours
        /// </summary>
        public int TotalHours { get; set; }
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
