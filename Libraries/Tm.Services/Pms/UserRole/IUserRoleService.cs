using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.UserRole;

namespace Tm.Services.Pms.UserRole
{
    public interface IUserRoleService
    {
        #region ADUser

        /// <summary>
        /// Gets all AD users 
        /// </summary>
        /// <returns>List of ADUser</returns>
        Task<IList<ADUser>> GetAllUserByAD();

        /// <summary>
        /// Gets AD user by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        Task<ADUser> GetADUserByADUserId(string userId);
        ADUser GetADUserByADCatchedUserId(string userId);

        /// <summary>
        /// Insert a ADUser
        /// </summary>
        Task InsertADUser(ADUser aDUser);

        /// <summary>
        /// Update a ADUser
        /// </summary>
        Task UpdateADUser(ADUser aDUser);

        /// <summary>
        /// Delete a ADUser
        /// </summary>
        Task DeleteADUser(ADUser aDUser);

        /// <summary>
        ///  Gets AD user by user Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        Task<ADUser> GetADUserById(int Id);
        Task<IList<ADUser>> GetAllProjectManagers();
        #endregion

        #region UserRoles
        Task<IList<UserRoles>> GetAllUserRoles();

        Task<UserRoles> GetUserRoleById(int roleId);
        Task<UserRoles> GetUsersRoleById(int id);

        Task InsertUserRole(UserRoles userRoles);

        Task UpdateUserRole(UserRoles userRoles);

        Task<UserRoles> DeleteUserRole(int id);
        Task<UserRoles> GetUserRoleByName(string name);

        #endregion UserRoles

        #region Role

        Task SaveUserRolesAsync(ADUser adUser);

        #endregion

        #region User Role Mapping
        /// <summary>
        /// Gets all UserRole mapping
        /// </summary>
        /// <returns>ADUserRoleMappings</returns>
        Task<IList<ADUserRoleMapping>> GetAllUserRoleMapping(int roleId = 0, string userId = null);

        /// <summary>
        /// Gets UserRole mapping by Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        Task<ADUserRoleMapping> GetUserRoleMappingById(int id);

        /// <summary>
        /// Gets UserRole mapping by user role Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        Task<ADUserRoleMapping> GetUserRoleMappingByRoleId(int roleId);

        /// <summary>
        /// Gets UserRole mapping by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        Task<ADUserRoleMapping> GetUserRoleMappingByUserId(string userId);

        /// <summary>
        /// Gets user role identifier by user Identifier
        /// </summary>
        /// <returns>UserRoleIds</returns>
        List<int> GetUserRoleIdsByUserId(string userId);

        /// <summary>
        /// Gets UserRole mappings by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        Task<List<ADUserRoleMapping>> GetUserRoleMappingsByUserId(string userId);

        /// <summary>
        /// Insert a UserRole mapping
        /// </summary>
        Task InsertUserRoleMapping(ADUserRoleMapping aDUserRoleMapping);

        /// <summary>
        /// Update a UserRole mapping
        /// </summary>
        Task UpdateUserRoleMapping(ADUserRoleMapping aDUserRoleMapping);

        /// <summary>
        /// Delete a UserRole mapping
        /// </summary>
        Task DeleteUserRoleMapping(ADUserRoleMapping aDUserRoleMapping);
        #endregion
    }
}
