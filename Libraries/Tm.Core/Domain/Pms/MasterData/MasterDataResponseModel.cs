using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.MasterData
{
    public class MasterDataResponseModel:BaseEntity
    {
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Head of department
        /// </summary>

        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedDate { get; set; }
        public int TotalCount { get; set; }
    }
}
