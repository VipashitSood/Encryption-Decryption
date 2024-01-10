using API.Auth;
using API.Controllers;
using API.Factories.Menus;
using API.Models.BaseModels;
using API.Models.PmsMenu;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Services.Localization;
using Tm.Services.Pms.PmsMenuPerimission;

[Route("api/[controller]")]
[ApiController]
public class MenuController : BaseAPIController
{
    #region Fields
    private readonly ILocalizationService _localizationService;
    private readonly IRoleMenuPermissionService _roleMenuPermissionService;
    private readonly TmConfig _tmConfig;
    private IMenuFactory _menuFactory;
    #endregion

    #region Ctor

    private readonly TokenCreator _tokenCreator;

    public MenuController(
    ILocalizationService localizationService,
    TmConfig tmConfig, IMenuFactory menuFactory, IRoleMenuPermissionService roleMenuPermissionService)
    {
        _roleMenuPermissionService = roleMenuPermissionService;
        _localizationService = localizationService;
        _tmConfig = tmConfig;
        _menuFactory = menuFactory;
    }
    #endregion

    #region Menus

    [HttpGet]
    [Route("GetAllMenusByUserId")]
    public async Task<BaseResponseModel> GetAllMenusWithSubmenusAndChildMenusByUserId(string userId)
    {
        try
        {
            //get menus
            var menus = await _menuFactory.GetAllMenusByUserId(userId);

            //return error if not found
            if (menus == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), menus);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("GetAllMenusByRoleId")]
    public async Task<BaseResponseModel> GetAllMenusWithSubmenusAndChildMenusByRoleId(int roleId)
    {
        try
        {
            //get menus
            var menus = await _menuFactory.GetAllMenusByRoleId(roleId);

            //return error if not found
            if (menus == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), menus);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    //Post Menu Permissions Read and Edit
    [Route("updateMenuPermissionsReadEdit")]
    [HttpPost]
    public async Task<BaseResponseModel> UpdateMenuPermissionsReadEdit([FromBody] IList<MainMenuModel> menuPermissionModelList)
    {
        if (menuPermissionModelList == null)
            return null;
        try
        {
            var updateAllMenu = await _menuFactory.UpdateAllMenu(menuPermissionModelList);
            if (!updateAllMenu.Item1)
                return ErrorResponse(updateAllMenu.Item2, HttpStatusCode.NoContent);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    #endregion Menus

}