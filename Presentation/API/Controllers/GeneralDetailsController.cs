using API.Factories.GeneralDetail;
using API.Factories.MasterData;
using API.Factories.ProjectDetail;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Services.Localization;
using Tm.Services.Pms.MasterData;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDetailsController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly TmConfig _tmConfig;
        private IGeneralDetailModelFactory _generalDetailModelFactory;
        private IMasterDataFactory _masterDataModelFactory;
        private IMasterDataService _masterDataService;
        private IProjectsFactory _projectFactory;
        #endregion

        #region Ctor
        public GeneralDetailsController(ILocalizationService localizationService,
            TmConfig tmConfig,
            IGeneralDetailModelFactory generalDetailModelFactory,
            IMasterDataFactory masterDataModelFactory,
            IMasterDataService masterDataService,
            IProjectsFactory projectFactory)
        {
            _localizationService = localizationService;
            _tmConfig = tmConfig;
            _generalDetailModelFactory = generalDetailModelFactory;
            _masterDataModelFactory = masterDataModelFactory;
            _masterDataService = masterDataService;
            _projectFactory = projectFactory;
        }
        #endregion


        #region Methods 


        //Get All DropDowns i.e List
        [Route("GetAllDropDownLists")]
        [HttpGet]
        public async Task<BaseResponseModel> DropDownLists()
        {
            try
            {
                // Get order Names
                var dropDownLists = await _generalDetailModelFactory.GetAllDropDownLists();
                if (dropDownLists == null)
                {
                    // If order Name is null or empty
                    return ErrorResponse("No Lists found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), dropDownLists);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Order Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOrderDetailById")]
        [HttpGet]
        public async Task<BaseResponseModel> OrderDetailById(int id)
        {
            try
            {
                // Get order Detail
                var orderDetail = await _generalDetailModelFactory.GetOrderDetailById(id);
                if (orderDetail == null)
                {
                    // If order detail is null or empty
                    return ErrorResponse("No order found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orderDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Project ManagerDetail By UserId (Guid)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetProjectManagerDetailById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectManagerDetailById(string userId)
        {
            try
            {
                // Get project Manager detail
                var projectManagerDetail = await _generalDetailModelFactory.GetProjectManagerDetailById(userId);
                if (projectManagerDetail == null)
                {
                    // If project Manager is null or empty
                    return ErrorResponse("No order names found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectManagerDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Get Project Manager detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetProjectManagerDetailByManagerId")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectManagerDetailById(int id)
        {
            try
            {
                // Get project Manager detail
                // Get order Detail
                var orderDetail = await _generalDetailModelFactory.GetProjectManagerDetailById(id);
                if (orderDetail == null)
                {
                    // If order detail is null or empty
                    return ErrorResponse("No order found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), orderDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        //Insert & Update General Details
        [Route("SaveGeneralDetail")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateGeneralDetail([FromForm] GenDetailModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    return ErrorResponse("Project name cannot be empty.", HttpStatusCode.BadRequest);
                }

                var projectNameExists = _masterDataService.ProjectNameExists(model.Name, model.Id);
                if (projectNameExists)
                {
                    return ErrorResponse("Project name already exists.", HttpStatusCode.BadRequest);
                }

                var generalDetailResult = await _projectFactory.InsertUpdateGenDetail(model);

                if (!generalDetailResult.Item1)
                {
                    // Handle error cases
                    return ErrorResponse(generalDetailResult.Item2, HttpStatusCode.BadRequest);
                }

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), generalDetailResult.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetProjectDetailById")]
        [HttpPost]
        public async Task<BaseResponseModel> GetProjectDetailByID(int projectId) {
            try
            {
                // Get project detail by ID
                var projectManagerDetail = await _projectFactory.GetProjectDetailByIdAsync(projectId);
                if (projectManagerDetail == null)
                {
                    // If project Manager is null or empty
                    return ErrorResponse("No order names found.", HttpStatusCode.NotFound);
                }
                //Add order  detail with project
                if (projectManagerDetail != null)
                {
                    projectManagerDetail.Order = await _generalDetailModelFactory.GetOrderDetailById(projectManagerDetail.OrderId);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectManagerDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
