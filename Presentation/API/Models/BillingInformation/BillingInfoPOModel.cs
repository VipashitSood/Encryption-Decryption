namespace API.Models.BillingInformation
{
	public class BillingInfoPOModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets PONumber
        /// </summary>
        public int BillingInfoId { get; set; }

        /// <summary>
        /// Gets or sets PONumber
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets POId
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the POValue
        /// </summary>
        public decimal POValue { get; set; }

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
        /// Gets or sets the POAmount
        /// </summary>
        public decimal POAmountRemaining { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }
    }
}
