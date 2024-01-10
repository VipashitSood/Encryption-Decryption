namespace API.Models.BillingInformation
{
	public class BillingInfoOrderModel
	{
		/// <summary>
		/// Gets or sets the CustomerId
		/// </summary>
		public int Value { get; set; }

		/// <summary>
		/// Gets or sets the Name
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Gets or sets the OrderNumber
		/// </summary>
		public int OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the CurrencyId
		/// </summary>
		public int CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the OrderType
		/// </summary>
		public string OrderType { get; set; }
	}
}
