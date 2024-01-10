using API.Factories.Customer;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.Customer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Services.Localization;
using Tm.Services.Pms.PmsCustomers;

namespace API.Controllers
{

	[Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private IPmsCustomersFactory _customerFactory;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private readonly ICustomersService _customerService;
        #endregion

        #region Ctor


        public CustomerController(
        ILocalizationService localizationService,
        IPmsCustomersFactory customerFactory,
        TmConfig tmConfig,
        IWebHostEnvironment env,
        ICustomersService customerService)
        {
            _localizationService = localizationService;
            _tmConfig = tmConfig;
            _customerFactory = customerFactory;
            _env = env;
            _customerService = customerService;
        }
        #endregion


        //Get Customer By Id
        [Route("GetProjectDetailName")]
        [HttpGet]
        public async Task<BaseResponseModel> GetProjectDetailName()
        {
            try
            {
                //get Customers by id
                var customer = await _customerFactory.GetProjectDetailNames();

                //return error if not found
                if (customer == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customer);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }


        [Route("CustomerFilter")]
        [HttpGet]
        public async Task<BaseResponseModel> CustomerFilter()
        {
            try
            {
                //get Customers by id
                var filters = await _customerFactory.CustomerFilter();

                //return error if not found
                if (filters == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), filters);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Customers i.e List
        [Route("GetAllCustomer")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllCustomer(int pageIndex, int pageSize, string companyName="",string customerName="")
        {
            try
            {
                // Get Customers by id
                var customers = await _customerFactory.GetAllCustomer(pageIndex, pageSize, companyName, customerName);
                if (customers == null || !customers.Any())
                {
                    // If Customers is null or empty
                    return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
                }
               var singleRecord= customers.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null) {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages =Convert.ToInt32( singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = customers
                };

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        //Get Customer By Id
        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<BaseResponseModel> GetCustomerById(int id)
        {
            try
            {
                //get Customers by id
                var customer = await _customerFactory.GetCustomerById(id);

                //return error if not found
                if (customer == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customer);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        // Delete Customers
        [Route("DeleteCustomer")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteCustomer(int Id)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get Customers by id
            var customer = await _customerService.GetCustomerById(Id);

            //return error if not found
            if (customer == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            try
            {
                var data = await _customerFactory.DeleteCustomer(Id);
                if (!data.Item1)
                    return ErrorResponse(data.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), data.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        // Insert Customers and Order
        [Route("CreateCustomerOrder")]
        [HttpPost]
        public async Task<BaseResponseModel> CreateCustomerOrder([FromBody] PmsCustomerModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var customerOrder = await _customerFactory.CreateCustomer(model);
                if (!customerOrder.Item1)
                    return ErrorResponse(customerOrder.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customerOrder.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Update Customers
        [Route("UpdateCustomer")]
        [HttpPut]
        public async Task<BaseResponseModel> UpdateCustomer([FromBody] UpdatePmsCustomerModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var userRole = await _customerFactory.UpdateCustomer(model);
                if (!userRole.Item1)
                    return ErrorResponse(userRole.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Customers Attachment
        [Route("GetCustomerByAttachment")]
        [HttpGet]
        public async Task<BaseResponseModel> GetCustomerByAttachment(int id)
        {
            try
            {
                //get Customers by id
                var customer = await _customerFactory.GetCustomerAttachment(id);

                //return error if not found
                if (customer == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customer);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
    }
}
    

