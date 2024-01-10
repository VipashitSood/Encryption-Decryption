using System;
using System.Collections.Generic;
using Tm.Core;
using Tm.Core.Domain.Pms.UserRole;

namespace Tm.Core.Domain.Pms.UserRole
{
    public class UserADRoles: BaseEntity
    {
        public List<ADUserRoleList> ADUserRoleList { get; set; }
        /// <summary>
        /// Gets or sets the Roles
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
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
        public string AdUserId { get; set; }
        public string AdUserName { get; set; }
    }
}
