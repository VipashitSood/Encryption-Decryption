using API.Models.BaseModels;
using System;

namespace API.Models.Attachments
{
    public class AttachmentModel : BaseRequestModel
    {
        /// <summary>
        /// Gets or sets the FileName
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the FilePath
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the AttachedBy
        /// </summary>
        public int AttachedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }
        public bool? IdDeleted { get; set; }
    }
}
