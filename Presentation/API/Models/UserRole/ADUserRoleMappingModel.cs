using API.Models.BaseModels;
using System;
using System.Collections.Generic;

namespace API.Models.UserRole
{
    public class ADUserRoleMappingModel : BaseRequestModel
    {
        public ADUserRoleMappingModel()
        {
            ADUserIds = new List<string>();
            SelectedRoleIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets user role identifier
        /// </summary>        
        public int UserRoleId { get; set; }

        /// <summary>
        /// Gets or sets Ad user identifiers
        /// </summary>
        public IList<string> ADUserIds { get; set; }

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

        public IList<int> SelectedRoleIds { get; set; }

        public string UserId { get; set; }
    }
}
