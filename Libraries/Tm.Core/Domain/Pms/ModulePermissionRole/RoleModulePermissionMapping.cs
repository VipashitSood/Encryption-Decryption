using System;

namespace Tm.Core.Domain.Pms.ModulePermissionRole
{
	public partial class RoleModulePermissionMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the User Role Id
        /// </summary>
        public int UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets the User Module Id
        /// </summary>
        public int UserModuleId { get; set; }

        /// <summary>
        /// Gets or sets the Permission Module Record Id
        /// </summary>
        public int PermissionModuleRecordId { get; set; }

        /// <summary>
        /// Gets or sets the Read
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the Edit
        /// </summary>
        public bool Edit { get; set; }

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
