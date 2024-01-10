using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.UserModule
{
    public partial class UserModuleService : IUserModuleService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<UserModules> _userModules;
        #endregion

        #region Ctor

        public UserModuleService(IEventPublisher eventPublisher,
            IRepository<UserModules> userModules)
        {
            _eventPublisher = eventPublisher;
            _userModules = userModules;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get All User Modules
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<UserModules>> GetAllUserModules()
        {
            var query = _userModules.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get User Modules By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<UserModules> GetUserModulesById(int id)
        {
            if (id <= 0)
                return null;

            var roleModulePermission = await _userModules.GetByIdAsync(id);

            if (roleModulePermission != null && !roleModulePermission.IsDeleted)
            {
                return roleModulePermission;
            }
            return null;
        }

        /// <summary>
        /// Insert a new User Module
        /// </summary>
        /// <param name="userModules">The User Module to insert</param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertUserModule(UserModules userModules)
        {
            if (userModules == null)
                throw new ArgumentNullException(nameof(userModules));

            await _userModules.InsertAsync(userModules);

            // Notify listeners about the entity insertion
            _eventPublisher.EntityInserted(userModules);
        }

        /// <summary>
        /// Update an existing User Module
        /// </summary>
        /// <param name="userModules">The User Module to update</param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateUserModule(UserModules userModules)
        {
            if (userModules == null)
                throw new ArgumentNullException(nameof(userModules));

            await _userModules.UpdateAsync(userModules);

            // Notify listeners about the entity update
            _eventPublisher.EntityUpdated(userModules);
        }
        #endregion
    }
}
