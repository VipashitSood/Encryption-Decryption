using System;

namespace Tm.Core.Domain.Pms.UserRole
{
    public class ADUserRoleMapping : BaseEntity
    {
        /// Gets or sets user role identifier
        public int UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets Ad user identifier
        /// </summary>
        public string ADUserId { get; set; }

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
        public bool? IsDeleted { get; set; } 
    }
}
