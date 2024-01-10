using API.Models.UserRole;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.UserRole;

namespace API.Factories.UserRole
{
    public interface IUserRoleFactory
    {
        #region ADUsers

        /// <summary>
        /// Get all user by AD
        /// </summary>
        /// <returns></returns>
        Task<List<ADUser>> GetAllUserByAD();

        /// <summary>
        /// Save ad user
        /// </summary>
        /// <param name="adUser"></param>
        /// <returns></returns>
        Task SaveADUserAsync(ADUserModel model);

        #endregion

        #region User Roles
        Task<List<UserRoles>> GetAllUserRoles();

        Task<(bool, string)> DeleteUserRoleAndMenuById(int id);
        Task<UserADRoles> GetUserRoleById(int id);
        Task<UserRoles> GetUsersRoleById(int id);
        Task<UserRoles> GetUserRoleByName(string name);
        Task<(bool, string, int)> CreateUserRole(UserRolesModel model);
        Task<(bool, string, int)> UpdateUserRole(UserRolesModel model);
        #endregion User Roles

        //#region Role

        //Task SaveRoleAsync(ADUserModel adModel);
        //#endregion

        #region UserRoleMapping
        Task<IList<ADUserRoleListModel>> GetAllUserRoleMapping(int roleId, string userId);

        Task<(bool, string)> SaveUserRoleMapping(ADUserRoleMappingModel model);
        #endregion
    }

}
