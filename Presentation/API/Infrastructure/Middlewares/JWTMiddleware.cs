using API.Models.AppSetting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Tm.Services.Accounts;
using System.Net;
using DocumentFormat.OpenXml.EMMA;
using API.Models.BaseModels;
using Newtonsoft.Json;

namespace API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWT _config;
        private readonly AD _ADconfig;
        public JWTMiddleware(RequestDelegate next, IOptions<JWT> confirg, IOptions<AD> aDconfig)
        {
            _next = next;
            _config = confirg.Value;
            _ADconfig = aDconfig.Value;
        }

        public async Task Invoke(HttpContext httpContext, IAccountService service)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split("Bearer").Last().Trim();
            SecurityToken jwt;

            string[] byPassAuthMethod = _ADconfig.ByPassAuthMethod.Split(",");
            string requestFrom = httpContext.Request?.Path.Value?.Split("/").Last();
            if (byPassAuthMethod.Where(x => x == requestFrom).Count() <= 0)
                if (token != null)
                {
                    try
                    {
                        string stsDiscoveryEndpoint = _ADconfig.StsDiscoveryEndpoint;

                        ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

                        OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;

                        TokenValidationParameters validationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            IssuerSigningKeys = config.SigningKeys,
                            ValidateLifetime = false
                        };

                        JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

                        tokendHandler.ValidateToken(token, validationParameters, out jwt);

                    }
                    catch (Exception ex)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        httpContext.Response.ContentType = "application/json";
                        var response = new BaseResponseModel
                        {
                            StatusCode = (int)HttpStatusCode.Unauthorized,
                            Message = "Invalid Token",
                        };
                        response.Errors.Add("Required valid token!");
                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                        return;
                    }
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    string message = "Unauthorized Access";
                    var response = new BaseResponseModel
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = message,
                    };
                    response.Errors.Add("Token requied!");
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    return;
                }
            await _next(httpContext).ConfigureAwait(false);
        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JWTMiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTMiddleware>();
        }
    }
}
