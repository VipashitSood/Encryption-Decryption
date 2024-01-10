using System;

namespace Tm.Core.Domain.Pms.BillingInformations
{
	public class BillingInfoPOMapping : BaseEntity
	{
		/// <summary>
		/// Gets or sets BillingInfoId
		/// </summary>
		public int BillingInfoId { get; set; }
		/// <summary>
		/// Gets or sets the POId
		/// </summary>
		public int POId { get; set; }

		/// <summary>
		/// Gets or sets the POValue
		/// </summary>
		public decimal POValue { get; set; }

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
	}
}
