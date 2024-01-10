using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsMenu;
using Tm.Data;

namespace Tm.Services.Pms.Menus
{
    public class MenuService : IMenuService
    {
        #region Fields
        private readonly IRepository<MainMenu> _menusRepository;
        private readonly IRepository<SubMenu> _subMenuRepository;
        private readonly IRepository<ChildMenu> _childMenuRepository;
        #endregion

        #region Ctor

        public MenuService(IRepository<MainMenu> menuRepository,
            IRepository<SubMenu> subMenuRepository,
            IRepository<ChildMenu> childMenuRepository)
        {
            _menusRepository = menuRepository;
            _subMenuRepository = subMenuRepository;
            _childMenuRepository = childMenuRepository;
        }

        #endregion

        /// <summary>
        /// Get all Menu
        /// </summary>
        /// <returns>Menu</returns>
        public virtual async Task<IList<MainMenu>> GetAllMenu(int roleId)
        {
            var query = await _menusRepository.Table.Where(x => x.RoleId == roleId && !x.IsDeleted).ToListAsync();
            return query;
        }

        public MainMenu GetMenuByIdAndRoleId(int id, int roleId)
        {
            var query = _menusRepository.Table.Where(x => x.Id == id && x.RoleId == roleId && !x.IsDeleted).FirstOrDefault();
            return query;
        }

        public virtual async Task<IList<SubMenu>> GetSubMenuByMenuId(int menuId, int roleId)
        {
            var query = await _subMenuRepository.Table.Where(x => x.MenuId == menuId && x.RoleId == roleId && !x.IsDeleted).ToListAsync();
            return query;
        }

        public virtual async Task<IList<SubMenu>> GetSubMenuByRoleId(int roleId)
        {
            var query = await _subMenuRepository.Table.Where(x => x.RoleId == roleId && !x.IsDeleted).ToListAsync();
            return query;
        }
        public virtual async Task<IList<ChildMenu>> GetChildMenuBySubMenuId(int subMenuId, int roleId)
        {
            var query = await _childMenuRepository.Table.Where(x => x.SubMenuId == subMenuId && x.RoleId == roleId && !x.IsDeleted).ToListAsync();
            return query;
        }
        public virtual async Task<IList<ChildMenu>> GetChildMenuByRoleId(int roleId)
        {
            var query = await _childMenuRepository.Table.Where(x => x.RoleId == roleId && !x.IsDeleted).ToListAsync();
            return query;
        }
        public void InsertMenu(MainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

             _menusRepository.Insert(mainMenu);
        }
        public virtual async Task UpdateMenu(MainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

            await _menusRepository.UpdateAsync(mainMenu);
        }
        public void InsertSubMenu(SubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

             _subMenuRepository.Insert(subMenu);
        }
        public void InsertChildMenu(ChildMenu childMenu)
        {
            if (childMenu == null)
                throw new ArgumentNullException(nameof(childMenu));

             _childMenuRepository.Insert(childMenu);
        }

        public IList<MainMenu> GetAllMainMenuByRoleId(int roleId)
        {
            var query = _menusRepository.Table.Where(x => x.RoleId == roleId).ToList();
            return query;
        }

    }
}
