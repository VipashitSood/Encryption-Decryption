namespace API.Models.BillingInformation
{
    public class AddBillingInfoPOModel
    {

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets OrderId
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets PONumber
        /// </summary>
        public int BillingInfoId { get; set; }

        /// <summary>
        /// Gets or sets the POValue
        /// </summary>
        public string PONUmber { get; set; }

        /// <summary>
        /// Gets or sets the POValue
        /// </summary>
        public decimal POAmount { get; set; }

        /// <summary>
        /// Gets or sets the Base64
        /// </summary>
        public string Base64 { get; set; }
        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the FilePath
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public int CustomerId { get; set; }
    }
}
