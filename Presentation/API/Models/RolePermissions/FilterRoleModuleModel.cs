using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace API.Models.RolePermissions
{
    public class FilterRoleModuleModel : BaseResponseModel
    {

        public FilterRoleModuleModel()
        {
            RoleList = new List<SelectListItem>();
            ModuleList = new List<SelectListItem>();
        }
        /// <summary>
        ///  Role List
        /// </summary>
        public List<SelectListItem> RoleList { get; set; }
        /// <summary>
        /// Module List
        /// </summary>
        public List<SelectListItem> ModuleList { get; set; }

    }
}
