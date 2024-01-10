namespace API.Models.POInformation
{
	public class POInfoCustomOrderModel
    {
        /// <summary>
        /// Gets or sets the OrderName
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the OrderName
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the OrderCost
        /// </summary>
        public decimal OrderCost { get; set; }

        /// <summary>
        ///  Gets or sets the OrderNumber
        /// </summary>
        public int OrderNumber { get; set; }


        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
