namespace API.Models.Reports
{
    public class MonthlyBillingInformationReport
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string CompanyName { get; set; }
        public int Year { get; set; }
        public MonthStatus January { get; set; }
        public MonthStatus February { get; set; }
        public MonthStatus March { get; set; }
        public MonthStatus April { get; set; }
        public MonthStatus May { get; set; }
        public MonthStatus June { get; set; }
        public MonthStatus July { get; set; }
        public MonthStatus August { get; set; }
        public MonthStatus September { get; set; }
        public MonthStatus October { get; set; }
        public MonthStatus November { get; set; }
        public MonthStatus December { get; set; }
    }

    public class MonthStatus
    {
        public string Status { get; set; }
        public decimal ProjectCost { get; set; }
        public string CurrencyPath { get; set; }
    }

    public class BillingGroupKey
    {
        public int OrderId { get; set; }
        public int Year { get; set; }
    }
}
