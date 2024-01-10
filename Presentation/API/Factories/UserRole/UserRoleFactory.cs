using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.UserRole;
using AutoMapper;
using Tm.Core;
using Tm.Services.Customers;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.UserRole;
using System.Linq;
using API.Models.UserRole;
using Tm.Framework.Models.Extensions;
using API.Factories.Menus;
using API.Models.PmsMenu;
using Tm.Services.Pms.Menus;
using Tm.Core.Domain.Pms.PmsMenu;

namespace API.Factories.UserRole
{
    public class UserRoleFactory : IUserRoleFactory
    {
        #region Fields
        private readonly IUserRoleService _roleService;
        private readonly IMenuFactory _menuFactory;
        private readonly IMenuService _menuService;
        private readonly IMasterDataService _masterDataService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public UserRoleFactory(IUserRoleService roleService,
            IMasterDataService masterDataService,
            IWorkContext workContext,
            ICustomerService customerService,
            IMapper mapper, IMenuFactory menuFactory,
            IMenuService menuService)
        {
            _roleService = roleService;
            _menuService = menuService;
            _menuFactory = menuFactory;
            _masterDataService = masterDataService;
            _workContext = workContext;
            _customerService = customerService;
            _mapper = mapper;
        }
        #endregion

        #region Utilities
        private string GetCustomerUsername(int customerId)
        {
            try
            {
                var customer = _customerService.GetCustomerById(customerId);
                return customer?.Username ?? ConstantValues.UnknownUser;
            }
            catch (Exception ex)
            {
                //get unknow user if user isn't register
                return ConstantValues.UnknownUser;
            }
        }

        #endregion

        #region ADUser

        /// <summary>
        /// Get all ADUsers
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<ADUser>> GetAllUserByAD()
        {
            var res = await _roleService.GetAllUserByAD();

            var result = _mapper.Map<List<ADUser>>(res);

            return result;
        }

        /// <summary>
        /// Save ad user
        /// </summary>
        /// <param name="adUser"></param>
        /// <returns></returns>
        public async Task SaveADUserAsync(ADUserModel model)
        {
            var existingADUser = await _roleService.GetADUserByADUserId(model.UserId);
            if (existingADUser == null)
            {
                var adUser = _mapper.Map<ADUser>(model);
                adUser.CreatedOn = DateTime.Now;
                adUser.IsDeleted = false;

                await _roleService.InsertADUser(adUser);
            }
            else
            {
                existingADUser.Name = model.Name;
                existingADUser.LastName = model.LastName;
                existingADUser.DisplayName = model.DisplayName;
                existingADUser.Email = model.Email;
                existingADUser.JobTitle = model.JobTitle;
                existingADUser.Mobile = model.Mobile;
                existingADUser.ModifiedOn = DateTime.Now;
                existingADUser.IsDeleted = false;

                await _roleService.UpdateADUser(existingADUser);
            }
        }

        #endregion

        #region UserRoles

        /// <summary>
        /// GetAllUserRoles UserRole
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<UserRoles>> GetAllUserRoles()
        {
            var data = await _roleService.GetAllUserRoles();

            var result = data.Select(x => new UserRoles

            {
                Id = x.Id,
                ModifiedBy = x.ModifiedBy,
            }).ToList();

            return result = data
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        /// <summary>
        /// GetUserRoleById UserRole
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<UserADRoles> GetUserRoleById(int userRoleId)
        {

            UserADRoles userADRoles = new UserADRoles();
            var userRole = await _roleService.GetUserRoleById(userRoleId);

            if (userRole != null && !userRole.IsDeleted)
            {
                userADRoles.Id = userRole.Id;
                userADRoles.Name = userRole.Name;
                // Assuming you have a method in your _roleService to get ADUserRoleMappings by UserRoleId.
                var userRoleMappings = await _roleService.GetAllUserRoleMapping(userRoleId);

                if (userRoleMappings != null && userRoleMappings.Any())
                {
                    var userRoleList = new List<ADUserRoleList>();

                    foreach (var userRoleMapping in userRoleMappings)
                    {
                        // Assuming you have a method in your _roleService to get ADUser by UserId.
                        var adUser = await _roleService.GetADUserByADUserId(userRoleMapping.ADUserId);

                        if (adUser != null)
                        {
                            var userRoleModel = new ADUserRoleList()
                            {
                                UserRoleId = userRole.Id,
                                Name = adUser.DisplayName,
                                CreatedBy = userRole.CreatedBy,
                                CreatedOn = userRole.CreatedOn,
                                ModifiedBy = userRole.ModifiedBy,
                                ModifiedOn = userRole.ModifiedOn,
                                IsDeleted = userRole.IsDeleted
                            };

                            userRoleModel.ADUserId = userRoleMapping.ADUserId; // Set the ADUserId
                            userRoleList.Add(userRoleModel);
                        }
                    }

                    userADRoles.ADUserRoleList = userRoleList;
                }
                return userADRoles;
            }

            throw new Exception(ConstantValues.RecordNotFound);
        }

        /// <summary>
        /// GetUserRoleById UserRole
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<UserRoles> GetUsersRoleById(int id)
        {
            var userRole = await _roleService.GetUsersRoleById(id);

            if (userRole != null && !userRole.IsDeleted)
            {
                var userRoleMappings = (await _roleService.GetAllUserRoleMapping(id, null))?.ToList();
                var userRoleList = new List<ADUserRoleList>();

                foreach (var userRoleMapping in userRoleMappings)
                {
                    // Check if the ADUser is not deleted
                    var adUser = await _roleService.GetADUserByADUserId(userRoleMapping.ADUserId);

                    var userName = adUser != null ? adUser.Name : "";

                    var userRoleModel = new ADUserRoleList()
                    {
                        UserRoleId = userRoleMapping.Id,
                        ADUserId = userRoleMapping.ADUserId,
                        CreatedBy = userRoleMapping.CreatedBy,
                        CreatedOn = userRoleMapping.CreatedOn,
                        ModifiedBy = userRoleMapping.ModifiedBy,
                        ModifiedOn = userRoleMapping.ModifiedOn,
                        IsDeleted = userRoleMapping.IsDeleted
                    };

                    userRoleList.Add(userRoleModel);
                }


                userRole.ADUserRoleList = userRoleList;

                return userRole;
            }

            throw new Exception(ConstantValues.RecordNotFound);
        }

        /// <summary>
        /// Delete UserRole
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteUserRoleAndMenuById(int id)
        {
            if (id > 0)
            {
                var userRoles = await _roleService.GetUsersRoleById(id);
                if (userRoles != null)
                {
                    userRoles.ModifiedOn = DateTime.UtcNow;
                    userRoles.IsDeleted = true;
                    await _roleService.UpdateUserRole(userRoles);

                    //Get Menus By Role Id
                    var mainMenus = _menuService.GetAllMenu(id);
                    if (mainMenus != null)
                    {
                        foreach (var mainMenu in mainMenus.Result)
                        {
                            mainMenu.IsDeleted = true;
                        }
                    }
                    var subMenus = _menuService.GetSubMenuByRoleId(id);
                    if (subMenus != null)
                    {
                        foreach (var subMenu in subMenus.Result)
                        {
                            subMenu.IsDeleted = true;
                        }
                    }
                    var childMenus = _menuService.GetChildMenuByRoleId(id);
                    if (childMenus != null)
                    {
                        foreach (var childMenu in childMenus.Result)
                        {
                            childMenu.IsDeleted = true;
                        }
                    }
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }
            }
            else
            {
                return (false, ConstantValues.RecordNotFound);
            }
        }


        /// <summary>
        /// Insert UserRoles
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>

        //Create UserRoles
        public async Task<(bool, string, int)> CreateUserRole(UserRolesModel model)
        {
            if (model.Id > 0)
            {
                return (false, ConstantValues.UnknownUser, -1);
            }

            var existingUserRole = await _roleService.GetUserRoleByName(model.Name);
            if (existingUserRole != null)
            {
                return (false, ConstantValues.DuplicateUserRole, -1);
            }
            else
            {
                var newUserRole = _mapper.Map<UserRoles>(model);
                newUserRole.CreatedOn = DateTime.UtcNow;
                newUserRole.IsDeleted = false;
                await _roleService.InsertUserRole(newUserRole);
                foreach (var item in model.ADUserList)
                {
                    var alreadyexist = await _roleService.GetAllUserRoleMapping(roleId: newUserRole.Id, userId: item);
                    if (alreadyexist.Count == 0)
                    {
                        ADUserRoleMapping mapping = new ADUserRoleMapping();
                        mapping.UserRoleId = newUserRole.Id;
                        mapping.ADUserId = item;
                        mapping.CreatedOn = DateTime.UtcNow;
                        mapping.IsDeleted = false;
                        await _roleService.InsertUserRoleMapping(mapping);
                    }
                }
                //Create menu, submenu and child menu based on roleId
                int loopCount = 0;
                var menus = await _menuFactory.GetAllMenusByUserId(model.ADUserList.FirstOrDefault());
                if (menus.Count() == 0)
                {
                    //get default menu if not have any user role access
                    menus = await _menuFactory.GetAllMenusByRoleId(1);
                }
                foreach (var menu in menus.FirstOrDefault().MenuModelList)
                {
                    var mainMenuModel = new MainMenu();
                    mainMenuModel.MenuName = menu.MenuName;
                    mainMenuModel.PageUrl = menu.PageUrl;
                    mainMenuModel.Edit = false; 
                    mainMenuModel.Read = false;
                    mainMenuModel.RoleId = newUserRole.Id;
                    mainMenuModel.IsDeleted = false;
                    _menuService.InsertMenu(mainMenuModel);

                    if (loopCount < 2)
                    {
                        loopCount = loopCount + 1;
                        var submenus = await _menuService.GetSubMenuByMenuId(menu.Id, menu.RoleId);
                        foreach (var subMenu in submenus)
                        {
                            var subMenuModel = new Tm.Core.Domain.Pms.PmsMenu.SubMenu();
                            subMenuModel.MenuId = mainMenuModel.Id;
                            subMenuModel.SubMenuName = subMenu.SubMenuName;
                            subMenuModel.Edit = false;
                            subMenuModel.Read = false;
                            subMenuModel.RoleId = newUserRole.Id;
                            subMenuModel.IsDeleted = false;
                            _menuService.InsertSubMenu(subMenuModel);
                            var childMenus = await _menuService.GetChildMenuBySubMenuId(subMenu.Id, subMenu.RoleId);
                            foreach (var childMenu in childMenus)
                            {
                                var childMenuModel = new Tm.Core.Domain.Pms.PmsMenu.ChildMenu();
                                childMenuModel.ChildMenuName = childMenu.ChildMenuName;
                                childMenuModel.SubMenuId = subMenuModel.Id;
                                childMenuModel.Edit = false;
                                childMenuModel.Read = false;
                                childMenuModel.PageUrl = childMenu.PageUrl;
                                childMenuModel.RoleId = newUserRole.Id;
                                childMenuModel.IsDeleted = false;
                                _menuService.InsertChildMenu(childMenuModel);
                            }
                        }
                    }
                }
                return (true, ConstantValues.Success, newUserRole.Id);
            }
        }


        /// <summary>
        /// Update UserRoles
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns> 

        //Update UserRoles
        public async Task<(bool, string, int)> UpdateUserRole(UserRolesModel model)
        {
            if (model.Id > 0)
            {


                var userRoles = await _roleService.GetUsersRoleById(model.Id);
                if (userRoles != null)
                {
                    // Update the properties of userRoles
                    userRoles.Name = model.Name;
                    userRoles.ModifiedBy = model.ModifiedBy;
                    userRoles.ModifiedOn = DateTime.UtcNow;
                    userRoles.IsDeleted = false;

                    // Update the UserRole
                    await _roleService.UpdateUserRole(userRoles);

                    // Get the existing ADUserMappings for this UserRole
                    var existingMappings = await _roleService.GetAllUserRoleMapping(roleId: userRoles.Id);

                    foreach (var item in model.ADUserList)
                    {
                        // Check if the ADUser is already mapped
                        var existingMapping = existingMappings.FirstOrDefault(mapping => mapping.ADUserId == item);

                        if (existingMapping != null)
                        {
                            // ADUser is already mapped, no need to create a new mapping
                            existingMapping.IsDeleted = false; // Set IsDeleted to false
                            await _roleService.UpdateUserRoleMapping(existingMapping);
                        }
                        else
                        {
                            var newMapping = new ADUserRoleMapping
                            {
                                UserRoleId = userRoles.Id,
                                ADUserId = item,
                                CreatedOn = DateTime.UtcNow,
                                IsDeleted = false
                            };

                            await _roleService.InsertUserRoleMapping(newMapping);
                        }
                    }
                    foreach (var existingMapping in existingMappings)
                    {
                        if (!model.ADUserList.Contains(existingMapping.ADUserId))
                        {
                            existingMapping.IsDeleted = true; // Set IsDeleted to true
                            await _roleService.UpdateUserRoleMapping(existingMapping);
                        }
                    }
                    return (true, ConstantValues.Success, userRoles.Id);
                }
                else
                {
                    return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                }
            }
            else
            {
                return (false, "Invalid input.", 0);
            }
        }
        //Get User By Name
        public async Task<UserRoles> GetUserRoleByName(string name)
        {
            var userRole = await _roleService.GetUserRoleByName(name);
            var result = _mapper.Map<UserRoles>(userRole);
            return result;
        }



        #endregion UserRoles

        //#region Role

        //public async Task SaveRoleAsync(ADUserModel adModel)
        //{
        //    await _roleService.SaveUserRolesAsync(_mapper.Map<ADUser>(adModel));
        //}

        //#endregion

        #region AdUserRoleMapping

        public async Task<IList<ADUserRoleListModel>> GetAllUserRoleMapping(int roleId, string userId)
        {
            var userRoleMappings = (await _roleService.GetAllUserRoleMapping(roleId, userId))?.ToList();
            var userRoleListModel = new List<ADUserRoleListModel>();

            //prepare user role mappings
            foreach (var userRoleMapping in userRoleMappings)
            {
                var selectedRoleIds =  _roleService.GetUserRoleIdsByUserId(userRoleMapping.ADUserId);
                var userName = (await _roleService.GetADUserByADUserId(userRoleMapping.ADUserId))?.DisplayName;

                var userRoleModel = new ADUserRoleListModel()
                {
                    Id = userRoleMapping.Id,
                    UserId = userRoleMapping.ADUserId,
                    UserName = userName
                };

                //map selected role with user
                foreach (var selectedRoleId in selectedRoleIds)
                {
                    var existingUserRoleMapping = (await _roleService.GetAllUserRoleMapping(selectedRoleId, userRoleMapping.ADUserId))?.FirstOrDefault();
                    if (existingUserRoleMapping == null)
                        continue;

                    userRoleModel.RoleModels.Add(new RoleModel()
                    {
                        Id = selectedRoleId,
                        IsSelected = existingUserRoleMapping.IsDeleted == false
                    });
                }
                if (userRoleModel.RoleModels.Any(x => x.IsSelected == true))
                {
                    userRoleListModel.Add(userRoleModel);
                }
            }

            return userRoleListModel.OrderBy(x => x.UserName).ToList();
        }

        public async Task<(bool, string)> SaveUserRoleMapping(ADUserRoleMappingModel model)
        {

            foreach (var adUserId in model.ADUserIds)
            {
                var existingUserRoleMapping = (await _roleService.GetAllUserRoleMapping(model.UserRoleId, adUserId))?.FirstOrDefault();
                if (existingUserRoleMapping != null)
                {
                    existingUserRoleMapping.IsDeleted = false;
                    await _roleService.UpdateUserRoleMapping(existingUserRoleMapping);
                }
                else
                {
                    var userRoleMapping = _mapper.Map<ADUserRoleMapping>(model);

                    userRoleMapping.ADUserId = adUserId;
                    userRoleMapping.CreatedOn = DateTime.UtcNow;
                    userRoleMapping.IsDeleted = false;
                    await _roleService.InsertUserRoleMapping(userRoleMapping);
                }

            }

            return (true, ConstantValues.Success);
        }

    }
}
#endregion UserRoles


