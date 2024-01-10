using API.Factories.BillingInformations;
using API.Models.BaseModels;
using API.Models.BillingInformation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Tm.Services.Localization;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BillingInformationController : BaseAPIController
	{
		#region Fields
		private readonly ILocalizationService _localizationService;
		private readonly IBillingInfoFactory _billingInfoFactory;
		#endregion

		#region Ctor
		public BillingInformationController(
			ILocalizationService localizationService,
			IBillingInfoFactory billingInfoFactory)
		{
			_localizationService = localizationService;
			_billingInfoFactory = billingInfoFactory;

		}
		#endregion

		#region Method


		[Route("CreateBillingInformation")]
		[HttpPost]
		public async Task<BaseResponseModel> CreateBillingInformation([FromBody] BillingInformationModel model)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
			try
			{
				var billingInfo = await _billingInfoFactory.CreateBillingInformation(model);
				if (!billingInfo.Item1)
					return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		[Route("GetBillingInfoOrders")]
		[HttpGet]
		public async Task<BaseResponseModel> GetBillingInfoOrders()
		{

			try
			{
				var billingInfoOrders = await _billingInfoFactory.GetBillingInfoOrder();
				//return error if not found
				if (billingInfoOrders == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfoOrders);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		[Route("GetCustomerPOList")]
		[HttpGet]
		public async Task<BaseResponseModel> GetCustomerPOList(int customerId)
		{
			try
			{
				var orderPoList = await _billingInfoFactory.GetCustomerPOList(customerId);
				//return error if not found
				if (orderPoList == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orderPoList);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		[Route("GetAllBillingInfo")]
		[HttpGet]
		public async Task<BaseResponseModel> GetAllBillingInfo(int customerId)
		{
			try
			{
				var billingInfo = await _billingInfoFactory.GetAllBillingInfo(customerId);
				//return error if not found
				if (billingInfo == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		[Route("UpdateBillingInfoAction")]
        [HttpPost]
		public async Task<BaseResponseModel> UpdateBillingInfoAction(int billingInfoId,int managerAction,int dHAction,int aHAction, string raiseDate, decimal actualBilling)
		{
			
			try
			{
                DateTime? parsedDate = null;

                if (raiseDate != null)
                {
                    if (!DateTime.TryParseExact(raiseDate, "yy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                    {
                        // Handle invalid date format
                        return ErrorResponse("Invalid date format for raiseDate", HttpStatusCode.BadRequest);
                    }

                    parsedDate = parsed;
                }


                var billingInfo = await _billingInfoFactory.BillingInfoAction(billingInfoId, managerAction, dHAction, aHAction, parsedDate, actualBilling);
				if (!billingInfo.Item1)
					return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		[Route("GetBillingInfoById")]
		[HttpPost]
		public async Task<BaseResponseModel> GetBillingInfoById(int billingInfoId)
		{

			try
			{
				var billingInfo = await _billingInfoFactory.GetBillingInfoById(billingInfoId);
				//return error if not found
				if (billingInfo == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		[Route("UpdateBillingInformation")]
		[HttpPost]
		public async Task<BaseResponseModel> UpdateBillingInformation([FromBody] UpdateBillingInformationModel model)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
			try
			{
				var billingInfo = await _billingInfoFactory.UpdateBillingInformation(model);
				if (!billingInfo.Item1)
					return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		[Route("AddPOBillingInformation")]
		[HttpPost]
		public async Task<BaseResponseModel> AddPOBillingInformation([FromBody] IList<AddBillingInfoPOModel> model)
		{
			try
			{
				var billingInfo = await _billingInfoFactory.AddPOBillingInfo(model);
				if (!billingInfo.Item1)
					return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		#endregion Method

	}
}