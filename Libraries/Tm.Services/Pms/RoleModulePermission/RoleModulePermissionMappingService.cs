using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.ProjectResponse;
using Tm.Core.Domain.Pms.UserRole;
using Tm.Data;
using Tm.Services.Events;
using Tm.Services.Pms.UserModule;
using Tm.Services.Pms.UserRole;

namespace Tm.Services.Pms.RoleModulePermission
{
	public partial class RoleModulePermissionMappingService : IRoleModulePermissionMappingService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<RoleModulePermissionMapping> _roleModulePermissionMapping;
        private readonly IRepository<RoleMenuPermissionMapping> _roleMenuPermissionMapping;
        private readonly IRepository<UserModules> _userModules;
        private readonly IRepository<PermissionModuleRecord> _permissionModuleRecord;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserModuleService _userModuleService;
        private readonly IRepository<UserRoles> _userRoles;
        private readonly IRepository<ADUser> _userAD;
        private readonly IRepository<ADUserRoleMapping> _aDUserRoleMapping;
        private readonly IRepository<RoleModulePermissionMappingResponseModel> _iRoleModulePermissionRepository;
        private readonly IRepository<RoleMenuPermissionMappingResponseModel> _iRoleMenuPermissionRepository;
        #endregion

        #region Ctor

        public RoleModulePermissionMappingService(IEventPublisher eventPublisher,
            IRepository<RoleModulePermissionMapping> roleModulePermissionMapping,
            IRepository<RoleMenuPermissionMapping> roleMenuPermissionMapping,
            IRepository<UserModules> userModules,
            IRepository<PermissionModuleRecord> permissionModuleRecord,
            IUserRoleService userRoleService,
             IUserModuleService userModuleService,
             IRepository<UserRoles> userRoles,
             IRepository<ADUser> userAD,
             IRepository<ADUserRoleMapping> aDUserRoleMapping,
             IRepository<RoleModulePermissionMappingResponseModel> iRoleModulePermissionRepository,
             IRepository<RoleMenuPermissionMappingResponseModel> iRoleMenuPermissionRepository)
        {
            _eventPublisher = eventPublisher;
            _roleModulePermissionMapping = roleModulePermissionMapping;
            _userModules = userModules;
            _permissionModuleRecord = permissionModuleRecord;  
            _userRoleService=userRoleService;
            _userModuleService = userModuleService;
            _userRoles = userRoles;
            _userAD = userAD;
            _aDUserRoleMapping = aDUserRoleMapping;
            _iRoleModulePermissionRepository=iRoleModulePermissionRepository;
            _iRoleMenuPermissionRepository = iRoleMenuPermissionRepository;
            _roleMenuPermissionMapping = roleMenuPermissionMapping;
        }

        #endregion

        #region Method

        /// <summary>
        /// Get All Role Module Permission
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<RoleModulePermissionMapping>> GetAllRoleModulePermissionMapping()
        {
            var query = _roleModulePermissionMapping.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Role Module Permission Mapping By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<RoleModulePermissionMapping> GetRoleModulePermissionMappingById(int id)
        {
            if (id <= 0)
                return null;

            var roleModulePermission = await _roleModulePermissionMapping.GetByIdAsync(id);

            if (roleModulePermission != null && !roleModulePermission.IsDeleted)
            {
                return roleModulePermission;
            }
            return null;
        }

        /// <summary>
        /// Get Role Menu Permission Mapping By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<RoleMenuPermissionMapping> GetMenuPermissionMappingByRoleId(int id)
        {
            if (id <= 0)
                return null;

            var roleMenuPermission = await _roleMenuPermissionMapping.GetByIdAsync(id);

            if (roleMenuPermission != null && !roleMenuPermission.IsDeleted)
            {
                return roleMenuPermission;
            }
            return null;
        }

        /// <summary>
        /// Insert Role Module Permission Mapping
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertRoleModulePermissionMapping(RoleModulePermissionMapping roleModulePermission)
        {
            if (roleModulePermission == null)
                throw new ArgumentNullException(nameof(roleModulePermission));

            await _roleModulePermissionMapping.InsertAsync(roleModulePermission);

            //event notification
            _eventPublisher.EntityInserted(roleModulePermission);
        }

        /// <summary>
        /// Update Role Module Permission Mapping
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateRoleModulePermissionMapping(RoleModulePermissionMapping roleModulePermission)
        {
            if (roleModulePermission == null)
                throw new ArgumentNullException(nameof(roleModulePermission));

            await _roleModulePermissionMapping.UpdateAsync(roleModulePermission);

            //event notification
            _eventPublisher.EntityUpdated(roleModulePermission);
        }

        /// <summary>
        /// Update Menu Permission Mapping
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateMenuPermissionMapping(RoleMenuPermissionMapping roleMenuPermission)
        {
            if (roleMenuPermission == null)
                throw new ArgumentNullException(nameof(roleMenuPermission));

            await _roleMenuPermissionMapping.UpdateAsync(roleMenuPermission);

            //event notification
            _eventPublisher.EntityUpdated(roleMenuPermission);
        }


        /// <summary>
        /// Retrieves a list of RoleModulePermissionMapping records filtered by role and module.
        /// </summary>
        /// <param name="roleId">The ID of the role to filter by. Pass 0 to include all roles.</param>
        /// <param name="moduleId">The ID of the module to filter by. Pass 0 to include all modules.</param>
        /// <returns>A list of RoleModulePermissionMapping records that match the provided filters.</returns>

        public async Task<List<RoleModulePermissionMappingResponseModel>> GetAllRoleModulePermissionFilterByRoleAndModuleMapping(int roleId, int moduleId, int pageNumber, int pageSize)
        {
            var pRoleId = SqlParameterHelper.GetInt32Parameter("RoleId", roleId);
            var pModuleId = SqlParameterHelper.GetInt32Parameter("ModuleId", moduleId);
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);

            return await Task.Factory.StartNew<List<RoleModulePermissionMappingResponseModel>>(() =>
            {
                List<RoleModulePermissionMappingResponseModel> projectResponseModelList = _iRoleModulePermissionRepository.EntityFromSql("SSP_GetAllRoleModulePermissions",
                    pRoleId,
                    pModuleId,
                    pPageNumber,
                    pPageSize).ToList();
                return projectResponseModelList;
            });

        }

        /// <summary>
        /// Retrieves a list of MenuPermissionMapping records filtered by role and module.
        /// </summary>
        /// <param name="roleId">The ID of the role to filter by. Pass 0 to include all roles.</param>
        /// <returns>A list of MenuPermissionMapping records that match the provided filters.</returns>

        public async Task<List<RoleMenuPermissionMappingResponseModel>> GetAllMenuPermissionFilterByRoleMapping(int roleId, int pageNumber, int pageSize)
        {
            var pRoleId = SqlParameterHelper.GetInt32Parameter("RoleId", roleId);
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);

            return await Task.Factory.StartNew<List<RoleMenuPermissionMappingResponseModel>>(() =>
            {
                List<RoleMenuPermissionMappingResponseModel> projectResponseModelList = _iRoleMenuPermissionRepository.EntityFromSql("SSP_GetAllMenuPermissions",
                    pRoleId,
                    pPageNumber,
                    pPageSize).ToList();
                return projectResponseModelList;
            });

        }

        /// <summary>
        /// Retrieves a RoleModulePermissionMapping record filtered by user, module, and module record names.
        /// </summary>
        /// <param name="userId">The ID of the user to filter by.</param>
        /// <param name="moduleName">The name of the module to filter by.</param>
        /// <param name="moduleRecordNameList">A list of module record names to filter by.</param>
        /// <returns>A RoleModulePermissionMapping record that matches the provided filters, or a default record if filters are empty or null.</returns>
        public virtual async Task<IList<RoleModulePermissionMapping>> GetFindUserPermissionReadEdit(string userId, string moduleName)
        {
            // Check for null or empty inputs
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(moduleName))
            {
                // Handle null or empty inputs by returning a default RoleModulePermissionMapping instance
                return new List<RoleModulePermissionMapping>();
            }

            // Get the user roles associated with the user ID
            var userRoles = await _aDUserRoleMapping.Table
                .Where(ur => ur.ADUserId == userId)
                .Select(ur => ur.UserRoleId)
                .ToListAsync();

            // Get the user modules based on the module name
            var userModules = await _userModules.Table
               .Where(um => um.Name == moduleName)
               .Select(um => um.Id)
               .ToListAsync();


            // Query to retrieve the filtered RoleModulePermissionMapping record with joins
            var filteredMapping = await (
                from mapping in _roleModulePermissionMapping.Table
                where userRoles.Contains(mapping.UserRoleId) &&
                      userModules.Contains(mapping.UserModuleId)
                select mapping
            ).ToListAsync();

            return filteredMapping;
        }

        #endregion
    }
}
