using API.Models.BaseModels;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace API.Models.GeneralDetail
{
    public class GenDetailModel : BaseRequestModel
    {
        public GenDetailModel() {
            FileDataList = new List<IFormFile>();
        }
        /// <summary>
        /// Gets or sets the Order Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the InHouse
        /// </summary>
        public bool InHouse { get; set; }
        /// <summary>
        /// Gets or sets the BillingVisibleToManager
        /// </summary>
        public bool BillingVisibleToManager { get; set; }
        /// <summary>
        /// Gets or sets the ProjectStatusId
        /// </summary>
        public int ProjectStatusId { get; set; }
        /// <summary>
        /// Gets or Sets Client Id
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// Gets or Sets ManagerId
        /// </summary>
        public int ManagerId { get; set; }
        /// <summary>
        /// Gets or sets the PlannedStartDate
        /// </summary>
        public DateTime? PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets the PlannedEndDate
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualStartDate
        /// </summary>
        public DateTime? ActualStartDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualEndDate
        /// </summary>
        public DateTime? ActualEndDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonStartDate
        /// </summary>
        public string DelayReasonStartDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonEndDate
        /// </summary>
        public string DelayReasonEndDate { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// Gets or Set azure project Id
        /// </summary>
        public string Azure_ProjectId { get; set; }
        /// <summary>
        /// Gets or Set Project Name Id
        /// </summary>
        public string ProjectNameId { get; set; }
        /// <summary>
        /// Gets or Set Project Domain ID
        /// </summary>
        public int ProjectDomainId { get; set; }
        /// <summary>
        /// Get or Set communicationMode Id
        /// </summary>
        public int? CommunicationModeId { get; set; }
        public List<IFormFile> FileDataList { get; set; }
        /// <summary>
        /// Get or set the project is on Azure or not
        /// </summary>
        public bool IsAzure { get; set; }
    }
}
