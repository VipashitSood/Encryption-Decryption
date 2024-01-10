using API.Factories.ChangeRequestDetail;
using API.Factories.GeneralDetail;
using API.Factories.MasterData;
using API.Factories.ProjectDetail;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
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
    public class ChangeRequestController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly TmConfig _tmConfig;
        private IGeneralDetailModelFactory _generalDetailModelFactory;
        private IMasterDataFactory _masterDataModelFactory;
        private IMasterDataService _masterDataService;
        private IProjectsFactory _projectFactory;
        private IChangeRequestFactory _changeRequestFactory;
        #endregion

        #region Ctor
        public ChangeRequestController(ILocalizationService localizationService,
            TmConfig tmConfig,
            IGeneralDetailModelFactory generalDetailModelFactory,
            IMasterDataFactory masterDataModelFactory,
            IMasterDataService masterDataService,
            IProjectsFactory projectFactory,
            IChangeRequestFactory changeRequestFactory)
        {
            _localizationService = localizationService;
            _tmConfig = tmConfig;
            _generalDetailModelFactory = generalDetailModelFactory;
            _masterDataModelFactory = masterDataModelFactory;
            _masterDataService = masterDataService;
            _projectFactory = projectFactory;
            _changeRequestFactory = changeRequestFactory;
        }
        #endregion


        #region Methods 


        ////Get All DropDowns i.e List
        //[Route("GetAllDropDownLists")]
        //[HttpGet]
        //public async Task<BaseResponseModel> DropDownLists()
        //{
        //    try
        //    {
        //        // Get order Names
        //        var dropDownLists = await _generalDetailModelFactory.GetAllDropDownLists();
        //        if (dropDownLists == null)
        //        {
        //            // If order Name is null or empty
        //            return ErrorResponse("No Lists found.", HttpStatusCode.NotFound);
        //        }
        //        return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), dropDownLists);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        //    }
        //}

        /// <summary>
        /// Get Change Request Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetChangeRequestById")]
        [HttpGet]
        public async Task<BaseResponseModel> GetChangeRequestById(int id)
        {
            try
            {
                // Get change request detail by Id
                var changeRequest = await _changeRequestFactory.GetAllChangeRequestByIdAsync(id);
                if (changeRequest == null)
                {
                    // If changerequest is null or empty
                    return ErrorResponse("No order found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Project change requests by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Route("GetProjectAllChangeRequestsById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectChangeRequestsById(int projectId)
        {
            try
            {
                // Get All project change requests
                var projectChangeRequestsDetail = await _changeRequestFactory.GetAllChangeRequestByProjectAsync(projectId);
                if (projectChangeRequestsDetail == null)
                {
                    // If project change request is null or empty
                    return ErrorResponse("No order names found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectChangeRequestsDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        ///// <summary>
        ///// Get Project Manager detail by Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[Route("GetProjectManagerDetailByManagerId")]
        //[HttpGet]
        //public async Task<BaseResponseModel> ProjectManagerDetailById(int id)
        //{
        //    try
        //    {
        //        // Get project Manager detail
        //        var projectManagerDetail = await _generalDetailModelFactory.GetProjectManagerDetailById(id);
        //        if (projectManagerDetail == null)
        //        {
        //            // If project Manager is null or empty
        //            return ErrorResponse("No order names found.", HttpStatusCode.NotFound);
        //        }
        //        return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectManagerDetail);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        //    }
        //}

        //Insert & Update General Details
        [Route("SaveChangeRequest")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateChangeRequest([FromForm] ChangeRequestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CRName))
                {
                    return ErrorResponse("Project name cannot be empty.", HttpStatusCode.BadRequest);
                }

                var projectNameExists = _masterDataService.ProjectNameExists(model.CRName,model.Id);
                if (projectNameExists)
                {
                    return ErrorResponse("Project name already exists.", HttpStatusCode.BadRequest);
                }

                var generalDetailResult = await _changeRequestFactory.InsertUpdateChangeRequest(model);

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

        #endregion
    }
}
