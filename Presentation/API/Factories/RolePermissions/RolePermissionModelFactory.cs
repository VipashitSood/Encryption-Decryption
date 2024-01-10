using API.Models.RolePermissions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.ProjectResponse;
using Tm.Data;
using Tm.Services.Pms.PermissionModule;
using Tm.Services.Pms.RoleModulePermission;
using API.Factories.Menus;

namespace API.Factories.RolePermissions
{
    /// <summary>
    /// Represents the account model factory
    /// </summary>
    public partial class RolePermissionModelFactory : IRolePermissionModelFactory
    {
        #region Fields
        private readonly IRoleModulePermissionMappingService _roleModulePermissionMappingService;
        private readonly IPermissionModuleRecordService _permissionModuleRecordService;
        private IMenuFactory _menuFactory;
        #endregion

        #region Ctor

        public RolePermissionModelFactory(
            IRoleModulePermissionMappingService roleModulePermissionMappingService,
            IPermissionModuleRecordService permissionModuleRecordService, IMenuFactory menuFactory)
        {
            _roleModulePermissionMappingService = roleModulePermissionMappingService;
            _permissionModuleRecordService = permissionModuleRecordService;
            _menuFactory = menuFactory;
        }

        #endregion

        #region Methods
        public async Task<RoleModulePermissionListModel> GetAllRoleModulePermissionFilterByRoleAndModuleMapping(int roleId, int moduleId, int pageIndex, int pageSize)
        {
            try
            {
                // Call the existing method to get a list of RoleModulePermissionMappingResponseModel
                var allRoleModulePermissionReadEditMappingList = await _roleModulePermissionMappingService.GetAllRoleModulePermissionFilterByRoleAndModuleMapping(roleId, moduleId, pageIndex, pageSize);

                // Create a new RoleModulePermissionListModel to populate
                var model = new RoleModulePermissionListModel();

                foreach (var item in allRoleModulePermissionReadEditMappingList)
                {
                    var permissionModuleRecords = await _permissionModuleRecordService.GetAllPermissionModuleRecord();
                    var permissionModuleRecord = permissionModuleRecords.FirstOrDefault(p => p.Id == item.PermissionModuleRecordId);

                    if (permissionModuleRecord != null)
                    {
                        RoleModulePermissionModel roleModulePermissionModel = new RoleModulePermissionModel();
                        roleModulePermissionModel.Id = item.Id;
                        roleModulePermissionModel.RoleId = roleId;
                        roleModulePermissionModel.ModuleId = moduleId;
                        roleModulePermissionModel.RoleModulePermissionId = item.PermissionModuleRecordId;
                        roleModulePermissionModel.Read = item.Read;
                        roleModulePermissionModel.Edit = item.Edit;
                        roleModulePermissionModel.RoleModulePermissionName = permissionModuleRecord.Name;
                        model.RoleModulePermissionList.Add(roleModulePermissionModel);
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingProjectData, ex);
            }
        }

        //public async Task<RoleMenuPermissionListModel> GetAllMenuPermissionFilterByRoleAndModuleMapping(int roleId, int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        // Call the existing method to get a list of MenuPermissionMappingResponseModel
        //        var allRoleMenuPermissionReadEditMappingList = await _roleModulePermissionMappingService.GetAllMenuPermissionFilterByRoleMapping(roleId, pageIndex, pageSize);

        //        // Create a new RoleMenuPermissionListModel to populate
        //        var model = new RoleMenuPermissionListModel();
        //        var allMenus = _menuFactory.GetAllMenus("", 0);
        //        foreach (var item in allRoleMenuPermissionReadEditMappingList)
        //        {
        //            RoleMenuPermissionModel roleMenuPermissionModel = new RoleMenuPermissionModel();
        //            roleMenuPermissionModel.Id = item.Id;
        //            roleMenuPermissionModel.RoleId = roleId;
        //            roleMenuPermissionModel.MenuId = item.MenuId;
        //            roleMenuPermissionModel.MenuName = allMenus.Result.Where(x => x.Id == item.MenuId).FirstOrDefault()?.MenuName;
        //            if (item.SubMenuId > 0)
        //            {
        //                roleMenuPermissionModel.SubMenuName = allMenus
        //                    .Result
        //                    .SelectMany(x => x.SubMenus)
        //                    .Where(y => y.Id == item.SubMenuId)
        //                    .Select(y => y.SubMenuName)
        //                    .FirstOrDefault().ToString();
        //            }
        //            if (item.ChildMenuId > 0)
        //            {
        //                roleMenuPermissionModel.ChildMenuName = allMenus
        //                    .Result
        //                    .SelectMany(x => x.SubMenus)
        //                    .SelectMany(z=>z.ChildMenus)
        //                    .Where(y => y.Id == item.ChildMenuId)
        //                    .Select(y => y.ChildMenuName)
        //                    .FirstOrDefault().ToString();
        //            }
        //            roleMenuPermissionModel.SubMenuId = item.SubMenuId;
        //            roleMenuPermissionModel.ChildMenuId = item.ChildMenuId;
        //            roleMenuPermissionModel.Read = item.Read;
        //            roleMenuPermissionModel.Edit = item.Edit;
        //            model.RoleMenuPermissionList.Add(roleMenuPermissionModel);
        //        }

        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ConstantValues.ErrorFetchingProjectData, ex);
        //    }
        //}
        #endregion
    }
}