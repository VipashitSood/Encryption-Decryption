using System;
using API.Models.Attachments;

namespace API.Models.ProjectDetail
{
    public class ProjectAttachmentModel
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Get or Sets FilePath
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the AttachedBy
        /// </summary>
        public int AttachBy { get; set; }
        /// <summary>
        /// Gets or sets the AttachedBy
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public AttachmentModel Attachment { get; set; }
    }
}
