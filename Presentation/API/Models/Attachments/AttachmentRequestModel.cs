using API.Models.BaseModels;
using System;

namespace API.Models.Attachments
{
    public class AttachmentRequestModel : BaseRequestModel
    {
        /// <summary>
        /// Gets or Sets FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///Gets or Sets FilePath 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///Gets or Sets FilePath 
        /// </summary>
        public string AttachType { get; set; }

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
