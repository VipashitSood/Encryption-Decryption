using API.Models.BaseModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Tm.Services.Logging;
using Tm.Core.Domain.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandling(RequestDelegate next,ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                _logger.InsertLog(LogLevel.Error, "Something went wrong", ex.ToString());
                await HandleError(httpContext, ex);
            }
        }
        public async Task HandleError(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string response = JsonConvert.SerializeObject(new
            {
                Message = "Internal Server Error.",
                StatusCode = httpContext.Response.StatusCode,
                Error = ex
            }.ToString());
            var bytes = Encoding.UTF8.GetBytes(response);

            await httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandling>();
        }
    }
}
