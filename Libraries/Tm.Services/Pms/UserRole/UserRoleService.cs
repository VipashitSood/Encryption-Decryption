using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Tm.Core.Caching;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.UserRole;
using Tm.Core.Domain.Users;
using Tm.Data;
using Tm.Services.Caching.Extensions;
using Tm.Services.Events;

namespace Tm.Services.Pms.UserRole
{
    public partial class UserRoleService : IUserRoleService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<UserRoles> _userRoles;
        private readonly IRepository<ADUser> _adUserRepository;
        private readonly IRepository<ADUserRoleMapping> _adUserRepositoryRoleMappingRepository;
        private readonly CachingSettings _cachingSettings;
        #endregion

        #region Ctor
        public UserRoleService(IEventPublisher eventPublisher,
            IRepository<UserRoles> userRoles,
            IRepository<ADUser> adUserRepository,
            IRepository<ADUserRoleMapping> adUserRoleMappingRepository, CachingSettings cachingSettings)
        {
            _eventPublisher = eventPublisher;
            _userRoles = userRoles;
            _adUserRepository = adUserRepository;
            _adUserRepositoryRoleMappingRepository = adUserRoleMappingRepository;
            _cachingSettings= cachingSettings;
        }

        #endregion

        #region ADUser

        /// <summary>
        /// Gets all AD users 
        /// </summary>
        /// <returns>List of ADUser</returns>
        public virtual async Task<IList<ADUser>> GetAllUserByAD()
        {
            return await _adUserRepository.Table.ToListAsync();
        }

        /// <summary>
        /// Gets AD user by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        public virtual async Task<ADUser> GetADUserByADUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var query = _adUserRepository.Table.Where(x => x.UserId == userId);
            if (query != null)
            {
                var result = await query.FirstOrDefaultAsync();

                return result;
            }
            else { return null; }
        }
        public virtual ADUser GetADUserByADCatchedUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            return _adUserRepository.ToCachedGetById(userId, _cachingSettings.ShortTermCacheTime);
        }

        /// <summary>
        /// Insert a ADUser
        /// </summary>
        public virtual async Task InsertADUser(ADUser aDUser)
        {
            if (aDUser == null)
                throw new ArgumentNullException(nameof(aDUser));

            await _adUserRepository.InsertAsync(aDUser);
        }

        /// <summary>
        /// Update a ADUser
        /// </summary>
        public virtual async Task UpdateADUser(ADUser aDUser)
        {
            if (aDUser == null)
                throw new ArgumentNullException(nameof(aDUser));

            await _adUserRepository.UpdateAsync(aDUser);
        }

        /// <summary>
        /// Delete a ADUser
        /// </summary>
        public virtual Task DeleteADUser(ADUser aDUser)
        {
            if (aDUser == null)
                throw new ArgumentNullException(nameof(aDUser));

            _adUserRepository.Delete(aDUser);
            return Task.CompletedTask;
        }

        public virtual async Task<ADUser> GetADUserById(int Id)
        {
            if (Id == 0)
                throw new ArgumentNullException(nameof(Id));

            var query = _adUserRepository.Table.Where(x => x.Id == Id && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public virtual async Task<IList<ADUser>> GetAllProjectManagers()
        {
            var query = _adUserRepository.Table.Where(x => x.JobTitle == "Project Manager" || x.JobTitle == "Sr. Project Manager" || x.JobTitle == "Technical Project Manager");

            var result = await query.ToListAsync();

            return result;
        }
        #endregion

        #region UserRoles
        /// <summary>
        /// Gets all UserRole
        /// </summary>
        /// <returns>UserRole</returns>
        public virtual async Task<IList<UserRoles>> GetAllUserRoles()
        {
            var query = _userRoles.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a UserRole 
        /// </summary>
        /// <param name="id">UserRole identifier</param>
        /// <returns>UserRole</returns>
        public virtual async Task<UserRoles> GetUserRoleById(int id)
        {
            if (id == 0)
                return null;

            //var query = from userRole in _userRoles.Table
            //            join mapping in _adUserRepositoryRoleMappingRepository.Table on userRole.Id equals mapping.UserRoleId
            //            join adUser in _adUserRepository.Table on mapping.ADUserId equals adUser.UserId
            //            where userRole.Id == roleId
            //            select new UserADRoles
            //            {
            //                Id = userRole.Id,
            //                Name = userRole.Name,
            //                AdUserId = mapping.ADUserId,
            //                AdUserName = adUser.Name
            //            };

            return await _userRoles.Table.FirstOrDefaultAsync(user => user.Id == id && !user.IsDeleted);
        }

        /// <summary>
        /// Gets a UserRole 
        /// </summary>
        /// <param name="id">UserRole identifier</param>
        /// <returns>UserRole</returns>
        public virtual async Task<UserRoles> GetUsersRoleById(int roleId)
        {
            if (roleId == 0)
                return null;

            var userRole = await _userRoles.Table.FirstOrDefaultAsync(b => b.Id == roleId);

            return userRole;
        }
        /// <summary>
        /// Inserts a User Role
        /// </summary>
        /// <param name="userRole">UserRole</param>
        public virtual async Task InsertUserRole(UserRoles userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            await _userRoles.InsertAsync(userRole);

            //event notification
            _eventPublisher.EntityInserted(userRole);
        }

        /// <summary>
        /// Updates the UserRole
        /// </summary>
        /// <param name="userRole">UserRole</param>
        public virtual async Task UpdateUserRole(UserRoles userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            await _userRoles.UpdateAsync(userRole);

            //event notification
            _eventPublisher.EntityUpdated(userRole);
        }

        /// <summary>
        /// Deletes the UserRole
        /// </summary>
        /// <param name="userRole">UserRole</param>
        public virtual async Task<UserRoles> DeleteUserRole(int id)
        {
            if (id == 0)
                return null;

            var Projects = _userRoles.GetByIdAsync(id);

            return await Projects;
        }
        /// <summary>
        /// Get UserRole By Name
        /// </summary>
        /// <param name="userRole">UserRole</param>
        public async Task<UserRoles> GetUserRoleByName(string name)
        {
            return await _userRoles.Table.FirstOrDefaultAsync(user => user.Name == name && !user.IsDeleted);
        }

        #endregion

        #region Role

        public virtual async Task SaveUserRolesAsync(ADUser adUser)
        {
            if (adUser == null)
                throw new ArgumentNullException(nameof(adUser));

            await _adUserRepository.InsertAsync(adUser);

            _eventPublisher.EntityInserted(adUser);
        }
        #endregion

        #region ADUserRoleMapping

        /// <summary>
        /// Gets all UserRole mapping
        /// </summary>
        /// <returns>ADUserRoleMappings</returns>
        public virtual async Task<IList<ADUserRoleMapping>> GetAllUserRoleMapping(int roleId=0, string userId = null)
        {
            var query = _adUserRepositoryRoleMappingRepository.Table;

            if (roleId > 0)
                query = query.Where(x => x.UserRoleId == roleId);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.ADUserId == userId);
            
            //query = query.Where(x => x.IsDeleted == false);

            var result = await query.GroupBy(x=>x.ADUserId).
                Select(g=>g.FirstOrDefault()).ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets UserRole mapping by Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        public virtual async Task<ADUserRoleMapping> GetUserRoleMappingById(int id)
        {
            if (id <= 0)
                return null;

            var query = _adUserRepositoryRoleMappingRepository.Table.Where(x => x.Id == id);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets UserRole mapping by user role Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        public virtual async Task<ADUserRoleMapping> GetUserRoleMappingByRoleId(int roleId)
        {
            if (roleId <= 0)
                return null;

            var query = _adUserRepositoryRoleMappingRepository.Table.Where(x=>x.UserRoleId == roleId);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets UserRole mapping by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        public virtual async Task<ADUserRoleMapping> GetUserRoleMappingByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var query = _adUserRepositoryRoleMappingRepository.Table.Where(x => x.ADUserId == userId);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets user role identifier by user Identifier
        /// </summary>
        /// <returns>UserRoleIds</returns>
        public virtual List<int> GetUserRoleIdsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var query = from p in _adUserRepositoryRoleMappingRepository.Table
                        where p.ADUserId == userId && p.IsDeleted == false
                        select p.UserRoleId;

            return query.ToList();
        }

        /// <summary>
        /// Gets UserRole mappings by user Identifier
        /// </summary>
        /// <returns>ADUserRoleMapping</returns>
        public virtual async Task<List<ADUserRoleMapping>> GetUserRoleMappingsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var query = _adUserRepositoryRoleMappingRepository.Table.Where(x => x.ADUserId == userId);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Insert a UserRole mapping
        /// </summary>
        public virtual async Task InsertUserRoleMapping(ADUserRoleMapping aDUserRoleMapping)
        {
            if (aDUserRoleMapping == null)
                throw new ArgumentNullException(nameof(aDUserRoleMapping));

            await _adUserRepositoryRoleMappingRepository.InsertAsync(aDUserRoleMapping);
        }

        /// <summary>
        /// Update a UserRole mapping
        /// </summary>
        public virtual async Task UpdateUserRoleMapping(ADUserRoleMapping aDUserRoleMapping)
        {
            if (aDUserRoleMapping == null)
                throw new ArgumentNullException(nameof(aDUserRoleMapping));

            await _adUserRepositoryRoleMappingRepository.UpdateAsync(aDUserRoleMapping);
        }

        /// <summary>
        /// Delete a UserRole mapping
        /// </summary>
        public virtual Task DeleteUserRoleMapping(ADUserRoleMapping aDUserRoleMapping)
        {
            if (aDUserRoleMapping == null)
                throw new ArgumentNullException(nameof(aDUserRoleMapping));

            _adUserRepositoryRoleMappingRepository.Delete(aDUserRoleMapping);
            return Task.CompletedTask;
        }

        #endregion
    }
}
