﻿using Tm.Core.Domain.Customers;
//using Tm.Core.Domain.Directory;
using Tm.Core.Domain.Localization;
//using Tm.Core.Domain.Tax;
//using Tm.Core.Domain.Vendors;

namespace Tm.Core
{
    /// <summary>
    /// Represents work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Gets the original customer (in case the current one is impersonated)
        /// </summary>
        Customer OriginalCustomerIfImpersonated { get; }

        ///// <summary>
        ///// Gets the current vendor (logged-in manager)
        ///// </summary>
        //Vendor CurrentVendor { get; }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }

        ///// <summary>
        ///// Gets or sets current user working currency
        ///// </summary>
        //Currency WorkingCurrency { get; set; }

        ///// <summary>
        ///// Gets or sets current tax display type
        ///// </summary>
        //TaxDisplayType TaxDisplayType { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
