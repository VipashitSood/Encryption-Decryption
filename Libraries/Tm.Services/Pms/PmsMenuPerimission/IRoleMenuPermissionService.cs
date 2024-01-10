using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsMenu;

namespace Tm.Services.Pms.PmsMenuPerimission
{
    public interface IRoleMenuPermissionService
    {
        /// <summary>
        /// Get All Role Menu Permission
        /// </summary>
        /// <returns></returns>
        Task<MainMenu> GetAllRoleMenuPermissionByRoleId(int roleId);
        void UpdateMenu(MainMenu mainMenu);
        Task UpdateSubMenu(SubMenu mainMenu);
        Task UpdateChildMenu(ChildMenu mainMenu);
    }
}
