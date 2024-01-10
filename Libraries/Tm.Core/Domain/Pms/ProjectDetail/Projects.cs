using System;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class Projects : BaseEntity
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

        ///// <summary>
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
        /// Gets or sets the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }



        /// <summary>
        /// Gets or sets the ProjectTypeId
        /// </summary>
        public int ProjectTypeId { get; set; }
        /// <summary>
        /// Gets or sets the ProjectDomainId
        /// </summary>
        public int ProjectDomainId { get; set; }
        /// <summary>
        /// Gets or sets the EffortsDuration
        /// </summary>
        public int EffortsDuration { get; set; }
        /// <summary>
        /// Gets or sets the EstimatedEffortUnit
        /// </summary>
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
        ///  Gets or sets the EstTotalHours
        /// </summary>
        public int EstTotalHours { get; set; }

        /// <summary>
        /// Gets or sets the ProjectNameId
        /// </summary>
        public int ProjectNameId { get; set; }

        /// <summary>
        /// Get or set the azure project Id
        /// </summary>
        public string Azure_ProjectId { get; set; }
        public int? CommunicationModeId { get; set; }

        /// <summary>
        /// Get or set the project is on Azure or not
        /// </summary>
        public bool IsAzure { get; set; }
    }
}
