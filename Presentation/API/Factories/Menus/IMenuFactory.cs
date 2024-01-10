using API.Models.PmsMenu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Factories.Menus
{
    public interface IMenuFactory
    {
        Task<List<Models.PmsMenu.MainMenuModelList>> GetAllMenusByUserId(string userId);
        Task<List<Models.PmsMenu.MainMenuModelList>> GetAllMenusByRoleId(int roleId);
        Task<(bool, string)> UpdateAllMenu(IList<MainMenuModel> menuPermissionModelList);
    }
}
