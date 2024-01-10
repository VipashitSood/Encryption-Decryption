using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ModulePermissionRole;

namespace Tm.Services.Pms.UserModule
{
	public interface IUserModuleService
    {

        /// <summary>
        /// Get All User Modules
        /// </summary>
        /// <returns></returns>
        Task<IList<UserModules>> GetAllUserModules();

        /// <summary>
        /// Get User Modules By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserModules> GetUserModulesById(int id);


        /// <summary>
        /// Insert a new User Module
        /// </summary>
        /// <param name="userModules">The User Module to insert</param>
        /// <exception cref="ArgumentNullException"></exception>
        Task InsertUserModule(UserModules userModules);

        /// <summary>
        /// Update an existing User Module
        /// </summary>
        /// <param name="userModules">The User Module to update</param>
        /// <exception cref="ArgumentNullException"></exception>
        Task UpdateUserModule(UserModules userModules);
    }
}
