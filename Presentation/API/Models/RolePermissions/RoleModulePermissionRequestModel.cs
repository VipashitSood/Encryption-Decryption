using API.Models.BaseModels;

namespace API.Models.RolePermissions
{
	public class RoleModulePermissionRequestModel : BaseRequestModel
    {

        /// <summary>
        /// Gets or sets the RoleId
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Gets or sets the Module Id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// Gets or sets the RoleModulePermissionId
        /// </summary>
        public int RoleModulePermissionId { get; set; }
        /// <summary>
        /// Gets or sets the RoleModulePermissionName
        /// </summary>
        public string RoleModulePermissionName { get; set; }
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
