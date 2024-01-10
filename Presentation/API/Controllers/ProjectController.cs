using API.Factories.MasterData;
using API.Factories.ProjectDetail;
using API.Helpers;
using API.Models.Attendance;
using API.Models.BaseModels;
using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
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
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Services.Customers;
using Tm.Services.Localization;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.ProjectDetail;
using Ubiety.Dns.Core;
using APDetails = API.Models.ProjectDetail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private IProjectsFactory _projectFactory;
        private readonly TmConfig _tmConfig;
        private IMasterDataFactory _masterDataFactory;
        private IMasterDataService _masterDataService;
        private readonly IWebHostEnvironment _env;
        private readonly IProjectsService _projectService;
        private readonly ICustomerService _customerService;
        #endregion

        #region Ctor
        public ProjectController(
            ILocalizationService localizationService,
            IProjectsFactory projectsFactory,
             TmConfig tmConfig,
            IMasterDataFactory masterDataFactory,
            IMasterDataService masterDataService,
            IWebHostEnvironment env,
            IProjectsService projectService,
            ICustomerService customerService)
        {
            _localizationService = localizationService;
            _projectFactory = projectsFactory;
            _tmConfig = tmConfig;
            _masterDataFactory = masterDataFactory;
            _masterDataService = masterDataService;
            _env = env;
            _projectService = projectService;
            _customerService = customerService;
        }
        #endregion

        #region Utilities
        private async Task<Tuple<bool, string, int>> InsertOrUpdateGeneralDetailWithAttachments(GeneralDetailModel model)
        {
            var generalDetail = await _projectFactory.InsertUpdateGeneralDetail(model);

            if (!generalDetail.Item1)
                return Tuple.Create(false, generalDetail.Item2, 0);

            await AttachFiles(model.AttachFiles, _tmConfig.AttachmentFolderPath, AttachmentEnum.AttachmentOnly, generalDetail.Item3, model);
            await AttachFiles(model.ManagerAttachFiles, _tmConfig.ManagerAttachmentFolderPath, AttachmentEnum.AttachmentManager, generalDetail.Item3, model);
            await AttachFiles(model.ManagerClosedAttachFiles, _tmConfig.ManagerClosedAttachmentFolderPath, AttachmentEnum.AttachmentClosed, generalDetail.Item3, model);

            return Tuple.Create(true, string.Empty, generalDetail.Item3);
        }

        private async Task AttachFiles(List<IFormFile> files, string path, AttachmentEnum attachType, int projectId, GeneralDetailModel model)
        {
            if (files != null && files.Count > 0)
            {
                string filePath = string.Concat(_env.ContentRootPath, path);

                foreach (var file in files)
                {
                    int lastIndex = file.FileName.LastIndexOf('.');
                    if (lastIndex + 1 < file.FileName.Length)
                    {
                        string firstPart = file.FileName.Substring(0, lastIndex);
                        string secondPart = file.FileName.Substring(lastIndex + 1);

                        string newFileName = $"{firstPart}-{DateTime.UtcNow:yyyyMMdd_hhmmss}." + secondPart;

                        var attachOnly = Path.Combine(path.Replace("wwwroot\\", string.Empty), newFileName);
                        await _projectService.InsertAttachment(new Attachments
                        {
                            FilePath = attachOnly,
                            CreatedBy = model.CreatedBy,
                            CreatedOn = DateTime.UtcNow
                        });
                        using (Stream stream = new FileStream(Path.Combine(filePath, newFileName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                    }
                }
            }
        }

        #endregion Utilities

        #region Method

        #region Project Listing 

        //Get All Projects
        [Route("GetAllProjects")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllProjects(int projectNameId, int projectTypeId, int managerId, int projectStatusId, int pageIndex, int pageSize)
        {
            try
            {
                //get all project
                var projects = await _projectFactory.GetAllProjectAsync(projectNameId, projectTypeId, managerId, projectStatusId, pageIndex, pageSize);

                if (projects?.Item2 == null)
                {
                    // If projects are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = projects?.Item1;
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = projects?.Item2;//.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

                return SuccessListResponse(_localizationService.GetResource("Tm.API.Success"), projects, totalRecords: projects.Item1);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Project Listing 

        #region  General Details

        //Get General Detail By Id
        [Route("GetGeneralDetailById")]
        [HttpGet]
        public async Task<BaseResponseModel> GeneralDetailById(int id)
        {
            try
            {
                //get general detail by id
                var generalDetail = await _projectFactory.GetGeneralDetailById(id);

                //return error if not found
                if (generalDetail == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), generalDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //check Project Name Exist
        [Route("CheckProjectNameExists")]
        [HttpGet]
        public IActionResult CheckProjectNameExists(string projectName)
        {
            try
            {
                if (string.IsNullOrEmpty(projectName))
                    return BadRequest("Project name cannot be empty.");

                var projectNameExists = _masterDataService.ProjectNameExists(projectName, 0);
                if (projectNameExists)
                {
                    //project name already exists
                    return Ok(projectNameExists);
                }

                //project name isn't exists
                return Ok(projectNameExists);
            }
            catch (Exception)
            {
                return BadRequest(error: "An error occurred while processing the request.");
            }
        }

        //Insert & Update General Details
        [Route("SaveGeneralDetail")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateGeneralDetail([FromForm] GeneralDetailModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            Tuple<bool, string, int> generalDetailResult = null;

            try
            {
                if (model.ProjectNameId == (int)ProjectEnum.Other && model.Id <= 0)
                {
                    if (string.IsNullOrEmpty(model.Name))
                        return ErrorResponse("Project name cannot be empty when 'Other' option is selected.", HttpStatusCode.BadRequest);

                    var projectNameExist = _masterDataService.ProjectNameExists(model.Name, model.Id);
                    if (projectNameExist)
                        return ErrorResponse("Project name already exists.", HttpStatusCode.BadRequest);

                    await _masterDataService.InsertProjectName(new ProjectName
                    {
                        Name = model.Name,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = (int)(_customerService.GetCustomerById(model.CreatedBy)?.Id),
                    });

                    generalDetailResult = await InsertOrUpdateGeneralDetailWithAttachments(model);
                    if (!generalDetailResult.Item1)
                        return ErrorResponse(generalDetailResult.Item2, HttpStatusCode.NoContent);

                }
                else
                {
                    generalDetailResult = await InsertOrUpdateGeneralDetailWithAttachments(model);
                    if (!generalDetailResult.Item1)
                        return ErrorResponse(generalDetailResult.Item2, HttpStatusCode.NoContent);
                }

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), generalDetailResult.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        // Delete Project
        [Route("projectId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteGeneralDetail(int projectId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get general detail by id
            var general = await _projectFactory.GetGeneralDetailById(projectId);

            //return error if not found
            if (general == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            try
            {
                var generalDetail = await _projectFactory.DeleteGeneralDetail(projectId);
                if (!generalDetail.Item1)
                    return ErrorResponse(generalDetail.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), generalDetail.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion  General Details

        #region Attachment

        //Delete Attachment
        [Route("attachmentId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteAttachment(int attachmentId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get attachment by id
            var attach = await _projectFactory.GetProjectAttachmentById(attachmentId);

            //return error if not found
            if (attach == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            try
            {
                var attachment = await _projectFactory.DeleteAttachment(attachmentId);
                if (!attachment.Item1)
                    return ErrorResponse(attachment.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), attachment.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Attachment

        #region Client Details

        //Get Client Detail By Id
        [Route("GetClientDetailById")]
        [HttpGet]
        public async Task<BaseResponseModel> ClientDetailById(int id)
        {
            try
            {
                //get client detail by id
                var clientDetail = await _projectFactory.GetClientDetailByIdAsync(id);

                //return error if not found
                if (clientDetail == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), clientDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Client DetailId By ProjectId
        [Route("GetClientDetailIdByProjectId")]
        [HttpGet]
        public async Task<BaseResponseModel> GetClientDetailIdByProjectId(int projectId)
        {
            try
            {
                // Get client detail id  by project id 
                var clientDetail = await _projectFactory.GetClientDetailByProjectIdAsync(projectId);

                // return error if not found
                if (clientDetail == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), clientDetail);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Client Detail
        [Route("SaveClientDetail")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateClientDetail([FromBody] ClientDetailModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var clientDetail = await _projectFactory.InsertUpdateClientDetailAsync(model);
                if (!clientDetail.Item1)
                    return ErrorResponse(clientDetail.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), clientDetail.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Client Details

        #region Meetings 

        //Get Meeting By Id
        [Route("GetMeetingById")]
        [HttpGet]
        public async Task<BaseResponseModel> MeetingById(int id)
        {
            try
            {
                //get meeting by id
                var meeting = await _projectFactory.GetMeetingByIdAsync(id);

                //return error if not found
                if (meeting == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), meeting);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Meeting By Project Id
        [Route("GetMeetingByProjectId")]
        [HttpGet]
        public async Task<BaseResponseModel> GetMeetingByProjectId(int projectId)
        {
            try
            {
                // Get meeting by id asynchronously
                var meeting = await _projectFactory.GetMeetingByProjectIdAsync(projectId);

                // Return error if not found
                if (meeting == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), meeting);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Meeting
        [Route("SaveMeetings")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateMeeting([FromBody] APDetails.MeetingModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var meeting = await _projectFactory.InsertUpdateMeetingAsync(model);
                if (!meeting.Item1)
                    return ErrorResponse(meeting.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), meeting.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Meetings 

        #region Billing Info

        //Get Billing Info By Id
        [Route("GetBillingInfoById")]
        [HttpGet]
        public async Task<BaseResponseModel> BillingInfoById(int billingInfoId)
        {
            try
            {
                // Get billing info by id asynchronously
                var billingInfo = await _projectFactory.GetBillingInfoByIdAsync(billingInfoId);

                // Return error if not found
                if (billingInfo == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Billing Info By Project Id
        [Route("GetAllBillingInfoByProjectId")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllBillingInfoByProjectId(int projectId, int pageIndex, int pageSize)
        {
            try
            {
                // Get billing info by id asynchronously
                var billingInfo = await _projectFactory.GetAllBillingInfoByProjectIdAsync(projectId);

                if (billingInfo == null || !billingInfo.Any())
                {
                    // If billing Info are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = billingInfo.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = billingInfo.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Billing Info
        [Route("SaveBillingInfo")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateBillingInfo([FromBody] BillingInfoModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));

            try
            {
                var billingInfo = await _projectFactory.InsertUpdateBillingInfoAsync(model);
                if (!billingInfo.Item1)
                    return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), billingInfo.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Billing Info
        [Route("billingInfoId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteBillingInfo(int billingInfoId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //return error if not found
            if (await _projectFactory.GetBillingInfoByIdAsync(billingInfoId) == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //get billing info by id
            try
            {
                var billingInfo = await _projectFactory.DeleteBillingInfoAsync(billingInfoId);
                if (!billingInfo.Item1)
                    return ErrorResponse(billingInfo.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), billingInfo.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Billing Info

        #region Monthly Planned Hours 

        // Get All MonthlyPlannedHrs i.e List
        [Route("GetAllMonthlyPlannedHrsByBillingInfoId")]
        [HttpGet]
        public async Task<BaseResponseModel> MonthlyPlannedHrs(int pageIndex, int pageSize, int orderId)
        {
            try
            {
                // Get monthly Planned by id asynchronously
                var monthlyPlanned = await _projectFactory.GetAllMonthlyPlannedHrs(orderId);

                // Check for pagination
                if (pageIndex > 0 || pageSize > 0)
                {
                    // Apply pagination
                    var totalItems = monthlyPlanned.MonthlyPlannedListModel.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = monthlyPlanned.MonthlyPlannedListModel.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    // Create the response object
                    var response = new
                    {
                        TotalItems = totalItems,
                        TotalPages = totalPages,
                        Page = pageIndex,
                        PageSize = pageSize,
                        Results = results,
                        TotalHours = monthlyPlanned.TotalHours
                    };
                    return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), monthlyPlanned);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Monthly Planned Hrs By Id
        [Route("GetMonthlyPlannedHrsById")]
        [HttpGet]
        public async Task<BaseResponseModel> MonthlyPlannedHrsById(int id)
        {
            try
            {
                // Get monthly planned by id asynchronously
                var monthlyPlanned = await _projectFactory.GetMonthlyPlannedHrsByIdAsync(id);

                // Return error if not found
                if (monthlyPlanned == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), monthlyPlanned);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Monthly Planned Hrs
        [Route("SaveMonthlyPlannedHrs")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateMonthlyPlannedHrs([FromBody] MonthlyPlannedModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));

            try
            {
                var monthlyPlanned = await _projectFactory.InsertUpdateMonthlyPlannedHrsAsync(model);
                if (!monthlyPlanned.Item1)
                    return ErrorResponse(monthlyPlanned.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), monthlyPlanned.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete MonthlyPlannedHrs
        [Route("monthlyHrsId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteMonthlyPlannedHrs(int monthlyHrsId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //return error if not found
            if (await _projectFactory.GetMonthlyPlannedHrsByIdAsync(monthlyHrsId) == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //get monthly planned hrs by id
            try
            {
                var monthlyPlanned = await _projectFactory.DeleteMonthlyPlannedHrsAsync(monthlyHrsId);
                if (!monthlyPlanned.Item1)
                    return ErrorResponse(monthlyPlanned.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), monthlyPlanned.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Monthly Planned Hours 

        #region Tech stack

        //Get All Tech stack i.e List
        [Route("GetAllTechStackMapping")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllTechStackMapping(int pageIndex, int pageSize)
        {
            try
            {
                // Get TechStack by id
                var tech = await _projectFactory.GetAllTechStackMapping();
                if (tech == null || !tech.Any())
                {
                    // If TechStack is null or empty
                    return ErrorResponse("No project names found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = tech.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = tech.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), tech);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        //Get Tech Stack By Id
        [Route("GetAllTechStackMappingByProjectId")]
        [HttpGet]
        public async Task<BaseResponseModel> TechStackById(int projectid)
        {
            try
            {
                //get Tech Stack by id
                var techStack = await _projectFactory.GetAllTechStackMappingByProjectId(projectid);

                //return error if not found
                if (techStack == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techStack);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        // Delete TechStack
        [Route("DeleteTechStack")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteTechStack(int Id)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get TechStack by id
            var tech = await _projectService.GetTechStackMappingByProjectId(Id);

            //return error if not found
            if (tech == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            try
            {
                var data = await _projectFactory.DeleteTechStack(Id);
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

        //Insert & Update TechStack 
        [Route("SaveTechStackMapping")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateTechStackMapping([FromBody] APDetails.techStackList model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var data = await _projectFactory.SaveTechStackMapping(model);
                if (!data.Item1)
                    return ErrorResponse(data.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), data.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Change Request

        //Get Change Request By Id
        [Route("GetChangeRequestById")]
        [HttpGet]
        public async Task<BaseResponseModel> GetChangeRequestById(int changeRequestId)
        {
            try
            {
                //get changeRequest by id
                var changeRequest = await _projectFactory.GetChangeRequestByIdAsync(changeRequestId);

                //return error if not found
                if (changeRequest == null || changeRequest.Id <= 0)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Billing Info By Project Id
        [Route("GetAllCRByProjectId")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllCRByProjectId(int projectId, int pageIndex, int pageSize)
        {
            try
            {
                // Get change request by id asynchronously
                var changeRequest = await _projectFactory.GetAllCRByProjectIdAsync(projectId);

                if (changeRequest == null || !changeRequest.Any())
                {
                    // If change Request are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = changeRequest.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = changeRequest.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

                // Return error if not found
                if (changeRequest == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Billing Info By Project Id
        [Route("GetChangeRequestAttachment")]
        [HttpGet]
        public async Task<BaseResponseModel> GetChangeRequestAttachment(int crId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //return error if not found
            var cr = await _projectFactory.GetChangeRequestByIdAsync(crId);
            if (cr == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //get change request by id 
            try
            {
                var changeRequest = await _projectFactory.GetCRAttachmentIdByProjectId(crId, cr.ProjectId);
                // Return error if not found
                if (changeRequest == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #region Commented Code

        ////Insert & Update Change Request
        //[Route("SaveChangeRequest")]
        //[HttpPost]
        //public async Task<BaseResponseModel> InsertUpdateChangeRequest([FromForm] ChangeRequestModel model)
        //{
        //    if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
        //        return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

        //    try
        //    {
        //        var changeRequest = await _projectFactory.InsertUpdateChangeRequestAsync(model);
        //        if (!changeRequest.Item1)
        //        {
        //            return ErrorResponse(changeRequest.Item2, HttpStatusCode.NoContent);
        //        }

        //        #region Change Request ATTACHMENTS
        //        try
        //        {
        //            var path = _tmConfig.CRAttachmentFolderPath;
        //            string filePath = string.Concat(_env.ContentRootPath, path);

        //            // Get the list of currently attached files
        //            var existingAttachments = await _projectService.GetAttachmentIdByCRId(changeRequest.Item4);

        //            // Soft delete attachments
        //            foreach (var existingAttachment in existingAttachments)
        //            {
        //                var attachmentFileName = existingAttachment.FileName;

        //                if (!model.AttachFiles.Any(file => file.FileName == attachmentFileName))
        //                {
        //                    existingAttachment.IsClosingDocument = true;
        //                    existingAttachment.ModifiedOn = DateTime.UtcNow;

        //                    await _projectService.UpdateAttachment(existingAttachment);
        //                }
        //            }

        //            #region Attach Files
        //            // Save the uploaded files 
        //            if (model.AttachFiles != null && model.AttachFiles.Count > 0)
        //            {
        //                foreach (var file in model.AttachFiles)
        //                {
        //                    //Split the string by character to get file extension type
        //                    int lastIndex = file.FileName.LastIndexOf('.');
        //                    if (lastIndex + 1 < file.FileName.Length)
        //                    {
        //                        string firstPart = file.FileName.Substring(0, lastIndex);
        //                        string secondPart = file.FileName.Substring(lastIndex + 1);

        //                        string newFileName = $"{firstPart}-{DateTime.UtcNow:yyyyMMdd_hhmmss}." + secondPart;

        //                        //save attachment in table 
        //                        var attachOnly = path.Replace("wwwroot\\", string.Empty) + newFileName;
        //                        await _projectService.InsertAttachment(new Attachment
        //                        {
        //                            FileName = attachOnly,
        //                            CRId = changeRequest.Item4,
        //                            AttachedBy = model.CreatedBy,
        //                            AttachType = (int)AttachmentEnum.AttachmentCR,
        //                            ProjectId = changeRequest.Item3,
        //                            CreatedOn = DateTime.UtcNow
        //                        });

        //                        using (Stream stream = new FileStream(filePath + newFileName, FileMode.Create))
        //                        {
        //                            file.CopyTo(stream);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion Attach Files

        //            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest.Item3);
        //        }
        //        catch (Exception ex)
        //        {
        //            return ErrorResponse(ex, HttpStatusCode.BadRequest);
        //        }

        //        #endregion ATTACHMENTS
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        //    }
        //}

        #endregion Commented Code

        //Insert Change Request
        [Route("CreateChangeRequest")]
        [HttpPost]
        public async Task<BaseResponseModel> CreateChangeRequest([FromForm] ChangeRequestModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            try
            {
                var changeRequest = await _projectFactory.InsertChangeRequestAsync(model);
                if (!changeRequest.Item1)
                {
                    return ErrorResponse(changeRequest.Item2, HttpStatusCode.NoContent);
                }

                // Save the uploaded files 
                if (model.AttachFiles != null && model.AttachFiles.Count > 0)
                {
                    var path = _tmConfig.CRAttachmentFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);

                    foreach (var file in model.AttachFiles)
                    {
                        //Split the string by character to get file extension type
                        int lastIndex = file.FileName.LastIndexOf('.');
                        if (lastIndex + 1 < file.FileName.Length)
                        {
                            string firstPart = file.FileName.Substring(0, lastIndex);
                            string secondPart = file.FileName.Substring(lastIndex + 1);

                            string newFileName = $"{firstPart}-{DateTime.UtcNow:yyyyMMdd_hhmmss}." + secondPart;

                            //save attachment in table 
                            var attachOnly = path.Replace("wwwroot\\", string.Empty) + newFileName;
                            await _projectService.InsertAttachment(new Attachments
                            {
                                FilePath = attachOnly,
                                CreatedOn = DateTime.UtcNow
                            });

                            using (Stream stream = new FileStream(filePath + newFileName, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                }

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Update Change Request
        [Route("UpdateChangeRequest")]
        [HttpPut]
        public async Task<BaseResponseModel> UpdateChangeRequest([FromForm] ChangeRequestModel model)
        {
            if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
                return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);

            try
            {
                //update change request if Id exist
                var changeRequest = await _projectFactory.UpdateChangeRequestAsync(model);
                if (!changeRequest.Item1)
                {
                    return ErrorResponse(changeRequest.Item2, HttpStatusCode.NoContent);
                }

                var checkCRAttachment = await _projectFactory.GetCRAttachmentIdByProjectId(model.Id, model.OrderId);

                //check if CR Attachments
                foreach (var attachment in checkCRAttachment)
                {
                    //check whether attachment id still exist in fetch data
                    var existingAttachmentsList = await _projectService.GetAttachmentIdByCRId(model.Id);
                    var existingAttachmentIds = existingAttachmentsList.Select(x => x.Id).ToList();

                    bool idExists = existingAttachmentIds.Contains(attachment.Id);
                    if (!idExists)
                    {
                        //Soft Delete Attachment
                        foreach (var existingAttachment in existingAttachmentsList)
                        {
                            var attachmentFileName = existingAttachment.FilePath;

                            if (!model.AttachFiles.Any(file => file.FileName == attachmentFileName))
                            {
                                existingAttachment.IsDeleted = true;
                                existingAttachment.ModifiedOn = DateTime.UtcNow;

                                await _projectService.UpdateAttachment(existingAttachment);
                            }
                        }
                    }

                }

                #region Insert Attachments

                var path = _tmConfig.CRAttachmentFolderPath;
                string filePath = string.Concat(_env.ContentRootPath, path);

                // Save the uploaded files 
                if (model.AttachFiles != null && model.AttachFiles.Count > 0)
                {
                    foreach (var file in model.AttachFiles)
                    {
                        //Split the string by character to get file extension type
                        int lastIndex = file.FileName.LastIndexOf('.');
                        if (lastIndex + 1 < file.FileName.Length)
                        {
                            string firstPart = file.FileName.Substring(0, lastIndex);
                            string secondPart = file.FileName.Substring(lastIndex + 1);
                            string newFileName = $"{firstPart}-{DateTime.UtcNow:yyyyMMdd_hhmmss}." + secondPart;

                            //save attachment in table 
                            var attachOnly = path.Replace("wwwroot\\", string.Empty) + newFileName;
                            await _projectService.InsertAttachment(new Attachments
                            {
                                FilePath = attachOnly,
                                CreatedOn = DateTime.UtcNow
                            });

                            using (Stream stream = new FileStream(filePath + newFileName, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                }

                #endregion

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest.Item3);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Change request
        [Route("DeleteChangeRequestId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteChangeRequest(int crId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //return error if not found
            var cr = await _projectFactory.GetChangeRequestByIdAsync(crId);
            if (cr == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //get change request by id 
            try
            {
                var changeRequest = await _projectFactory.DeleteCRIdByProjectIdAsync(crId, cr.ProjectId);
                var attachment = await _projectFactory.DeleteAttachmentIdByCRIdAsync(crId);
                if (!changeRequest.Item1)
                    return ErrorResponse(changeRequest.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), changeRequest.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete AttachmentId
        [Route("DeleteCRAttachmentId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteCRAttachmentId(int crId, int attachId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //check CR exist 
            var cr = await _projectFactory.GetChangeRequestByIdAsync(crId);
            if (cr == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //cr exist then only attachment delete
            try
            {
                var attachment = await _projectFactory.DeleteAttachment(attachId);
                if (!attachment.Item1)
                    return ErrorResponse(attachment.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), attachment.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion Change Request

        #region Other Info 

        //Get All Other Info By Project Id
        [Route("GetOtherInfoRecords")]
        [HttpGet]
        public async Task<BaseResponseModel> GetOtherInfoRecords(int projectId)
        {
            try
            {
                // Get change request by id asynchronously
                var changeRequest = await _projectFactory.GetOtherInfoByProjectId(projectId);

                // Return error if not found
                if (changeRequest == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), changeRequest);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        #endregion OtherInfo

        #region Project Managers
        /// <summary>
        /// Get all project manager data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProjectManagers")]
        public async Task<BaseResponseModel> GetAllProjectManagers()
        {
            var res = await _projectFactory.GetAllProjectManagersAsync();
            return new BaseResponseModel() { StatusCode = 200, Data = res, Message = ConstantValues.Success };
        }
        [HttpPost]
        [Route("SaveProjectManager")]
        public async Task<BaseResponseModel> SaveProjectManager([FromBody] APDetails.ProjectManagers manager)
        {
            await _projectFactory.SaveProjectManager(manager);
            return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
        }
        [HttpPost]
        [Route("SaveResources")]
        public async Task<BaseResponseModel> SaveResources([FromBody] List<ResourceDTO> resource)
        {
            await _projectFactory.SaveResources(resource);
            return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
        }
        #endregion

        #region Projection
        //Insert projection Details
        [Route("SaveProjectionDetail")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertProjectionDetail([FromForm] ProjectionRequestModel model)
        {
            // Validate the public key

            try
            {
                var projectionReequestResult = await _projectFactory.InsertProjectionRequestAsync(model);

                if (!projectionReequestResult.Item1)
                {
                    // Handle error cases
                    return ErrorResponse(projectionReequestResult.Item2, HttpStatusCode.BadRequest);
                }

                return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Update projection Details
        [Route("UpdateProjectionDetail")]
        [HttpPost]
        public async Task<BaseResponseModel> UpdateProjectionDetail([FromForm] UpdateProjectionRequestModel model)
        {
            try
            {
                var projectionReequestResult = await _projectFactory.UpdateProjectionRequestAsync(model);

                if (!projectionReequestResult.Item1)
                {
                    // Handle error cases
                    return ErrorResponse(projectionReequestResult.Item2, HttpStatusCode.BadRequest);
                }

                return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get projection Details by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        [Route("GetAllProjectionListByProjectIdAsync")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllProjectionListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0)
        {
            //// Validate the public key
            //if (!Helper.ValidatePublicKey(_tmConfig, model.PublicKey))
            //{
            //    return ErrorResponse(_localizationService.GetResource("Tm.API.InvalidPublicKey", model.LanguageId), HttpStatusCode.Unauthorized);
            //}

            try
            {
                var projectionRequestResult = await _projectFactory.GetAllProjectionListByProjectIdAsync(projectId, startDate, endDate, languageId); ;
                if (projectionRequestResult == null)
                {
                    // If projects are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectionRequestResult);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Projection By Id
        [Route("GetProjectionById")]
        [HttpGet]
        public async Task<BaseResponseModel> GetProjectionById(int id)
        {
            try
            {
                // Get projection by id asynchronously
                var projection = await _projectFactory.GetProjectionDetailByIdAsync(id);

                // Return error if not found
                if (projection == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projection);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetProjectionCalculationByProjectIdAsync")]
        [HttpGet]
        public async Task<BaseResponseModel> GetProjectionCalculationByProjectIdAsync(int projectId, int languageId = 0)
        {
            try
            {
                var projectionRequestResult = await _projectFactory.GetProjectionCalculationByProjectIdAsync(projectId, languageId); ;
                if (projectionRequestResult == null)
                {
                    // If projects are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectionRequestResult);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region Attendance
        /// <summary>
        /// Insert projection Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SaveAttendanceRequestAsync")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertAttendanceRequestAsync([FromForm] CopyProjectionToAttendanceRequestModel model)
        {
            try
            {
                var copyAttendanceRequestResult = await _projectFactory.InsertAttendanceRequestAsync(model);

                if (!copyAttendanceRequestResult.Item1)
                {
                    // Handle error cases
                    return ErrorResponse(copyAttendanceRequestResult.Item2, HttpStatusCode.BadRequest);
                }

                return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update projection Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateAttendanceRequestAsync")]
        [HttpPost]
        public async Task<BaseResponseModel> UpdateAttendanceRequestAsync([FromForm] UpdateAttendanceRequestModel model)
        {
            
            try
            {
                var copyAttendanceRequestResult = await _projectFactory.UpdateAttendanceRequestAsync(model);

                if (!copyAttendanceRequestResult.Item1)
                {
                    // Handle error cases
                    return ErrorResponse(copyAttendanceRequestResult.Item2, HttpStatusCode.BadRequest);
                }

                return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get projection Details by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        [Route("GetProjectionResourcesForAttendanceIdAsync")]
        [HttpGet]
        public async Task<BaseResponseModel> GetProjectionResourcesForAttendanceIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var projectionRequestResult = await _projectFactory.GetProjectionResourcesForAttendanceIdAsync(projectId, startDate, endDate);
                if (projectionRequestResult == null)
                {
                    // If projects are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectionRequestResult);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get projection Details by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        [Route("GetAllAttendanceListByProjectIdAsync")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllAttendanceListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var projectionRequestResult = await _projectFactory.GetAllAttendanceListByProjectIdAsync(projectId, startDate, endDate);
                if (projectionRequestResult == null)
                {
                    // If projects are null or empty
                    return ErrorResponse("No project found.", HttpStatusCode.NotFound);
                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectionRequestResult);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get Projection By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetAttendanceDetailByIdAsync")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAttendanceDetailByIdAsync(int id)
        {
            try
            {
                // Get projection by id asynchronously
                var projection = await _projectFactory.GetAttendanceDetailByIdAsync(id);

                // Return error if not found
                if (projection == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projection);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #endregion Method

    }
}