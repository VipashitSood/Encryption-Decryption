﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Domain.Pms.MasterData
{
    public partial class Currency : BaseEntity
    {
        /// <summary>
        /// Gets or sets the CurrencyName
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the CurrencyIcon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Gets or sets the CreatedOn
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }
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
