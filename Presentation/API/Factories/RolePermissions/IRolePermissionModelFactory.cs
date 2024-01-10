using API.Models.RolePermissions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ModulePermissionRole;

namespace API.Factories.RolePermissions
{
	/// <summary>
	/// Represents the interface of the role permission module
	/// </summary>
	public partial interface IRolePermissionModelFactory
    {
        Task<RoleModulePermissionListModel> GetAllRoleModulePermissionFilterByRoleAndModuleMapping(int roleId, int moduleId, int pageIndex, int pageSize);

		//Task<RoleMenuPermissionListModel> GetAllMenuPermissionFilterByRoleAndModuleMapping(int roleId, int pageIndex, int pageSize);
	}
}
