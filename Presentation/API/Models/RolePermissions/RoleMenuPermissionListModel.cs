using API.Models.BaseModels;
using System.Collections.Generic;

namespace API.Models.RolePermissions
{
	public class RoleMenuPermissionListModel : BaseResponseModel
    {
        public RoleMenuPermissionListModel()
        {
            RoleMenuPermissionList = new List<RoleMenuPermissionModel>();
        }
        /// <summary>
        /// Module List
        /// </summary>
        public List<RoleMenuPermissionModel> RoleMenuPermissionList { get; set; }
    }
}
