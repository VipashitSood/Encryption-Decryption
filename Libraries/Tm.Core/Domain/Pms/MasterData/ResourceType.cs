using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.MasterData
{
    public class ResourceType : BaseEntity
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
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int? ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        /// Gets or sets the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Get or set IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }
}
