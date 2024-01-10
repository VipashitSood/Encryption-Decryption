using API.Factories.Orders;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.Orders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Services.Localization;
using Tm.Services.Pms.Orders;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : BaseAPIController
	{
		#region Fields
		private readonly ILocalizationService _localizationService;
		private IPmsOrdersFactory _ordersFactory;
		private readonly TmConfig _tmConfig;
		private readonly IWebHostEnvironment _env;
		private IOrderService _orderService;
		private readonly IOrderService _ordersService;
		#endregion

		#region Ctor


		public OrderController(
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
		}
		#endregion

		#region Methods
		//Filter By Id
		[Route("OrderFilter")]
		[HttpGet]
		public async Task<BaseResponseModel> OrderFilter()
		{
			try
			{
				//get Customers by id
				var orders = await _ordersFactory.OrderFilter();

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



		//Get All Orders i.e List
		[Route("GetAllOrder")]
		[HttpGet]
		public async Task<BaseResponseModel> GetAllOrder(int pageIndex, int pageSize, int customerId,int orderId,int sowDocumentId,bool? isPoRequired,bool? inHouse)
		{
			try
			{
				// Get Customers by id
				var orders = await _ordersFactory.GetAllOrders(customerId,orderId,sowDocumentId,isPoRequired, inHouse);
				if (orders == null || !orders.Any())
				{
					// If Customers is null or empty
					return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
				}

				// Check for pagination
				if (pageIndex > 0 && pageSize >= 1)
				{
					// Apply pagination
					var totalItems = orders.Count();
					var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
					var results = orders.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orders);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}
		//Get Orders By Id
		[Route("GetOrdersById")]
		[HttpGet]
		public async Task<BaseResponseModel> GetOrdersById(int id)
		{
			try
			{
				//get Customers by id
				var orders = await _ordersFactory.GetOrdersById(id);

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

		// Delete Customers
		[Route("DeleteOrders")]
		[HttpDelete]
		public async Task<BaseResponseModel> DeleteOrders(int Id)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

			//get Customers by id
			var userRole = _orderService.GetOrdersById(Id);

			//return error if not found
			if (userRole == null)
				return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

			try
			{
				var data = await _ordersFactory.DeleteOrders(Id);
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
		//Insert  Orders
		[Route("CreateOrders")]
		[HttpPost]
		public async Task<BaseResponseModel> CreateOrders([FromBody] IList<OrderModel> orderModels)
		{
		
			try
			{
				var userRole = await _ordersFactory.CreateOrder(orderModels);
				if (!userRole.Item1)
					return ErrorResponse(userRole.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}
		//Update Orders
		[Route("UpdateOrders")]
		[HttpPut]
		public async Task<BaseResponseModel> UpdateOrders([FromBody] OrderModel model)
		{
			if (!ModelState.IsValid)
				return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
			try
			{
				var userRole = await _ordersFactory.UpdateOrder(model);
				if (!userRole.Item1)
					return ErrorResponse(userRole.Item2, HttpStatusCode.NoContent);

				return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole.Item3);
			}
			catch (Exception ex)
			{
				return ErrorResponse(ex, HttpStatusCode.InternalServerError);
			}
		}

	}
	#endregion
}