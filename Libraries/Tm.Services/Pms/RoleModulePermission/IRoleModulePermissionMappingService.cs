using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.ProjectDetail;

namespace Tm.Services.Pms.RoleModulePermission
{
	public interface IRoleModulePermissionMappingService
	{

		/// <summary>
		/// Get All Role Module Permission
		/// </summary>
		/// <returns></returns>
		Task<IList<RoleModulePermissionMapping>> GetAllRoleModulePermissionMapping();

		/// <summary>
		/// Get Role Module Permission Mapping By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
	   Task<RoleModulePermissionMapping> GetRoleModulePermissionMappingById(int id);
		/// <summary>
		/// Get Role Menu Permission Mapping By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<RoleMenuPermissionMapping> GetMenuPermissionMappingByRoleId(int id);

		/// <summary>
		/// Insert Role Module Permission Mapping
		/// </summary>
		/// <param name="projectName"></param>
		/// <exception cref="ArgumentNullException"></exception>
		Task InsertRoleModulePermissionMapping(RoleModulePermissionMapping roleModulePermission);


		/// <summary>
		/// Update Role Module Permission Mapping
		/// </summary>
		/// <param name="projectName"></param>
		/// <exception cref="ArgumentNullException"></exception>
		Task UpdateRoleModulePermissionMapping(RoleModulePermissionMapping roleModulePermission);
		/// <summary>
		/// Update Menu Permission Mapping
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		Task UpdateMenuPermissionMapping(RoleMenuPermissionMapping roleMenuPermission);

		/// <summary>
		/// Retrieves a list of RoleModulePermissionMapping records filtered by role and module.
		/// </summary>
		/// <param name="roleId">The ID of the role to filter by. Pass 0 to include all roles.</param>
		/// <param name="moduleId">The ID of the module to filter by. Pass 0 to include all modules.</param>
		/// <returns>A list of RoleModulePermissionMapping records that match the provided filters.</returns>
		Task<List<RoleModulePermissionMappingResponseModel>> GetAllRoleModulePermissionFilterByRoleAndModuleMapping(int pageNumber, int pageSize, int roleId, int moduleId);

		/// <summary>
		/// Retrieves a list of RoleMenuPermissionMapping records filtered by role and module.
		/// </summary>
		/// <param name="roleId">The ID of the role to filter by. Pass 0 to include all roles.</param>
		/// <returns>A list of RoleMenuPermissionMapping records that match the provided filters.</returns>
		Task<List<RoleMenuPermissionMappingResponseModel>> GetAllMenuPermissionFilterByRoleMapping(int roleId, int pageNumber, int pageSize);

		/// <summary>
		/// Retrieves a RoleModulePermissionMapping record filtered by user, module, and module record.
		/// </summary>
		/// <param name="userId">The ID of the user to filter by.</param>
		/// <param name="moduleName">The name of the module to filter by.</param>
		/// <param name="moduleRecordNameList">The name of the module record list to filter by.</param>
		/// <returns>A RoleModulePermissionMapping record that matches the provided filters, or a default record if filters are empty or null.</returns>
		Task<IList<RoleModulePermissionMapping>> GetFindUserPermissionReadEdit(string userId, string moduleName);
	}
}
