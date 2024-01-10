using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsMenu;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.PmsMenuPerimission
{
    public class RoleMenuPermissionService : IRoleMenuPermissionService
    {
        #region Fields
        private readonly IRepository<MainMenu> _menuItemService;
        private readonly IRepository<SubMenu> _subMenuService;
        private readonly IRepository<ChildMenu> _childMenuService;
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Ctor
        public RoleMenuPermissionService(IRepository<MainMenu> menuItemService, IEventPublisher eventPublisher,
            IRepository<SubMenu> subMenuService, IRepository<ChildMenu> childMenuService)
        {
            _childMenuService = childMenuService;
            _subMenuService = subMenuService;
            _menuItemService = menuItemService;
            _eventPublisher = eventPublisher;
        }
        #endregion
        public virtual async Task<MainMenu> GetAllRoleMenuPermissionByRoleId(int Id)
        {
            if (Id <= 0)
                return null;

            var roleMenuPermission = await _menuItemService.GetByIdAsync(Id);

            if (roleMenuPermission != null)
            {
                return roleMenuPermission;
            }
            return null;
        }

        /// <summary>
        /// Update Menu Permission Mapping
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateMenu(MainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

             _menuItemService.UpdateAsync(mainMenu);

            //event notification
            _eventPublisher.EntityUpdated(mainMenu);
        }

        public virtual async Task UpdateSubMenu(SubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            await _subMenuService.UpdateAsync(subMenu);

            //event notification
            _eventPublisher.EntityUpdated(subMenu);
        }
        public virtual async Task UpdateChildMenu(ChildMenu childMenu)
        {
            if (childMenu == null)
                throw new ArgumentNullException(nameof(childMenu));

            await _childMenuService.UpdateAsync(childMenu);

            //event notification
            _eventPublisher.EntityUpdated(childMenu);
        }
    }
}
