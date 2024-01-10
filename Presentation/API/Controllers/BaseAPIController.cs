using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Net;
using Tm.Core.Domain.Customers;
using Tm.Core.Infrastructure;
using Tm.Framework.Controllers;
using Tm.Services.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public partial class BaseAPIController : BaseController
    {
        /// <summary>
        /// Return response for invalid model
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public BaseResponseModel InvalidateModelResponse(ModelStateDictionary modelState, string message)
        {
            return new BaseResponseModel
            {
                StatusCode = (int)HttpStatusCode.NoContent,
                Message = message,
                Errors = modelState.Select(e => e.Value.Errors.Select(y => y.ErrorMessage).FirstOrDefault()).ToList(),
                Data = null
            };
        }

        /// <summary>
        /// Return response for error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stausCode"></param>
        /// <returns></returns>
        [NonAction]
        public BaseResponseModel ErrorResponse(string message = "", HttpStatusCode stausCode = HttpStatusCode.BadRequest)
        {
            var response = new BaseResponseModel
            {
                StatusCode = (int)stausCode,
                Message = message,
            };

            response.Errors.Add(message);
            return response;
        }

        /// <summary>
        /// Return response for error
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="stausCode"></param>
        /// <returns></returns>
        [NonAction]
        public BaseResponseModel ErrorResponse(Exception exception, HttpStatusCode stausCode = HttpStatusCode.BadRequest)
        {
            var _logger = EngineContext.Current.Resolve<ILogger>();
            var customer = (Customer)HttpContext.Items["Customer"];
            _logger.Error(exception.Message, exception, customer);

            var response = new BaseResponseModel
            {
                StatusCode = (int)stausCode,
                Message = exception.Message,
            };

            response.Errors.Add(exception.Message);
            return response;
        }

        /// <summary>
        /// Return response for success
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public BaseResponseModel SuccessResponse(string message="",object data=null)
        {
            return new BaseResponseModel
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Return list response for success
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        [NonAction]
        public BaseListResponseModel SuccessListResponse(string message = "", object data = null,int totalRecords=0)
        {
            return new BaseListResponseModel
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = message,
                Data = data,
                TotalRecords= totalRecords
            };
        }
    }
}
