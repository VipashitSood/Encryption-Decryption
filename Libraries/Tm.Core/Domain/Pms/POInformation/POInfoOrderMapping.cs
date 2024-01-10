using System;

namespace Tm.Core.Domain.Pms.POInformation
{
	public class POInfoOrderMapping : BaseEntity
	{
		/// <summary>
		/// Gets or sets CustomerId
		/// </summary>
		public int POInfoId { get; set; }
		/// <summary>
		/// Gets or sets the OrderId
		/// </summary>
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or Sets the CustomerId
		/// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the ConsumedAmount
        /// </summary>
        public decimal ConsumedAmount { get; set; }

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
