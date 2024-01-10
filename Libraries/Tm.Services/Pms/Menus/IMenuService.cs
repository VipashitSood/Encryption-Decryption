using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsMenu;

namespace Tm.Services.Pms.Menus
{
    public interface IMenuService
    {
        Task<IList<MainMenu>> GetAllMenu(int roleId);
        Task<IList<SubMenu>> GetSubMenuByMenuId(int menuId, int roleId);
        Task<IList<ChildMenu>> GetChildMenuBySubMenuId(int subMenuId, int roleId);
        MainMenu GetMenuByIdAndRoleId(int id, int roleId);
        void InsertMenu(MainMenu mainMenu);
        void InsertSubMenu(SubMenu subMenu);
        void InsertChildMenu(ChildMenu childMenu);
        Task<IList<SubMenu>> GetSubMenuByRoleId(int roleId);
        Task<IList<ChildMenu>> GetChildMenuByRoleId(int roleId);
        IList<MainMenu> GetAllMainMenuByRoleId(int roleId);
        Task UpdateMenu(MainMenu mainMenu);
    }
}
