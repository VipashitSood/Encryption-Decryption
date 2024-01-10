using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ModulePermissionRole
{
    public class RoleMenuPermissionMappingResponseModel : BaseEntity
    {
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the RoleId
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Gets or sets the MenuId
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// Gets or sets the SubMenuId
        /// </summary>
        public int SubMenuId { get; set; }
        /// <summary>
        /// Gets or sets the ChildMenuId
        /// </summary>
        public int ChildMenuId { get; set; }
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
