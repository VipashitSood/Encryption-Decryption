﻿using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tm.Services.Authentication;

namespace Tm.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents filter attribute that sign out from the external authentication scheme
    /// </summary>
    public sealed class SignOutFromExternalAuthenticationAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        public SignOutFromExternalAuthenticationAttribute() : base(typeof(SignOutFromExternalAuthenticationFilter))
        {
        }

        #endregion

        #region Nested filter

        /// <summary>
        /// Represents a filter that sign out from the external authentication scheme
        /// </summary>
        private class SignOutFromExternalAuthenticationFilter : IAuthorizationFilter
        {
            #region Methods

            /// <summary>
            /// Called early in the filter pipeline to confirm request is authorized
            /// </summary>
            /// <param name="filterContext">Authorization filter context</param>
            public async void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                    throw new ArgumentNullException(nameof(filterContext));

                //sign out from the external authentication scheme
                var authenticateResult = await filterContext.HttpContext.AuthenticateAsync(TmAuthenticationDefaults.ExternalAuthenticationScheme);
                if (authenticateResult.Succeeded)
                    await filterContext.HttpContext.SignOutAsync(TmAuthenticationDefaults.ExternalAuthenticationScheme);
            }

            #endregion
        }

        #endregion
    }
}