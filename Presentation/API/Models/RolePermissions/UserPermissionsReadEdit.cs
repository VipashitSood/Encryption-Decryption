using API.Models.BaseModels;

namespace API.Models.RolePermissions
{
	public class UserPermissionsReadEdit : BaseRequestModel
    {
        /// <summary>
        ///   Gets or sets the User Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the Module Name
        /// </summary>
        public string ModuleName { get; set; }
    }
}
