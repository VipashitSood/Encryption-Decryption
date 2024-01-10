using System;

namespace Tm.Core.Domain.Schedules
{
    /// <summary>
    /// Represents a schedule
    /// </summary>
    public partial class Schedule : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the schedule start date and time
        /// </summary>
        public DateTime StartDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the schedule end date and time
        /// </summary>
        public DateTime EndDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the minimum shift hours required
        /// </summary>
        public decimal MinShiftHoursRequired { get; set; }

        /// <summary>
        /// Gets or sets the schedule created by
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the client identifier
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance updation
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }
    }
}