using API.Models.BaseModels;
using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    public class techStackList : BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the BackendTechStackId
        /// </summary>
        public List<int?> BackendTechStackId { get; set; }
        /// <summary>
        /// Gets or sets the FrontendTechStackId
        /// </summary>
        public List<int?> FrontendTechStackId { get; set; }
        /// <summary>
        /// Gets or sets the Android
        /// </summary>
        public bool Android { get; set; }
        /// <summary>
        /// Gets or sets the IOS
        /// </summary>
        public bool IOS { get; set; }
        /// <summary>
        /// Gets or sets the Hybrid
        /// </summary>
        public bool Hybrid { get; set; }
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
        public bool IsDeleted { get; internal set; }
    }

}