using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.PermissionModuleRecordResponse;

namespace Tm.Services.Pms.PermissionModule
{
	public interface IPermissionModuleRecordService
    {

        /// <summary>
        /// Get All Permission Module Record
        /// </summary>
        /// <returns></returns>

        Task<IList<PermissionModuleRecordModel>> GetAllPermissionModuleRecord(string name = "", int userModuleId = 0);

        /// <summary>
        /// Get Permission Module Record By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PermissionModuleRecord> GetPermissionModuleRecordById(int id);


        /// <summary>
        /// Insert Permission Module Record
        /// </summary>
        /// <param name="roleModulePermission"></param>
        /// <exception cref="ArgumentNullException"></exception>
        Task InsertPermissionModuleRecord(PermissionModuleRecord permissionModuleRecord);

        /// <summary>
        /// Update Permission Module Record
        /// </summary>
        /// <param name="permissionModuleRecord"></param>
        /// <exception cref="ArgumentNullException"></exception>
        Task UpdatePermissionModuleRecord(PermissionModuleRecord permissionModuleRecord);
    }
}
