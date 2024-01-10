using API.Models.BaseModels;

namespace API.Models.POInformation
{
	public class PoOrderModel 
    {

        /// <summary>
        /// Gets or sets the OrderId
        /// </summary>
        public int? OrderId { get; set; }

        /// <summary>
        /// Gets or sets the ConsumedAmount
        /// </summary>
        public decimal? ConsumedAmount { get; set; }
    }
}
