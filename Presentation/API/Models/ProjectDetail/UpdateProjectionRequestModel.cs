using System;

namespace API.Models.ProjectDetail
{
    public class UpdateProjectionRequestModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gte or set resourced
        /// </summary>
        public string ResourceId { get; set; }
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
        /// Gets or sets the CreatedBy/Created
        /// </summary>
        public int RequestedById { get; set; }
        
        public int LanguageId { get; set; }
    }
}
