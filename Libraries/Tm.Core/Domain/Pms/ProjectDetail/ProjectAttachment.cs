using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public class ProjectAttachment : BaseEntity
    {
            /// <summary>
            /// Get or Sets ProjectId
            /// </summary>
            public int ProjectId { get; set; }

            /// <summary>
            /// Get or Sets AttachmentId
            /// </summary>
            public int AttachmentId { get; set; }

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
