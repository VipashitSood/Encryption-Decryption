using System;

namespace API.Models.ProjectDetail
{
    public class UpdateAttendanceRequestModel
    {
        /// <summary>
        /// Get or set Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Get or set resourceTypeId
        /// </summary>
        public int ResourceTypeId { get; set; }
        /// <summary>
        /// Get or set hours
        /// </summary>
        public decimal Hours { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy/Created
        /// </summary>
        public int RequestedById { get; set; }
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
    }
}
