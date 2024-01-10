using API.Factories.POInformation;
using API.Factories.ProjectDetail;
using API.Models.BaseModels;
using API.Models.POInformation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Services.Localization;
using Tm.Services.Pms.PmsPOInformation;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class POInformationController : BaseAPIController
	{
		#region Fields
		private readonly ILocalizationService _localizationService;
		private IPOInfoFactory _poInfoFactory;
		private IPOInfoService _poInfoService;
		private readonly TmConfig _tmConfig;
		private readonly IWebHostEnvironment _env;

		#endregion

		#region Ctor
		public POInformationController(
			ILocalizationService localizationService,
			IPOInfoFactory poInfoFactory,
			IPOInfoService poInfoService,
			TmConfig tmConfig,
			IWebHostEnvironment env)
		{
			_localizationService = localizationService;
			_poInfoFactory = poInfoFactory;
			_poInfoService = poInfoService;
			_tmConfig = tmConfig;
			_env = env;

		}
		#endregion

		#region Method


		// Insert POInformation
		[Route("CreatePoInformation")]
		[HttpPost]
		public async Task<BaseResponseModel> CreatePoInformation([FromBody] PoInfoModel model)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
			try
			{
				var customerOrder = await _poInfoFactory.CreatePoInformation(model);
				if (!customerOrder.Item1)
					return ErrorResponse(customerOrder.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customerOrder.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		[Route("GetPOCustomer")]
		[HttpGet]
		public async Task<BaseResponseModel> GetPOCustomer()
		{

			try
			{
				var customerOrder = await _poInfoFactory.GetCustomer();
				//return error if not found
				if (customerOrder == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customerOrder);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		// Insert POInformation
		[Route("GetCustomerOrderByCustomerId")]
		[HttpGet]
		public async Task<BaseResponseModel> GetCustomerOrderByCustomerId(int Id)
		{
			
			try
			{
				var customerOrder = await _poInfoFactory.GetCustomerOrder(Id);
				//return error if not found
				if (customerOrder == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customerOrder);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		// Update POInformation
		[Route("UpdatePoInformation")]
		[HttpPost]
		public async Task<BaseResponseModel> UpdatePoInformation([FromBody] UpdatePoInfoModel model)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
			try
			{
				var customerOrder = await _poInfoFactory.UpdatePoInformation(model);
				if (!customerOrder.Item1)
					return ErrorResponse(customerOrder.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), customerOrder.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		//// Get POInformation
		[Route("GetPoInformationById")]
		[HttpGet]
		public async Task<BaseResponseModel> GetPoInformationById(int id)
		{
			try
			{
				//get Customers by id
				var orders = await _poInfoFactory.GetPoInformationById(id);

				//return error if not found
				if (orders == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orders);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}


		// Get All POInformation
		[Route("GetAllPoInformation")]
		[HttpGet]
		public async Task<BaseResponseModel> GetAllPoInformation(int poId, int clientId, string companyName, int pageIndex, int pageSize)
		{
			try
			{
				var poInfoList = await _poInfoFactory.GetAllPoInformation(poId, clientId, companyName, pageIndex, pageSize);

				if (poInfoList == null || !poInfoList.Any())
				{
					// If Roles ,Module and Permissions are null or empty
					return ErrorResponse("No data found.", HttpStatusCode.NotFound);
				}
				// Check for pagination
				if (pageIndex > 0 && pageSize >= 1)
				{
					// Apply pagination
					var totalItems = poInfoList.Count();
					var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
					var results = poInfoList.Skip((pageIndex - 1) * pageSize).Take(pageSize);

					var response = new
					{
						TotalItems = totalItems,
						TotalPages = totalPages,
						Page = pageIndex,
						PageSize = pageSize,
						Results = results
					};

					return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
				}

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), poInfoList);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		// Get Get PoInformation Filters
		[Route("GetPoInformationFilters")]
		[HttpGet]
		public async Task<BaseResponseModel> GetPoInformationFilters()
		{
			try
			{
				//get Customers by id
				var orders = await _poInfoFactory.PoInformationFilters();

				//return error if not found
				if (orders == null)
					return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orders);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

		#endregion Method

	}
}