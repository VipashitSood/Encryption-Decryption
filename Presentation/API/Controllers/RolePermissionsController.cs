using API.Factories.RolePermissions;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.RolePermissions;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Services.Localization;
using Tm.Services.Pms.PermissionModule;
using Tm.Services.Pms.RoleModulePermission;
using Tm.Services.Pms.UserModule;
using Tm.Services.Pms.UserRole;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolePermissionsController : BaseAPIController
	{
		#region Fields
		private readonly ILocalizationService _localizationService;
		private readonly IRolePermissionModelFactory _rolePermissionModelFactory;
		private readonly IRoleModulePermissionMappingService _roleModulePermissionMappingService;
		private readonly IUserModuleService _userModuleService;
		private readonly IUserRoleService _userRoleService;
		private readonly IPermissionModuleRecordService _permissionModuleRecordService;
		private readonly TmConfig _tmConfig;
		#endregion

		#region Ctor
		public RolePermissionsController(
			ILocalizationService localizationService,
			IRolePermissionModelFactory rolePermissionModelFactory,
			IRoleModulePermissionMappingService roleModulePermissionMappingService,
			IUserModuleService userModuleService,
			IUserRoleService userRoleService,
			IPermissionModuleRecordService permissionModuleRecordService,
			TmConfig tmConfig)
		{
			_localizationService = localizationService;
			_rolePermissionModelFactory = rolePermissionModelFactory;
			_roleModulePermissionMappingService = roleModulePermissionMappingService;
			_userModuleService = userModuleService;
			_userRoleService = userRoleService;
			_permissionModuleRecordService = permissionModuleRecordService;
			_tmConfig = tmConfig;
		}
		#endregion

		#region Method

		// Filter Role And Modules
		[Route("getFilterRolesModules")]
		[HttpGet]
		public async Task<BaseResponseModel> GetAllRoleModulePermissions()
		{
			try
			{
				FilterRoleModuleModel filterRoleModuleModel = new FilterRoleModuleModel();

				var userModuleList = await _userModuleService.GetAllUserModules();
				filterRoleModuleModel.ModuleList.AddRange(userModuleList.Select(userModule =>
					new SelectListItem { Text = userModule.Name.ToString(), Value = userModule.Id.ToString() }));

				var userRolesList = await _userRoleService.GetAllUserRoles();
				filterRoleModuleModel.RoleList.AddRange(userRolesList.Select(userRole =>
					new SelectListItem { Text = userRole.Name.ToString(), Value = userRole.Id.ToString() }));

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), filterRoleModuleModel);

			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


        [Route("GetAllRoleModulePermissionFilterByRoleAndModuleMapping")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllRoleModulePermissionFilterByRoleAndModuleMapping(int roleId, int moduleId, int pageIndex, int pageSize)
        {
            try
            {
				// Get the RoleModulePermissionListModel
                var allRoleModulePermissions = await _rolePermissionModelFactory.GetAllRoleModulePermissionFilterByRoleAndModuleMapping(roleId, moduleId, pageIndex, pageSize);

                if (allRoleModulePermissions == null || !allRoleModulePermissions.RoleModulePermissionList.Any())
                {
                    // If RoleModulePermissionList is null or empty
                    return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
                }

                // Calculate TotalItems and TotalPages based on the RoleModulePermissionList
                int TotalItems = allRoleModulePermissions.RoleModulePermissionList.Count();
                int TotalPages = (int)Math.Ceiling((double)TotalItems / pageSize);

                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = allRoleModulePermissions.RoleModulePermissionList
                };

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }



        //Post Role Module Permissions Read and Edit
        [Route("updateRoleModulePermissionsReadEdit")]
		[HttpPost]
		public async Task<BaseResponseModel> UpdateRoleModulePermissionsReadEdit([FromBody] IList<RoleModulePermissionRequestModel> roleModulePermissionModelList)
		{
			try
			{

				if (roleModulePermissionModelList == null || !roleModulePermissionModelList.Any())
				{
					// If Roles ,Module and Permissions are null or empty
					return ErrorResponse("No data found.", HttpStatusCode.NotFound);
				}

				foreach (var roleModulePermissionModel in roleModulePermissionModelList)
				{
					var roleModulePermission = await _roleModulePermissionMappingService.GetRoleModulePermissionMappingById(roleModulePermissionModel.RoleModulePermissionId);
					if (roleModulePermission == null)
					{
						// If role module permission are null or empty
						return ErrorResponse("No role module permission.", HttpStatusCode.NotFound);
					}
					else
					{
						roleModulePermission.Id = roleModulePermissionModel.RoleModulePermissionId;
						roleModulePermission.UserRoleId = roleModulePermission.UserRoleId;
						roleModulePermission.PermissionModuleRecordId = roleModulePermission.PermissionModuleRecordId;
						roleModulePermission.Read = roleModulePermissionModel.Read;
						roleModulePermission.Edit = roleModulePermissionModel.Edit;
						roleModulePermission.CreatedBy = 1;
						roleModulePermission.ModifiedOn = DateTime.UtcNow;
						roleModulePermission.ModifiedBy = 1;
						await _roleModulePermissionMappingService.UpdateRoleModulePermissionMapping(roleModulePermission);
					}
				}

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		[Route("getUserPermissionsReadEdit")]
		[HttpPost]
		public async Task<BaseResponseModel> GetUserPermissionsReadEdit([FromBody] UserPermissionsReadEdit userPermissionsReadEdit)
		{
			try
			{
				if (!ModelState.IsValid)
					return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", userPermissionsReadEdit.LanguageId));

				if (userPermissionsReadEdit == null)
				{
					// If Roles ,Module and Permissions are null or empty
					return ErrorResponse("No data found.", HttpStatusCode.NotFound);
				}

				var response = await _roleModulePermissionMappingService.GetFindUserPermissionReadEdit(userId: userPermissionsReadEdit.UserId, moduleName: userPermissionsReadEdit.ModuleName);
				if (response == null)
				{
					return ErrorResponse("No role module permission read and edit.", HttpStatusCode.NotFound);
				}
				else
				{
					var model = new RoleModulePermissionListModel();
					foreach (var item in response)
					{
						var permissionModuleRecord = await _permissionModuleRecordService.GetPermissionModuleRecordById(item.PermissionModuleRecordId);
						if (permissionModuleRecord != null)
						{
							RoleModulePermissionModel roleModulePermissionModel = new RoleModulePermissionModel();
							roleModulePermissionModel.Id = item.Id;
							roleModulePermissionModel.RoleId = item.UserRoleId;
							roleModulePermissionModel.ModuleId = item.UserModuleId;
							roleModulePermissionModel.RoleModulePermissionId = item.PermissionModuleRecordId;
							roleModulePermissionModel.Read = item.Read;
							roleModulePermissionModel.Edit = item.Edit;
							roleModulePermissionModel.RoleModulePermissionName = permissionModuleRecord != null ? permissionModuleRecord.Name : "";
							model.RoleModulePermissionList.Add(roleModulePermissionModel);
						}
					}
					return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), model);
				}
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		//Post Menu Permissions Read and Edit
		[Route("updateMenuPermissionsReadEdit")]
		[HttpPost]
		public async Task<BaseResponseModel> UpdateMenuPermissionsReadEdit([FromBody] IList<RoleMenuPermissionRequestModel> menuPermissionModelList)
		{
			try
			{

				if (menuPermissionModelList == null || !menuPermissionModelList.Any())
				{
					// If Roles ,Module and Permissions are null or empty
					return ErrorResponse("No data found.", HttpStatusCode.NotFound);
				}

				foreach (var menuPermissionModel in menuPermissionModelList)
				{
					var menuPermission = await _roleModulePermissionMappingService.GetMenuPermissionMappingByRoleId(menuPermissionModel.RoleId);
					if (menuPermission == null)
					{
						// If role module permission are null or empty
						return ErrorResponse("No role menu permission.", HttpStatusCode.NotFound);
					}
					else
					{
						menuPermission.Id = menuPermissionModel.Id;
						menuPermission.MenuId = menuPermissionModel.MenuId;
						menuPermission.SubMenuId = menuPermissionModel.SubMenuId;
						menuPermission.ChildMenuId = menuPermissionModel.ChildMenuId;
						menuPermission.Read = menuPermissionModel.Read;
						menuPermission.Edit = menuPermissionModel.Edit;
                        await _roleModulePermissionMappingService.UpdateMenuPermissionMapping(menuPermission);
                    }
				}

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		//[Route("GetAllMenuPermissionFilterByRoleMapping")]
		//[HttpGet]
		//public async Task<BaseResponseModel> GetAllMenuPermissionFilterByRoleMapping(int roleId, int pageIndex, int pageSize)
		//{
		//	try
		//	{
		//		// Get the menuPermissionListModel
		//		var allRoleMenuPermissions = await _rolePermissionModelFactory.GetAllMenuPermissionFilterByRoleAndModuleMapping(roleId, pageIndex, pageSize);

		//		if (allRoleMenuPermissions == null || !allRoleMenuPermissions.RoleMenuPermissionList.Any())
		//		{
		//			// If RoleModulePermissionList is null or empty
		//			return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
		//		}

		//		// Calculate TotalItems and TotalPages based on the RoleMenuPermissionList
		//		int TotalItems = allRoleMenuPermissions.RoleMenuPermissionList.Count();
		//		int TotalPages = (int)Math.Ceiling((double)TotalItems / pageSize);

		//		var response = new
		//		{
		//			TotalItems = TotalItems,
		//			TotalPages = TotalPages,
		//			Page = pageIndex,
		//			PageSize = pageSize,
		//			Results = allRoleMenuPermissions.RoleMenuPermissionList
		//		};

		//		return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorResponse(ex, HttpStatusCode.InternalServerError);
		//	}
		//}

		#endregion Method

	}
}