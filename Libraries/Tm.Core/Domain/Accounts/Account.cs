using System;

namespace Tm.Core.Domain.Accounts
{
    /// <summary>
    /// Represents a account
    /// </summary>
    public partial class Account : BaseEntity
    {
        /// <summary>
        /// Gets or sets the acountno
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// Gets or sets the acountholdername
        /// </summary>
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance updation
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
    }
}