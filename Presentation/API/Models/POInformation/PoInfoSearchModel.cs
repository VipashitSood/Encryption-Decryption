using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace API.Models.POInformation
{
	public class PoInfoSearchModel : BaseRequestModel
    {
        /// <summary>
        /// PONumber
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// POAmount
        /// </summary>
        public decimal POAmount { get; set; }

        /// <summary>
        /// ConsumedAmount
        /// </summary>
        public decimal ConsumedAmount { get; set; }

        /// <summary>
        /// RemainingAmount
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// ClientName
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// OrderName
        /// </summary>
        public string OrderName { get; set; }
        public int CustomerId { get; set; }
    }
}
