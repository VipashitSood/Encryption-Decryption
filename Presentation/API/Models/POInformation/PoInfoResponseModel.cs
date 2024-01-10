using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace API.Models.POInformation
{
	public class PoInfoResponseModel : BaseRequestModel
    {
        public PoInfoResponseModel()
        {
            OrderModelList = new List<PoInfoResponseOrderModel>();
        }
        /// <summary>
        /// Gets or sets PONumber
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the POAmount
        /// </summary>
        public decimal POAmount { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        ///// <summary>
        ///// Gets or sets the FileName
        ///// </summary>
        public string FileName { get; set; }

        ///// <summary>
        ///// Gets or sets the FileName
        ///// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        ///  Order List Model
        /// </summary>
        public List<PoInfoResponseOrderModel> OrderModelList { get; set; }
    }
}
