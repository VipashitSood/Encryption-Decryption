using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class MonthlyPlannedHrs : BaseEntity
    {
        /// <summary>
        /// Gets or sets the OrderId
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        public string PlannedDate { get; set; }
        /// <summary>
        ///  Gets or sets the Hours
        /// </summary>
        public decimal Hours { get; set; }
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
