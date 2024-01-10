using System;

namespace Tm.Core.Domain.Pms.POInformation
{
	public class POInfo : BaseEntity
	{
		/// <summary>
		/// Gets or sets CustomerId
		/// </summary>
		public int CustomerId { get; set; }
		/// <summary>
		/// Gets or sets the PONumber
		/// </summary>
		public string PONumber { get; set; }
		/// <summary>
		/// Gets or sets the POAmount
		/// </summary>
		public decimal POAmount { get; set; }

		/// <summary>
		/// Gets or sets the POAmount
		/// </summary>
		public decimal PORemainingAmount { get; set; }
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
