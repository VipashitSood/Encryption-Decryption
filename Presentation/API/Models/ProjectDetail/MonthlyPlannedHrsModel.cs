using API.Models.BaseModels;
using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    public class MonthlyPlannedHrsModel 
    {
        public IList<MonthlyPlannedModel> MonthlyPlannedListModel { get; set; }
        public int TotalHours { get; set; }
    }

    public class MonthlyPlannedModel : BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        public string PlannedDate { get; set; }
        /// <summary>
        ///  Gets or sets the Hours
        /// </summary>
        public decimal Hours { get; set; }
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
    }
}
