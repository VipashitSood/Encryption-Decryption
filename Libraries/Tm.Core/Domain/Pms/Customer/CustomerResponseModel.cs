using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.Customer
{
    public class CustomerResponseModel : BaseEntity
    {
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets DisplayName
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        /// Gets or sets the Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNo
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the ContactPersonName
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Gets or sets the ContactEmail
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the ContactPhoneNo
        /// </summary>
        public string ContactPhoneNo { get; set; }


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

        public bool? SameInfo { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }


    }
}
