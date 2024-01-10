using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
   public partial class BillingInfo: BaseEntity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the MilestoneName
        /// </summary>
        public string MilestoneName { get; set; }
        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Gets or sets the Hours
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// Gets or sets the EndDate
        /// </summary>
        public DateTime EndDate { get; set; }
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
