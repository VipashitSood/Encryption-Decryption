using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectResponse
{
    public class MasterProjectResponseModel : BaseEntity
    {

        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public string UpdatedDate { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public string UpdatedByName { get; set; }

        /// <summary>
        /// Gets or sets the Head of department
        /// </summary>
        public string HODName { get; set; }
        /// <summary>
        /// Gets or sets the Head of department Id
        /// </summary>
        public string HODId { get; set; }
    }
}

