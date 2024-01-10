using API.Models.BaseModels;

namespace API.Models.RolePermissions
{
	public class RoleMenuPermissionRequestModel : BaseRequestModel
    {

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

    }
}
