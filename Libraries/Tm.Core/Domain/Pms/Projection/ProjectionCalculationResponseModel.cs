using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.Projection
{
    public class ProjectionCalculationResponseModel : BaseEntity
    {
        /// <summary>
        /// Get or Set Resource Type
        /// </summary>
        public string ResourceTypeName { get; set; }
        /// <summary>
        /// Get or set color code
        /// </summary>
        public string ColorCode { get; set; }
        /// <summary>
        /// Gets or sets the TotalResources
        /// </summary>
        public int TotalResources { get; set; }
        /// <summary>
        /// Gets or sets the TotalHours
        /// </summary>
        public decimal TotalHours { get; set; }
        /// <summary>
        /// Gets or sets the TotalCost
        /// </summary>
        public decimal TotalCost { get; set; }
    }
}
