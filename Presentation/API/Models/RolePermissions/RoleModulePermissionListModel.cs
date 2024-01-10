using API.Models.BaseModels;
using System.Collections.Generic;

namespace API.Models.RolePermissions
{
	public class RoleModulePermissionListModel : BaseResponseModel
    {
        public RoleModulePermissionListModel()
        {
            RoleModulePermissionList = new List<RoleModulePermissionModel>();
        }
        /// <summary>
        /// Module List
        /// </summary>
        public List<RoleModulePermissionModel> RoleModulePermissionList { get; set; }
    }
}
