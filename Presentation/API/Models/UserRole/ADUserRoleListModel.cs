using API.Models.BaseModels;
using System.Collections.Generic;

namespace API.Models.UserRole
{
    public class ADUserRoleListModel 
    {
        public ADUserRoleListModel()
        {
            RoleModels = new List<RoleModel>();
        }

        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user identifier
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets user identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets selected role models
        /// </summary>
        public IList<RoleModel> RoleModels { get; set; }
    }

    #region Nested Classes

    public class RoleModel
    {
        public int Id { get; set; }

        public bool IsSelected { get; set; }
    }

    #endregion

}
