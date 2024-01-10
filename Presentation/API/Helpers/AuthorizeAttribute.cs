using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Tm.Core.Domain.Customers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var customer = (Customer)context.HttpContext.Items["Customer"];
        if (customer == null)
        {
            // not logged in
            context.Result = new JsonResult(new BaseResponseModel { Message = "Unauthorized", StatusCode = StatusCodes.Status401Unauthorized, Data=null });
        }
    }
}