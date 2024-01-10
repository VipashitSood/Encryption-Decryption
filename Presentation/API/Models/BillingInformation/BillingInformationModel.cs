using System;
using System.Collections.Generic;

namespace API.Models.BillingInformation
{
	public class BillingInformationModel
    {
		public BillingInformationModel()
		{
			BillingInfoOrderPOList = new List<BillingInfoPOModel>();
		}

		/// <summary>
		/// Gets or sets ProjectId
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets OrderId
		/// </summary>
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or sets the MilestoneName
		/// </summary>
		public string MilestoneName { get; set; }

		/// <summary>
		/// Gets or sets the Deliverables
		/// </summary>
		public string Deliverables { get; set; }
		/// <summary>
		/// Gets or sets the ProjectedHours
		/// </summary>
		public decimal ProjectedHours { get; set; }

		/// <summary>
		/// Gets or sets the ProjectedCost
		/// </summary>
		public decimal ProjectCost { get; set; }

		/// <summary>
		/// Gets or sets the TimePeriod
		/// </summary>
		public DateTime? TimePeriod { get; set; }

		/// <summary>
		/// Gets or sets the ToBeRaised
		/// </summary>
		public DateTime? ToBeRaised { get; set; }

		/// <summary>
		/// Gets or sets the ManagerAction
		/// </summary>
		public int ManagerAction { get; set; }

		/// <summary>
		/// Gets or sets the DHAction
		/// </summary>
		public int DHAction { get; set; }

		/// <summary>
		/// Gets or sets the AHAction
		/// </summary>
		public int AHAction { get; set; }

		/// <summary>
		/// Gets or sets the CurrencyId
		/// </summary>
		public int CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy
		/// </summary>
		public int CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the ModifiedBy
		/// </summary>
		public int ModifiedBy { get; set; }

		/// <summary>
		///  Billing Info Order PO List
		/// </summary>
		public List<BillingInfoPOModel> BillingInfoOrderPOList { get; set; }

		/// <summary>
		///  LanguageId
		/// </summary>
		public int LanguageId { get; set; }
		/// <summary>
		/// PublicKey
		/// </summary>
		public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the ProjectedCost
        /// </summary>
        public decimal ActualBilling { get; set; }

		/// <summary>
		/// Gets or Sets the CustomerId
		/// </summary>
        public int CustomerId { get;  set; }
    }
}
