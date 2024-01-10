using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace API.Models.Orders
{
	public class FilteredOrderModel : BaseResponseModel
    {
        /// <summary>
        /// FilteredOrderModel
        /// </summary>
        public FilteredOrderModel()
        {
            Companys = new List<SelectListItem>();
            ProjectTypes = new List<SelectListItem>();
            OrderName = new List<SelectListItem>();
            IsPORequired = new List<SelectListItem>();
            InHouse = new List<SelectListItem>();
        }


        /// <summary>
        /// Customers List
        /// </summary>

        public List<SelectListItem> Companys { get; set; }
        public List<SelectListItem> ProjectTypes { get; set; }
        public List<SelectListItem> OrderName { get; set; }
        public List<SelectListItem> IsPORequired { get; set; }
        public List<SelectListItem> InHouse { get; set; }

    }
}
