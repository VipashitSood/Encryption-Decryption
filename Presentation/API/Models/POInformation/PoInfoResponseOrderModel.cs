namespace API.Models.POInformation
{
	public class PoInfoResponseOrderModel
    {
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the OrderId
        /// </summary>
        public decimal OrderCost { get; set; }

        /// <summary>
        /// Gets or sets the OrderNumber
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the ConsumedAmount
        /// </summary>
        public decimal ConsumedAmount { get; set; }

        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
