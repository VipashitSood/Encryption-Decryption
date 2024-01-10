using System;

namespace Tm.Core.Domain.Common
{
    /// <summary>
    /// Represents a address
    /// </summary>
    public partial class Address : BaseEntity
    {
        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state/province identifier
        /// </summary>
        public int? StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

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