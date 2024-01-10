using System;
using System.Security.Permissions;

namespace Tm.Core.Domain.Pms.BillingInformations
{
	public class BillingInformation : BaseEntity
	{

		/// <summary>
		/// Gets or sets ProjectId
		/// </summary>
		public int ProjectId { get; set; }
		/// <summary>
		/// Gets or sets MilestoneName
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
		/// Gets or sets the OrderId
		/// </summary>
		public int OrderId { get; set; }

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
		/// Gets or sets the CurrencyId
		/// </summary>
		public int CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the CreatedOn
		/// </summary>
		public DateTime CreatedOn { get; set; }
		/// <summary>
		/// Gets or sets the CreatedBy
		/// </summary>
		public int CreatedBy { get; set; }
		/// <summary>
		/// Gets or sets the ModifiedBy
		/// </summary>
		public int ModifiedBy { get; set; }
		/// <summary>
		/// Gets or sets the ModifiedOn
		/// </summary>
		public DateTime? ModifiedOn { get; set; }
		/// <summary>
		/// Gets or sets the IsDeleted
		/// </summary>
		public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the ProjectedCost
        /// </summary>
        public decimal ActualBilling { get; set; }

		/// <summary>
		/// Gets or Sets CustomerId
		/// </summary>
        public int CustomerId { get; set; }
    }
}
