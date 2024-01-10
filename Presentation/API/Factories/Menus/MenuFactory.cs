using API.Models.PmsMenu;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsMenu;
using Tm.Services.Pms.Menus;
using Tm.Services.Pms.PmsMenuPerimission;
using Tm.Services.Pms.RoleModulePermission;
using Tm.Services.Pms.UserRole;

namespace API.Factories.Menus
{
    public class MenuFactory : IMenuFactory
    {
        #region Fields
        private readonly IMenuService _menuService;
        private readonly IRoleModulePermissionMappingService _roleModulePermissionMappingService;
        private readonly IRoleMenuPermissionService _roleMenuPermissionService;
        private readonly IMapper _mapper;
        private IUserRoleService _userRoleService;
        #endregion

        public MenuFactory(IMenuService menuService, IMapper mapper, IUserRoleService userRoleService,
            IRoleModulePermissionMappingService roleModulePermissionMappingService, IRoleMenuPermissionService roleMenuPermissionService)
        {
            _roleMenuPermissionService = roleMenuPermissionService;
            _menuService = menuService;
            _mapper = mapper;
            _userRoleService = userRoleService;
            _roleModulePermissionMappingService = roleModulePermissionMappingService;
        }
        /// <summary>
        /// Get All Menus based on userId Async
        /// </summary>
        /// <returns>Menu</returns>
        public async Task<List<Models.PmsMenu.MainMenuModelList>> GetAllMenusByUserId(string userId)
        {
            var menuModelList = new List<MainMenuModel>();
            var mainMenuModelList = new List<MainMenuModelList>();
            var listMenu = new List<MainMenuModel>();
            var allUserRolesByUserId = _userRoleService.GetUserRoleIdsByUserId(userId);
            foreach (var roleId in allUserRolesByUserId)
            {
                var menus = await _menuService.GetAllMenu(roleId);
                foreach (var menu in menus)
                {
                    var model = new MainMenuModel();
                    model.Id = menu.Id;
                    model.MenuName = menu.MenuName;
                    model.PageUrl = menu.PageUrl;
                    model.IsEdit = menu.Edit;
                    model.IsRead = menu.Read;
                    model.RoleId = menu.RoleId;
                    var submenus = await _menuService.GetSubMenuByMenuId(menu.Id, roleId);
                    model.SubMenus = new List<Models.PmsMenu.SubMenu>();
                    foreach (var subMenu in submenus)
                    {
                        var subMenuModel = new Models.PmsMenu.SubMenu();
                        subMenuModel.Id = subMenu.Id;
                        subMenuModel.MenuId = subMenu.MenuId;
                        subMenuModel.SubMenuName = subMenu.SubMenuName;
                        subMenuModel.IsEdit = subMenu.Edit;
                        subMenuModel.RoleId = subMenu.RoleId;
                        subMenuModel.IsRead = subMenu.Read;
                        subMenuModel.ChildMenus = new List<Models.PmsMenu.ChildMenu>();

                        var childMenus = await _menuService.GetChildMenuBySubMenuId(subMenu.Id, roleId);
                        foreach (var childMenu in childMenus)
                        {
                            var childMenuModel = new Models.PmsMenu.ChildMenu();
                            childMenuModel.Id = childMenu.Id;
                            childMenuModel.SubMenuId = childMenu.SubMenuId;
                            childMenuModel.ChildMenuName = childMenu.ChildMenuName;
                            childMenuModel.IsEdit = childMenu.Edit;
                            childMenuModel.IsRead = childMenu.Read;
                            childMenuModel.PageUrl = childMenu.PageUrl;
                            childMenuModel.RoleId = childMenu.RoleId;
                            subMenuModel.ChildMenus.Add(childMenuModel);
                        }
                        model.SubMenus.Add(subMenuModel);
                    }

                    menuModelList.Add(model);
                }
                if (menus.Count() != 0)
                {
                    mainMenuModelList.Add(new MainMenuModelList() { MenuModelList = menuModelList });
                    menuModelList = new List<MainMenuModel>();
                }
            }
            //var result = _mapper.Map<List<Models.PmsMenu.MainMenuModel>>(mainMenuModelList);
            return mainMenuModelList;
        }

        /// <summary>
        /// Get All Menus based on roleId Async
        /// </summary>
        /// <returns>Menu</returns>
        public async Task<List<Models.PmsMenu.MainMenuModelList>> GetAllMenusByRoleId(int roleId)
        {
            var menuModelList = new List<MainMenuModel>();
            var mainMenuModelList = new List<MainMenuModelList>();
            var menus = await _menuService.GetAllMenu(roleId);
            foreach (var menu in menus)
            {
                var model = new MainMenuModel();
                model.Id = menu.Id;
                model.MenuName = menu.MenuName;
                model.PageUrl = menu.PageUrl;
                model.IsEdit = menu.Edit;
                model.IsRead = menu.Read;
                model.RoleId = roleId;
                var submenus = await _menuService.GetSubMenuByMenuId(menu.Id, roleId);
                model.SubMenus = new List<Models.PmsMenu.SubMenu>();
                foreach (var subMenu in submenus)
                {
                    var subMenuModel = new Models.PmsMenu.SubMenu();
                    subMenuModel.Id = subMenu.Id;
                    subMenuModel.MenuId = subMenu.MenuId;
                    subMenuModel.SubMenuName = subMenu.SubMenuName;
                    subMenuModel.IsEdit = subMenu.Edit;
                    subMenuModel.RoleId = subMenu.RoleId;
                    subMenuModel.IsRead = subMenu.Read;
                    subMenuModel.ChildMenus = new List<Models.PmsMenu.ChildMenu>();

                    var childMenus = await _menuService.GetChildMenuBySubMenuId(subMenu.Id, roleId);
                    foreach (var childMenu in childMenus)
                    {
                        var childMenuModel = new Models.PmsMenu.ChildMenu();
                        childMenuModel.Id = childMenu.Id;
                        childMenuModel.SubMenuId = childMenu.SubMenuId;
                        childMenuModel.ChildMenuName = childMenu.ChildMenuName;
                        childMenuModel.IsEdit = childMenu.Edit;
                        childMenuModel.IsRead = childMenu.Read;
                        childMenuModel.PageUrl = childMenu.PageUrl;
                        childMenuModel.RoleId = childMenu.RoleId;
                        subMenuModel.ChildMenus.Add(childMenuModel);
                    }
                    model.SubMenus.Add(subMenuModel);
                }

                menuModelList.Add(model);
            }
            mainMenuModelList.Add(new MainMenuModelList() { MenuModelList = menuModelList });

            //var result = _mapper.Map<List<Models.PmsMenu.MainMenuModel>>(menuModelList);
            return mainMenuModelList;
        }

        public virtual async Task<(bool, string)> UpdateAllMenu(IList<MainMenuModel> menuPermissionModelList)
        {
            try
            {
                foreach (var menu in menuPermissionModelList)
                {
                    var mainMenu = _menuService.GetMenuByIdAndRoleId(menu.Id, menu.RoleId);
                    var model = new MainMenu();
                    mainMenu.Id = menu.Id;
                    mainMenu.MenuName = menu.MenuName;
                    mainMenu.PageUrl = menu.PageUrl;
                    mainMenu.Edit = menu.IsEdit;
                    mainMenu.Read = menu.IsRead;
                    if (menu != null)
                    {
                        _roleMenuPermissionService.UpdateMenu(mainMenu);
                    }
                    //var submenus = await _menuService.GetSubMenuByMenuId(menu.Id, menu.RoleId);
                    //model.SubMenus = new List<Tm.Core.Domain.Pms.PmsMenu.SubMenu>();
                    foreach (var subMenu in menu.SubMenus)
                    {
                        var subMenuModel = new Tm.Core.Domain.Pms.PmsMenu.SubMenu();
                        subMenuModel.Id = subMenu.Id;
                        subMenuModel.MenuId = subMenu.MenuId;
                        subMenuModel.SubMenuName = subMenu.SubMenuName;
                        subMenuModel.RoleId = subMenu.RoleId;
                        subMenuModel.Edit = subMenu.IsEdit;
                        subMenuModel.Read = subMenu.IsRead;
                        if (subMenuModel != null)
                        {
                            await _roleMenuPermissionService.UpdateSubMenu(subMenuModel);
                        }
                        subMenuModel.ChildMenus = new List<Tm.Core.Domain.Pms.PmsMenu.ChildMenu>();
                        var childMenus = menu.SubMenus?.Select(x => x.ChildMenus).ToList();
                        foreach (var childMenu in childMenus)
                        {
                            foreach (var item in childMenu)
                            {
                                var childMenuModel = new Tm.Core.Domain.Pms.PmsMenu.ChildMenu();
                                childMenuModel.Id = item.Id;
                                childMenuModel.SubMenuId = item.SubMenuId;
                                childMenuModel.ChildMenuName = item.ChildMenuName;
                                childMenuModel.Edit = item.IsEdit;
                                childMenuModel.Read = item.IsRead;
                                childMenuModel.RoleId = item.RoleId;
                                childMenuModel.PageUrl = item.PageUrl;
                                if (childMenuModel != null)
                                {
                                    await _roleMenuPermissionService.UpdateChildMenu(childMenuModel);
                                }
                            }
                        }
                    }
                }
                return (true, "All Menu updated successfully.");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
