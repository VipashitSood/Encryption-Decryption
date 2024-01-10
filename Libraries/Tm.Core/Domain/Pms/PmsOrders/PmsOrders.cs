using System;

namespace Tm.Core.Domain.Pms.Orders
{
	public class PmsOrders : BaseEntity

	{
		/// <summary>
		/// Gets or sets CustomerId
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets CustomerId
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// Gets or sets CustomerId
		/// </summary>
		public string DeliveryHeadId { get; set; }

		/// <summary>
		/// Gets or sets the OrderNumber
		/// </summary>
		public int OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the OrderName
		/// </summary>
		public string OrderName { get; set; }

		/// <summary>
		/// Gets or sets the SOWDocumentId
		/// </summary>
		public int SOWDocumentId { get; set; }

		/// <summary>
		/// Gets or sets the SOWSigningDate
		/// </summary>
		public DateTime SOWSigningDate { get; set; }

		/// <summary>
		/// Gets or sets the OrderCost
		/// </summary>
		public string OrderCost { get; set; }

		/// <summary>
		/// Gets or sets the EstimatedEfforts
		/// </summary>
		public decimal EstimatedEfforts { get; set; }

		/// <summary>
		/// Gets or sets the EstimatedTotalHours
		/// </summary>
		public decimal EstimatedTotalHours { get; set; }

		/// <summary>
		/// Gets or sets the EstimatedHourlyCost
		/// </summary>
		public decimal EstimatedHourlyCost { get; set; }

		/// <summary>
		/// Gets or sets the InHouse
		/// </summary>
		public bool InHouse { get; set; }

		/// <summary>
		/// Gets or sets the IsPoRequired
		/// </summary>
		public bool IsPoRequired { get; set; }

		/// <summary>
		/// Gets or sets the PONumber
		/// </summary>
		public string PONumber { get; set; }

		/// <summary>
		/// Gets or sets the ProjectDomainId
		/// </summary>
		public int ProjectDomainId { get; set; }

		/// <summary>
		/// Gets or sets the Notes
		/// </summary>
		public string Notes { get; set; }


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

		public int? CurrencyId { get; set; }

		public int TimeUnitId { get; set; }
	}
}
