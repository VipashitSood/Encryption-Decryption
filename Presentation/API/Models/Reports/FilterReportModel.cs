using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace API.Models.Reports
{
    public class FilterReportModel : BaseResponseModel
    {
        /// <summary>
        /// FilteredOrderModel
        /// </summary>
        public FilterReportModel()
        {
            OrderName = new List<SelectListItem>();
            Months = new List<SelectListItem>();
            Year = new List<SelectListItem>();
            CompanyName = new List<SelectListItem>();
        }


        /// <summary>
        /// Customers List
        /// </summary>

        public List<SelectListItem> OrderName { get; set; }
        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Year { get; set; }
        public List<SelectListItem> CompanyName { get; set; }

    }
}

