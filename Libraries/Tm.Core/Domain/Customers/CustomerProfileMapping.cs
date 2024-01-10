namespace Tm.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer-profile role mapping class
    /// </summary>
    public partial class CustomerProfileMapping : BaseEntity
    {
        public int CustomerId { get; set; }
        public string ProfileId { get; set; }
        public int BrandId { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
    }
}