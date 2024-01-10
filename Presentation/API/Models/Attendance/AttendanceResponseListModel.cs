﻿using System;
using System.Collections.Generic;

namespace API.Models.Attendance
{
    public class AttendanceResponseListModel
    {

        public List<AttendanceResourceData> Resources { get; set; }
        public List<AttendanceDataModel> Data { get; set; }

    }
    public class AttendanceResourceData
    {
        public string Name { get; set; }
        public string ResourceId { get; set; }
    }

    public class AttendanceDataModel
    {
        public string ResourceId { get; set; }
        public string Name { get; set; }
        public List<AttendanceEvents> Events { get; set; }
    }
    public class AttendanceEvents
    {
        public int Id { get; set; }
        /// <summary>
        /// Get or Set ProjectID
        /// </summary>
        public int ProjectId { get; set; }
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
        /// Get or Set Projection Date
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

        public string ADUserId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
    }
}
