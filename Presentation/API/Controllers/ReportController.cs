using API.Controllers;
using API.Factories.Orders;
using API.Models.BaseModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using Tm.Core.Configuration;
using Tm.Services.Localization;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.Reports;
using API.Factories.Reports;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private IPmsOrdersFactory _ordersFactory;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private IOrderService _orderService;
        private readonly IOrderService _ordersService;
        private readonly IReportFactory _reportFactory;
        #endregion

        #region Ctor


        public ReportController(
            IReportFactory reportFactory,
        ILocalizationService localizationService,
        IPmsOrdersFactory orderFactory,
          TmConfig tmConfig,
        IWebHostEnvironment env,
        IOrderService orderService)
        {
            _localizationService = localizationService;
            _tmConfig = tmConfig;
            _ordersFactory = orderFactory;
            _env = env;
            _orderService = orderService;
            _reportFactory= reportFactory;
        }
        #endregion

        //Filter By Id
        [Route("ReportFilter")]
        [HttpGet]
        public async Task<BaseResponseModel> ReportFilter()
        {
            try
            {
                //get Customers by id
                var report = await _reportFactory.ReportFilter();

                //return error if not found
                if (report == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), report);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        [Route("ReportBllingInfo")]
        [HttpGet]
        public async Task<BaseResponseModel> ReportBllingInfo(int orderId, int customerId,int month,int year)
        {
            try
            {
                //get Customers by id
                var report = await _reportFactory.GetAllBillingInformationReportsMonthly(orderId, customerId, year, month);

                //return error if not found
                if (report == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), report);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
    }
}
