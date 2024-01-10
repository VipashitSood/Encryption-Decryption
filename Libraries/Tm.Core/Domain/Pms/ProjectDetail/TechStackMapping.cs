using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class TechStackMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the BackendTechStackId
        /// </summary>
        public int? BackendTechStackId { get; set; }
        /// <summary>
        /// Gets or sets the FrontendTechStackId
        /// </summary>
        public int? FrontendTechStackId { get; set; }
        /// <summary>
        /// Gets or sets the Android
        /// </summary>
        public bool Android { get; set; }
        /// <summary>
        /// Gets or sets the IOS
        /// </summary>
        public bool IOS { get; set; }
        /// <summary>
        /// Gets or sets the Hybrid
        /// </summary>
        public bool Hybrid { get; set; }
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
