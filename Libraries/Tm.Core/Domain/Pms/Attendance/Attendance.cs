using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.Attendance
{
    public class Attendance : BaseEntity
    {
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gte or set resourced
        /// </summary>
        public string ResourceId { get; set; }
        /// <summary>
        /// Get or Set ProjectionId
        /// </summary>
        public int ProjectionId { get; set; }
        /// <summary>
        /// Get or set resourceTypeId
        /// </summary>
        public int ResourceTypeId { get; set; }
        /// <summary>
        /// Get or set cost type
        /// </summary>
        public string CostType { get; set; }
        /// <summary>
        /// Get 0r set perhour coust
        /// </summary>
        public decimal PerHourCost { get; set; }
        //Get or Set hours
        public decimal Hours { get; set; }
        /// <summary>
        /// Get or Set Attendance Date
        /// </summary>
        public DateTime AttendanceDate { get; set; }
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
