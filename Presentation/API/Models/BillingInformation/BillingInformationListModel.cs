using System;
using System.Collections.Generic;

namespace API.Models.BillingInformation
{
	public class BillingInformationListModel
	{

		/// <summary>
		/// Gets or sets OrderId
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets OrderId
		/// </summary>
		public string OrderName { get; set; }

		/// <summary>
		/// Gets or sets OrderNumber
		/// </summary>
		public int OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the OrderType
		/// </summary>
		public int OrderType { get; set; }

		/// <summary>
		/// Gets or sets the MilestoneName
		/// </summary>
		public string MilestoneName { get; set; }

		/// <summary>
		/// Gets or sets the Deliverables
		/// </summary>
		public string Deliverables { get; set; }

		/// <summary>
		/// Gets or sets the ProjectedCost
		/// </summary>
		public decimal ProjectCost { get; set; }
		/// <summary>
		/// Gets or sets the ProjectedHours
		/// </summary>
		public decimal ProjectedHours { get; set; }

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
		/// Gets or sets the RaiseDate
		/// </summary>
		public DateTime? RaiseDate { get; set; }

		/// <summary>
		/// PONumberList
		/// </summary>
		public string PONumberList { get; set; }

		/// <summary>
		/// Gets or sets the CurrencyId
		/// </summary>
		public int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectedCost
        /// </summary>
        public decimal ActualBilling { get; set; }

		public int CustomerId { get; set; }

    }
}
