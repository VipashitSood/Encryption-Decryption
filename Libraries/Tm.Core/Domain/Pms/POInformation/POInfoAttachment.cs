using System;

namespace Tm.Core.Domain.Pms.POInformation
{
	public class POInfoAttachment : BaseEntity
    {
        /// <summary>
        /// Get or Sets CustomerId
        /// </summary>
        public int POInfoId { get; set; }

        /// <summary>
        /// Get or Sets AttachmentId
        /// </summary>
        public int AttachmentId { get; set; }

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
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
