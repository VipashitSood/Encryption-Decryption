using API.Models.BaseModels;
using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    public class BillingInfoModel: BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the MilestoneName
        /// </summary>
        public string MilestoneName { get; set; }
        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Gets or sets the Hours
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// Gets or sets the EndDate
        /// </summary>
        public string EndDate { get; set; }
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
