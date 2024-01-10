using API.Models.BaseModels;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;

namespace API.Models.UserRole
{
    public class UserRolesModel : BaseRequestModel
    {
        public UserRolesModel() 
        {
            ADUserList = new List<string>();
        
        }
        /// <summary>
        /// Gets or sets the Roles
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the AddUser
		/// </summary>
		public List<string> ADUserList { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>   
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary> 
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedOn
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }

    public class AdUserList
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string UserId {get; set;}
    }
}
