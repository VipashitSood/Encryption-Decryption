using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace API.Models.PmsCustomer
{
	public class FilterCustomerModel : BaseResponseModel
    {
        /// <summary>
        /// FilteredOrderModel
        /// </summary>
        public FilterCustomerModel()
        {
            CustomerName = new List<SelectListItem>();
            CompanyName = new List<SelectListItem>();
        }


        /// <summary>
        /// Customers List
        /// </summary>

        public List<SelectListItem> CustomerName { get; set; }
        public List<SelectListItem> CompanyName { get; set; }

    }
}
