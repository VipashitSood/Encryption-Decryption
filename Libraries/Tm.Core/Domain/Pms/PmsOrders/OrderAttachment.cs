using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.PmsOrders
{
    public class OrderAttachment :BaseEntity
    {
        /// <summary>
        /// Gets or Sets OrderId
        /// </summary>
        public int OrderId { get; set; }


        /// <summary>
        /// Gets or Sets AttachmentId
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Gets or Sets AttachmentId
        /// </summary>
        public bool IsSOWDocument { get; set; }

        /// <summary>
        /// Gets or Sets AttachmentId
        /// </summary>
        public bool IsPOUpload { get; set; }

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
