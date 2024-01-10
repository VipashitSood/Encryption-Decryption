using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.PmsMenu
{
    public class MainMenu : BaseEntity
    {
        public string MenuName { get; set; }
        public string PageUrl { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public List<SubMenu> SubMenus { get; set; }
    }

    public class SubMenu : BaseEntity
    {
        public int MenuId { get; set; }
        public string SubMenuName { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public List<ChildMenu> ChildMenus { get; set; }
    }

    public class ChildMenu : BaseEntity
    {
        public int SubMenuId { get; set; }
        public string ChildMenuName { get; set; }
        public string PageUrl { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
