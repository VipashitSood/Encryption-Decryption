﻿using System;
using System.Collections.Generic;

namespace API.Models.ProjectDetail
{
    public class ProjectionResponseModel
    {

        public List<ResourceData> Resources { get; set; }
        public List<ProjectionDataModel> Data { get; set; }

    }
    public class ResourceData
    {
        public string Name { get; set; }
        public string ResourceId { get; set; }
    }

    public class ProjectionDataModel
    {
        public string ResourceId { get; set; }
        public string Name { get; set; }
        public List<ProjectionEvents> Events { get; set; }
    }
    public class ProjectionEvents
    {
        public int Id { get; set; }
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
       
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
        /// Get or Set Projection Date
        /// </summary>
        public DateTime ProjectionDate { get; set; }
        /// <summary>
        /// Get or set Projection Start  Date
        /// </summary>
        public DateTime ProjectionStartDate { get; set; }
        /// <summary>
        /// Get or Set projection End Date 
        /// </summary>
        public DateTime ProjectionEndDate { get; set; }
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

        public string ADUserId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
    }
}