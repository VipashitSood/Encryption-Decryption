using System.Collections.Generic;

namespace API.Models.POInformation
{
	public class PoInfoModel 
    {
        //public PoInfoModel()
        //{
        //    OrderModelList = new List<PoOrderModel>();
        //}
        /// <summary>
        /// Gets or sets PONumber
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the POAmount
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
        /// Gets or sets the CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        ///  Order List Model
        /// </summary>
        //public List<PoOrderModel>? OrderModelList { get; set; }

        /// <summary>
        ///  LanguageId
        /// </summary>
        public int LanguageId { get; set; }
        /// <summary>
        /// PublicKey
        /// </summary>
        public string PublicKey { get; set; }
    }
}
