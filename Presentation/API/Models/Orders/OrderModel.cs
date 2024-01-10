using API.Models.BaseModels;
using System;

namespace API.Models.Orders
{
	public class OrderModel : BaseRequestModel
    {

        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectDomainId
        /// </summary>
        public int ProjectDomainId { get; set; }
        /// <summary>
        /// Gets or sets the DeliveryId
        /// </summary>
        public string DeliveryHeadId { get; set; }
        /// <summary>
        ///  Gets or sets the CustomerName
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        ///  Gets or sets the OrderNumber
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the OrderName
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// Gets or sets the SOWDocumentId
        /// </summary>
        public int SOWDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the SOWDocument
        /// </summary>
        public string SOWDocumentBase64 { get; set; }

        /// <summary>
        ///Gets or Sets SOWDocumentFilePath 
        /// </summary>
        public string SOWDocumentFilePath { get; set; }

        /// <summary>
        /// Gets or sets the SOWDocument
        /// </summary>
        public string SOWDocumentFileName { get; set; }
        /// <summary>
        /// Gets or sets the SOWSigningDate
        /// </summary>
        public DateTime SOWSigningDate { get; set; }

        /// <summary>
        /// Gets or sets the PONumber
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the OrderCost
        /// </summary>
        public decimal OrderCost { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedEfforts
        /// </summary>
        public decimal EstimatedEfforts { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedTotalHours
        /// </summary>
        public decimal EstimatedTotalHours { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedHourlyCost
        /// </summary>
        public decimal EstimatedHourlyCost { get; set; }

        /// <summary>
        /// Gets or sets the InHouse
        /// </summary>
        public bool InHouse { get; set; }

        /// <summary>
        /// Gets or sets the IsPoRequired
        /// </summary>
        public bool IsPoRequired { get; set; }
        /// <summary>
        /// Gets or sets the Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }


        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        ///  Gets or sets the Time unit
        /// </summary>
        public int TimeUnitId { get; set; }

    }
}
