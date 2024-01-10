using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.ProjectDetail
{
    public partial class ClientDetail: BaseEntity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the ClientName
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Gets or sets the CompanyName
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the ClientEmailId
        /// </summary>
        public string ClientEmailId { get; set; }
        
        /// <summary>
        /// Gets or sets the PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the ContactPersonName
        /// </summary>
        public string ContactPersonName { get; set; }
        /// <summary>
        /// Gets or sets the ContactPersonEmailId
        /// </summary>
        public string ContactPersonEmailId { get; set; }
        
        /// <summary>
        /// Gets or sets the ContactPersonPhoneNumber
        /// </summary>
        public string ContactPersonPhoneNumber { get; set; }
         /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }
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


        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the  ContactCountryCode
        /// </summary>
        public int ContactCountryCode { get; set; }
        /// <summary>
        /// Gets or sets the CommunicationModeId
        /// </summary>
        public int CommunicationModeId { get; set; }

        /// <summary>
        /// Gets or sets the CountryCode
        /// </summary>
        public int CountryCode { get; set; }
    }
}
