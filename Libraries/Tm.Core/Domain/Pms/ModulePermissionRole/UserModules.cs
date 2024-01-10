using System;

namespace Tm.Core.Domain.Pms.ModulePermissionRole
{
	public partial class UserModules : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Module Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
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
