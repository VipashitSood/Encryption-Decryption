using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using Tm.Core.Domain.Pms.PmsOrders;

namespace API.Models.GeneralDetail
{
    public class OrderDetailResponseModel
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public int OrderNumber { get; set; }
        public int SowDocType { get; set; }
        public string SowDocTypeName { get; set; }
        public decimal EstimatedEfforts  { get; set; }
        public decimal EstimatedHours  { get; set; }
        public decimal OrderCost { get; set; }
        public decimal HourlyCost { get; set; }

        public string ClientName { get; set; }
        public string Company { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNumber { get; set; }

        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string CurrencyIcon { get; set; }
        public string Address { get; set; }
        public Tm.Core.Domain.Pms.PmsAttachment.Attachments OrderAttachments { get; set; }
    }
}
