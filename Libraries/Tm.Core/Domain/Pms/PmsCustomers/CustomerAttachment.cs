using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.PmsCustomers
{
    public class CustomerAttachment : BaseEntity
    {
        /// <summary>
        /// Get or Sets CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Get or Sets AttachmentId
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Get or Sets MSA
        /// </summary>
        public bool MSA { get; set; }

        /// <summary>
        /// Get or Sets NDA
        /// </summary>
        public bool NDA { get; set; }

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
