using API.Models.Attachments;
using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    public class GeneralDetailModel : BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the Order Id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the ProjectDomainId
        /// </summary>
        public int ProjectDomainId { get; set; }
        
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
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the PlannedStartDate
        /// </summary>
        public DateTime PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets the PlannedEndDate
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualStartDate
        /// </summary>
        public DateTime ActualStartDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualEndDate
        /// </summary>
        public DateTime ActualEndDate { get; set; }
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
        /// Gets or sets the AttachFiles
        /// </summary>
        public List<IFormFile> AttachFiles { get; set; }
        



        /// <summary>
        /// Gets or sets the ProjectNameId
        /// </summary>
        public int ProjectNameId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectTypeId
        /// </summary>
        public int ProjectTypeId { get; set; }
        /// <summary>
        /// Gets or sets the EffortsDuration
        /// </summary>
        public int EffortsDuration { get; set; }
        ///// <summary>
        ///// Gets or sets the EstimatedEffortUnit
        ///// </summary>
        public int EstimatedEffortUnit { get; set; }
        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectCostValue
        /// </summary>
        public decimal ProjectCostValue { get; set; }
        /// <summary>
        /// Gets or sets the HourlyCostValue
        /// </summary>
        public decimal HourlyCostValue { get; set; }
        /// <summary>
        /// Gets or sets the ManagerAttachFiles
        /// </summary>
        public List<IFormFile> ManagerAttachFiles { get; set; }
        /// <summary>
        ///  Gets or sets the ManagerClosedAttachFiles
        /// </summary>
        public List<IFormFile> ManagerClosedAttachFiles { get; set; }

    }
    public class GeneralDetailResponseModel : BaseRequestModel
    {
        public GeneralDetailResponseModel()
        {
            Attachments = new List<ProjectAttachmentModel>();
        }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the ProjectNameId
        /// </summary>
        public int ProjectNameId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectDomainId
        /// </summary>
        public int ProjectDomainId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectTypeId
        /// </summary>
        public int ProjectTypeId { get; set; }
        /// <summary>
        /// Gets or sets the EffortsDuration
        /// </summary>
        public int EffortsDuration { get; set; }
        ///// <summary>
        ///// Gets or sets the EstimatedEffortUnit
        ///// </summary>
        public int EstimatedEffortUnit { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the CurrencyId
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectCostValue
        /// </summary>
        public decimal ProjectCostValue { get; set; }
        /// <summary>
        /// Gets or sets the HourlyCostValue
        /// </summary>
        public decimal HourlyCostValue { get; set; }
        /// <summary>
        /// Gets or sets the ProjectStatusId
        /// </summary>
        public int ProjectStatusId { get; set; }
        /// <summary>
        /// Gets or sets the InHouse
        /// </summary>
        public bool InHouse { get; set; }
        /// <summary>
        /// Gets or sets the BillingVisibleToManager
        /// </summary>
        public bool BillingVisibleToManager { get; set; }
        /// <summary>
        /// Gets or sets the PlannedStartDate
        /// </summary>
        public string PlannedStartDate { get; set; }
        /// <summary>
        /// Gets or sets the PlannedEndDate
        /// </summary>
        public string PlannedEndDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualStartDate
        /// </summary>
        public string ActualStartDate { get; set; }
        /// <summary>
        /// Gets or sets the ActualEndDate
        /// </summary>
        public string ActualEndDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonStartDate
        /// </summary>
        public string DelayReasonStartDate { get; set; }
        /// <summary>
        /// Gets or sets the DelayReasonEndDate
        /// </summary>
        public string DelayReasonEndDate { get; set; }

        /// <summary>
        /// Gets or Sets ManagerId
        /// </summary>
        public int ManagerId { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }
        
        public List<ProjectAttachmentModel> Attachments{ get; set; }
       
        
        /// <summary>
        ///  Gets or sets the EstTotalHours
        /// </summary>
        public int EstTotalHours { get; set; }
        /// <summary>
        /// Get or set the project is on Azure or not
        /// </summary>
        public bool IsAzure { get; set; }

    }
}
