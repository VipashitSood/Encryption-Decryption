using System.Collections.Generic;

namespace API.Models.PmsMenu
{

    public class MainMenuModelList
    {
        public MainMenuModelList()
        {
            MenuModelList = new List<MainMenuModel>();
        }
        public List<MainMenuModel> MenuModelList { get; set; }
    }
        public class MainMenuModel
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public bool IsRead { get; set; }
        public bool IsEdit { get; set; }
        public string PageUrl { get; set; }
        public int RoleId { get; set; }
        public List<SubMenu> SubMenus { get; set; }
    }

    public class SubMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string SubMenuName { get; set; }
        public bool IsRead { get; set; }
        public bool IsEdit { get; set; }
        public int RoleId { get; set; }
        public List<ChildMenu> ChildMenus { get; set; }
    }

    public class ChildMenu
    {
        public int Id { get; set; }
        public int SubMenuId { get; set; }
        public string ChildMenuName { get; set; }
        public string PageUrl { get; set; }
        public bool IsRead { get; set; }
        public bool IsEdit { get; set; }
        public int RoleId { get; set; }
    }
}
