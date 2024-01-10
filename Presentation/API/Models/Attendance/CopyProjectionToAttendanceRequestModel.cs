using System;

namespace API.Models.Attendance
{
    public class CopyProjectionToAttendanceRequestModel
    {
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gte or set resourced
        /// </summary>
        public string[] ResourceIds { get; set; }
        /// <summary>
        /// Get or set Projection Start  Date
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Get or Set projection End Date 
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy/Created
        /// </summary>
        public int RequestedById { get; set; }
    }
}
