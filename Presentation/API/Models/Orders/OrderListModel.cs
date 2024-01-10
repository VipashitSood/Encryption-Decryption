using System;

namespace API.Factories.Orders
{
	public class OrderListModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public string OrderName { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string SowTypeName { get; set; }
        public DateTime SOWSigningDate { get; set; }
        public bool PoRequired { get; set; }
        public decimal ProjectCost { get; set; }
        public string CurrencyUrl { get; set; }
    }
}
