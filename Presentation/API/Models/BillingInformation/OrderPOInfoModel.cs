namespace API.Models.BillingInformation
{
	public class OrderPOInfoModel
	{
		/// <summary>
		/// Gets or sets the PONumber
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Gets or sets the PoId
		/// </summary>
		public int Value { get; set; }

		/// <summary>
		/// Gets or sets the POAmount
		/// </summary>
		public decimal POAmount { get; set; }

		/// <summary>
		/// Gets or sets the POAmount
		/// </summary>
		public decimal POAmountRemaining { get; set; }

		/// <summary>
		/// Gets or sets the OrderId
		/// </summary>
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or sets the FileName
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the FilePath
		/// </summary>
		public string FilePath { get; set; }

		public string CurrencyPath { get; set; }

		/// <summary>
		/// Gets or Sets CustomerId
		/// </summary>
        public int CustomerId { get; set; }
    }
}
