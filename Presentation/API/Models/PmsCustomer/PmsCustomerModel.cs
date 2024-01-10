using API.Models.BaseModels;
using API.Models.Orders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace API.Models.Customer
{
	public class PmsCustomerModel :  BaseRequestModel
    {
		public PmsCustomerModel()
		{
			OrderModelList = new List<CustomOrderModel>();
		}
		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNo
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the MSA
        /// </summary>
        public string MSABase64 { get; set; }

        /// <summary>
        /// Gets or sets the MSA
        /// </summary>
        public string MSAFileName { get; set; }

        /// <summary>
        /// Gets or sets the NDA
        /// </summary>
        public string NDABase64 { get; set; }
        /// <summary>
        /// Gets or sets the NDA
        /// </summary>
        public string NDAFileName { get; set; }

        /// <summary>
        /// Gets or sets the ContactPersonName
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Gets or sets the ContactEmail
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the ContactPhoneNo
        /// </summary>
        public string ContactPhoneNo { get; set; }

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
        public List<CustomOrderModel> OrderModelList { get; set; }
        public string SameInfo { get; set; }
    }
}
