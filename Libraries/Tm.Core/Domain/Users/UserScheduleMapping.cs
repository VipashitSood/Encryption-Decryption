using System;

namespace Tm.Core.Domain.Users
{
    /// <summary>
    /// Represents a user-schedule mapping class
    /// </summary>
    public partial class UserScheduleMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity updation
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }
    }
}