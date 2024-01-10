using API.Models.BaseModels;
using API.Models.Attachments;
using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Tm.Core;
using Tm.Core.Configuration;
using Tm.Core.Constants;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Data;
using Tm.Services.Customers;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.ProjectDetail;
using Tm.Services.Pms.UserRole;
using Tm.Core.Domain.Pms.Attendance;
using API.Models.Attendance;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using API.Models.AppSetting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using API.Models.UserRole;
using System.Security.Policy;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace API.Factories.ProjectDetail
{
    public class ProjectsFactory : IProjectsFactory
    {
        #region Fields
        private readonly IProjectsService _projectService;
        private readonly IMasterDataService _masterDataService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private readonly IAttachmentsService _attachmentService;
        private readonly IOrderService _orderService;
        private readonly ICustomersService _customersService;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRoleService _userRoleService;
        private readonly AD _adConfig;
        #endregion

        #region Ctor
        public ProjectsFactory(IProjectsService projectService,
            IMasterDataService masterDataService,
            IWorkContext workContext,
            ICustomerService customerService,
            IMapper mapper,
            TmConfig tmConfig,
            IWebHostEnvironment env,
            IAttachmentsService attachmentService,
            IOrderService orderService,
            ICustomersService customersService,
            IHttpContextAccessor httpContextAccessor,
            IUserRoleService userRoleService,
            IOptions<AD> adConfig)
        {
            _projectService = projectService;
            _masterDataService = masterDataService;
            _workContext = workContext;
            _customerService = customerService;
            _mapper = mapper;
            _tmConfig = tmConfig;
            _env = env;
            _attachmentService = attachmentService;
            _orderService = orderService;
            _customersService = customersService;
            _httpContextAccessor = httpContextAccessor;
            _userRoleService = userRoleService;
            _adConfig = adConfig.Value;
        }
        #endregion

        #region Utilities
        private string GetCustomerUsername(int customerId)
        {
            try
            {
                var customer = _customerService.GetCustomerById(customerId);
                return customer?.Username ?? ConstantValues.UnknownUser;
            }
            catch (Exception ex)
            {
                //get unknow user if user isn't register
                return ConstantValues.UnknownUser;
            }
        }
        private async Task<string> GetManagerName(int managerId)
        {
            try
            {
                //var manager = await _projectService.GetProjectManagersById(managerId);
                //return manager?.ManagerName ?? ConstantValues.UnknownUser;
                var result = new ProjectManagerDetailResponseModel();
                var projectManagerDetail = await _userRoleService.GetADUserById(managerId);
                return projectManagerDetail?.Name ?? ConstantValues.UnknownUser;
            }
            catch (Exception ex)
            {
                //get unknow user if user isn't register
                return ConstantValues.UnknownUser;
            }
        }
        #endregion Utilities

        #region Methods

        #region Project Listing 

        /// <summary>
        /// Get General Detail
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<int, List<ProjectListModel>>> GetAllProjectAsync(int projectNameId, int projectTypeId, int managerId, int projectStatusId, int pageIndex, int pageSize)
        {
            try
            {
                List<ProjectListModel> result = new List<ProjectListModel>();

                var client = await _projectService.GetAllClientDetail();
                var data = await _projectService.GetAllProjectsWithFilter(projectNameId, projectTypeId, managerId, projectStatusId, pageIndex, pageSize);

                foreach (var project in data.Item2)
                {
                    ProjectListModel model = new ProjectListModel();
                    model.ProjectId = project.Id;
                    model.ProjectNameId = project.ProjectNameId;
                    model.ProjectName = project.Name;
                    model.IsAzure = project.IsAzure;
                    if (project.OrderId > 0)
                    {
                        var orderDetail = await _orderService.GetOrdersById(project.OrderId);
                        if (orderDetail != null)
                        {
                            var customerDetail = await _customersService.GetCustomerById(orderDetail.CustomerId);
                            model.ClientName = customerDetail != null && !string.IsNullOrEmpty(customerDetail.Name) ? customerDetail?.Name : ConstantValues.NoClientExist;

                            model.ProjectTypeId = orderDetail.SOWDocumentId;

                            var projectType = await _masterDataService.GetProjectTypeById(orderDetail.SOWDocumentId);
                            model.ProjectType = projectType != null && !string.IsNullOrEmpty(projectType.Name) ? projectType.Name : ConstantValues.NoProjectTypeExist;
                        }
                    }


                    model.ManagerId = project.ManagerId; //too-do manager record is not save table
                    model.ManagerName = await GetManagerName(project.ManagerId);

                    model.ProjectStatusId = project.ProjectStatusId;

                    var projectStatus = await _masterDataService.GetProjectStatusById(project.ProjectStatusId);
                    if (projectStatus != null)
                        model.ProjectStatus = projectStatus.Name;

                    result.Add(model);
                }

                return Tuple.Create(data.Item1, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingProjectData, ex);
            }
        }

        #endregion Project Listing 

        #region Project Detail
        /// <summary>
        /// Get Project data by Project ID
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ProjectResponseModel> GetProjectDetailByIdAsync(int projectId)
        {
            try
            {

                ProjectResponseModel projectModelList = new ProjectResponseModel();
                var client = await _projectService.GetAllClientDetail();
                var project = await _projectService.GetProjectsById(projectId);
                if (project != null)
                {

                    projectModelList.Id = project.Id;
                    projectModelList.ProjectNameId = project.ProjectNameId;
                    projectModelList.Name = project.Name;
                    projectModelList.OrderId = project.OrderId;
                    if (project.OrderId > 0)
                    {
                        var orderDetail = await _orderService.GetOrdersById(project.OrderId);
                        if (orderDetail != null)
                        {
                            var customerDetail = await _customersService.GetCustomerById(orderDetail.CustomerId);
                            projectModelList.ClientName = customerDetail != null && !string.IsNullOrEmpty(customerDetail.Name) ? customerDetail?.Name : ConstantValues.NoClientExist;
                        }
                    }
                    projectModelList.Description = project.Description;
                    projectModelList.InHouse = project.InHouse;
                    projectModelList.BillingVisibleToManager = project.BillingVisibleToManager;
                    projectModelList.ProjectStatusId = project.ProjectStatusId;
                    var projectStatus = await _masterDataService.GetProjectStatusById(project.ProjectStatusId);
                    if (projectStatus != null)
                        projectModelList.ProjectStatus = projectStatus.Name;
                    projectModelList.ClientId = project.ClientId;
                    projectModelList.ManagerId = project.ManagerId;
                    projectModelList.ManagerName = await GetManagerName(project.ManagerId);
                    projectModelList.PlannedStartDate = project.PlannedStartDate;
                    projectModelList.PlannedEndDate = project.PlannedEndDate;
                    projectModelList.ActualStartDate = project.ActualStartDate;
                    projectModelList.ActualEndDate = project.ActualEndDate;
                    projectModelList.DelayReasonStartDate = project.DelayReasonStartDate;
                    projectModelList.DelayReasonEndDate = project.DelayReasonEndDate;
                    projectModelList.CreatedBy = project.CreatedBy;
                    projectModelList.CreatedOn = project.CreatedOn;
                    projectModelList.ModifiedBy = project.ModifiedBy;
                    projectModelList.ModifiedOn = project.ModifiedOn;
                    projectModelList.IsDeleted = project.IsDeleted;
                    projectModelList.ProjectDomainId = project.ProjectDomainId;
                    projectModelList.EffortsDuration = project.EffortsDuration;
                    projectModelList.EstimatedEffortUnit = project.EstimatedEffortUnit;
                    projectModelList.CurrencyId = project.CurrencyId;
                    projectModelList.ProjectCostValue = project.ProjectCostValue;
                    projectModelList.HourlyCostValue = project.HourlyCostValue;
                    projectModelList.EstTotalHours = project.EstTotalHours;
                    projectModelList.Azure_ProjectId = project.Azure_ProjectId;
                    projectModelList.CommunicationModeId = project.CommunicationModeId;
                    projectModelList.ProjectTypeId = project.ProjectTypeId;
                    projectModelList.IsAzure = project.IsAzure;
                    var projectType = await _masterDataService.GetProjectTypeById(project.ProjectTypeId);
                    projectModelList.ProjectType = projectType != null && !string.IsNullOrEmpty(projectType.Name) ? projectType.Name : ConstantValues.NoProjectTypeExist;

                    //Project Attachments
                    projectModelList.ProjectAttachment = await GetProjectAttachmentById(project.Id);


                }
                return projectModelList;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingProjectData, ex);
            }
        }

        /// <summary>
        /// Get General Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<GeneralDetailResponseModel> GetGeneralDetailById(int id)
        {
            var result = new GeneralDetailResponseModel();
            var generalDetail = await _projectService.GetProjectsById(id);
            try
            {
                if (generalDetail.IsDeleted == true)
                    throw new Exception(ConstantValues.NoRecordFoundForUpdate);

                if (generalDetail != null)
                {
                    result.Id = generalDetail.Id;
                    if (generalDetail.Id == (int)ProjectEnum.Other)
                    {
                        result.Name = generalDetail.Name;
                    }
                    result.ProjectNameId = generalDetail.ProjectNameId;
                    result.ProjectTypeId = generalDetail.ProjectTypeId;
                    result.ProjectDomainId = generalDetail.ProjectDomainId;
                    result.EffortsDuration = generalDetail.EffortsDuration;
                    result.EstimatedEffortUnit = generalDetail.EstimatedEffortUnit;
                    result.Description = generalDetail.Description;
                    result.CurrencyId = generalDetail.CurrencyId;
                    result.ProjectCostValue = generalDetail.ProjectCostValue;
                    result.HourlyCostValue = generalDetail.HourlyCostValue;
                    result.ProjectStatusId = generalDetail.ProjectStatusId;
                    result.EstTotalHours = await _projectService.GetProjectTotalHoursAsync(id);
                    if (result.EstTotalHours != 0)
                    {
                        generalDetail.EstTotalHours = result.EstTotalHours;
                        await _projectService.UpdateProjects(generalDetail);
                    }
                    result.InHouse = generalDetail.InHouse;
                    result.BillingVisibleToManager = generalDetail.BillingVisibleToManager;
                    result.DelayReasonStartDate = generalDetail.DelayReasonStartDate;
                    result.DelayReasonEndDate = generalDetail.DelayReasonEndDate;
                    result.ManagerId = generalDetail.ManagerId;
                    result.CreatedBy = generalDetail.CreatedBy;
                    result.ModifiedBy = generalDetail.ModifiedBy;
                    result.IsAzure = generalDetail.IsAzure;
                    #region Attachments

                    //Get all attachment files
                    result.Attachments = await GetProjectAttachmentById(generalDetail.Id);
                    #endregion Attachments

                }
                return result;
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }

        /// <summary>
        /// Insert & Update General Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateGenDetail(GenDetailModel model)
        {
            string savedProjectName = string.Empty;
            Projects generalDetail = new Projects();
            if (model.Id > 0)
            {
                generalDetail = await _projectService.GetProjectsById(model.Id);
            }
            //Name will be used to compare the project in the dev then it will updat the azure_projectId
            savedProjectName = generalDetail.Name;
            generalDetail.OrderId = model.OrderId;
            generalDetail.Name = model.Name;
            generalDetail.Description = model.Description;
            generalDetail.InHouse = model.InHouse;
            generalDetail.BillingVisibleToManager = model.BillingVisibleToManager;
            generalDetail.ProjectStatusId = model.ProjectStatusId;
            generalDetail.ClientId = model.ClientId;
            generalDetail.ManagerId = model.ManagerId;
            generalDetail.PlannedStartDate = model.PlannedStartDate;
            generalDetail.PlannedEndDate = model.PlannedEndDate;
            generalDetail.ActualStartDate = model.ActualStartDate;
            generalDetail.ActualEndDate = model.ActualEndDate;
            generalDetail.DelayReasonStartDate = model.DelayReasonStartDate;
            generalDetail.DelayReasonEndDate = model.DelayReasonEndDate;
            generalDetail.ProjectDomainId = model.ProjectDomainId;
            generalDetail.CommunicationModeId = model.CommunicationModeId;
            generalDetail.IsAzure = model.IsAzure;
            if (model.Id > 0)
            {
                generalDetail.Id = model.Id;
                //generalDetail.ModifiedBy = _workContext.CurrentCustomer.Id;
                generalDetail.ModifiedBy = model.ModifiedBy;
                generalDetail.ModifiedOn = DateTime.UtcNow;
                await _projectService.UpdateProjects(generalDetail);
                model.Id = generalDetail.Id;
            }
            else
            {
                //generalDetail.CreatedBy = _workContext.CurrentCustomer.Id;
                generalDetail.CreatedBy = model.CreatedBy;
                generalDetail.CreatedOn = DateTime.UtcNow;
                generalDetail.IsDeleted = false;
                await _projectService.InsertProjects(generalDetail);
                model.Id = generalDetail.Id;
            }
            if (model.FileDataList != null)
            {
                var path = _tmConfig.ProjectAttachmentFolderPath;
                string filePath = string.Concat(_env.ContentRootPath, path);
                foreach (var fileData in model.FileDataList)
                {
                    if (fileData != null)
                    {
                        int lastIndex = fileData.FileName.LastIndexOf('.');

                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, fileData.FileName);

                        //create folder if not exist
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            //get file extension
                            FileInfo fileInfo = new FileInfo(fileData.FileName);
                            string fileName = fileData.FileName;

                            string fileNameWithPath = Path.Combine(filePath, fileName);

                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                fileData.CopyTo(stream);
                            }

                        }
                        catch (Exception ex)
                        {
                            // Log the exception message for debugging
                            Console.WriteLine($"Image.Save Error: {ex.Message}");
                            // Handle the exception or rethrow it as needed
                        }
                        string contentDirectory = "Content"; // The directory you want to extract

                        int contentIndex = fullPath.IndexOf(contentDirectory);
                        if (contentIndex != -1)
                        {
                            string relativePath = fullPath.Substring(contentIndex);
                            var updatedPath = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", relativePath).Replace("\\", "/");

                            // Create and insert an Attachment object with the newFileName
                            var attachment = new Attachments();
                            attachment.FilePath = updatedPath;
                            attachment.FileName = fileData.FileName; // Use the newFileName here
                            attachment.CreatedBy = model.CreatedBy;
                            attachment.ModifiedBy = model.ModifiedBy;
                            attachment.CreatedOn = DateTime.UtcNow;
                            attachment.ModifiedOn = DateTime.UtcNow;
                            attachment.ModifiedBy = model.ModifiedBy;
                            await _attachmentService.InsertAttachment(attachment);

                            // Create and insert a ProjectAttachment object
                            var projectAttachment = new ProjectAttachment();
                            projectAttachment.ProjectId = model.Id;
                            projectAttachment.AttachmentId = attachment.Id;
                            projectAttachment.CreatedOn = DateTime.UtcNow;
                            projectAttachment.ModifiedOn = DateTime.UtcNow;
                            projectAttachment.CreatedBy = model.CreatedBy;
                            projectAttachment.ModifiedBy = model.ModifiedBy;
                            projectAttachment.IsDeleted = false;
                            await _attachmentService.InsertProjectAttachment(projectAttachment);
                        }
                    }
                }
            }
            //TODO:call Azure API 
            if (model.IsAzure)
            {
                if (string.IsNullOrEmpty(generalDetail.Azure_ProjectId) || (savedProjectName != model.Name && !string.IsNullOrEmpty(model.Name)))
                {

                    string personalAccessToken = _adConfig.PersonalAccessToken;
                    string endPoint = _adConfig.ProjectsByName;
                    endPoint = endPoint + model.Name;
                    string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));

                    //ListOfProjectsResponse.Projects viewModel = null;
                    string jsonResponse = "";
                    //use the httpclient
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(endPoint);  //url of your organization
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        //connect to the REST endpoint            
                        HttpResponseMessage response = client.GetAsync(endPoint).Result;

                        //check to see if we have a successful response
                        if (response.IsSuccessStatusCode)
                        {
                            //set the viewmodel from the content in the response
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                jsonResponse = await response.Content.ReadAsStringAsync();
                            }
                            if (!string.IsNullOrEmpty(jsonResponse))
                            {
                                var projectData = JsonSerializer.Deserialize<ADProjects>(jsonResponse);
                                if (projectData != null)
                                {
                                    generalDetail.Azure_ProjectId = projectData.id;
                                    await _projectService.UpdateProjects(generalDetail);
                                }
                            }
                        }
                    }
                }
            }
            return (true, ConstantValues.Success, model.Id);
        }



        public async Task<(bool, string, int)> InsertUpdateGeneralDetail(GeneralDetailModel model)
        {
            Projects generalDetail = new Projects();

            if (model.Id > 0)
            {
                generalDetail = await _projectService.GetProjectsById(model.Id);
                generalDetail.Name = (await _masterDataService.GetProjectNameById(model.ProjectNameId)).Name;
                if (generalDetail == null)
                {
                    return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                }
            }
            else
            {
                if (model.ProjectNameId == (int)ProjectEnum.Other)
                {
                    generalDetail.ProjectNameId = await _masterDataService.GetProjectIdByNameAsync(model.Name);
                }
                else
                {
                    generalDetail.ProjectNameId = model.ProjectNameId;
                    generalDetail.Name = (await _masterDataService.GetProjectNameById(model.ProjectNameId)).Name;
                }
            }
            generalDetail.Name = string.IsNullOrEmpty(model.Name) ? generalDetail.Name : model.Name;
            generalDetail.ProjectTypeId = model.ProjectTypeId == 0 ? generalDetail.ProjectTypeId : model.ProjectTypeId;
            generalDetail.ProjectDomainId = model.ProjectDomainId == 0 ? generalDetail.ProjectDomainId : model.ProjectDomainId;
            generalDetail.EffortsDuration = model.EffortsDuration == 0 ? generalDetail.EffortsDuration : model.EffortsDuration;
            generalDetail.EstimatedEffortUnit = model.EstimatedEffortUnit == 0 ? generalDetail.EstimatedEffortUnit : model.EstimatedEffortUnit;
            generalDetail.Description = string.IsNullOrEmpty(model.Description) ? generalDetail.Description : model.Description;
            generalDetail.CurrencyId = model.CurrencyId == 0 ? generalDetail.CurrencyId : model.CurrencyId;
            generalDetail.ProjectCostValue = model.ProjectCostValue == 0 ? generalDetail.ProjectCostValue : model.ProjectCostValue;
            generalDetail.HourlyCostValue = model.HourlyCostValue == 0 ? generalDetail.HourlyCostValue : model.HourlyCostValue;
            generalDetail.ProjectStatusId = model.ProjectStatusId == 0 ? generalDetail.ProjectStatusId : model.ProjectStatusId;
            generalDetail.InHouse = model.InHouse || generalDetail.InHouse;
            generalDetail.BillingVisibleToManager = model.BillingVisibleToManager || generalDetail.BillingVisibleToManager;
            generalDetail.DelayReasonStartDate = string.IsNullOrEmpty(model.DelayReasonStartDate) ? generalDetail.DelayReasonStartDate : model.DelayReasonStartDate;
            generalDetail.DelayReasonEndDate = string.IsNullOrEmpty(model.DelayReasonEndDate) ? generalDetail.DelayReasonEndDate : model.DelayReasonEndDate;
            generalDetail.IsAzure = generalDetail.IsAzure;
            if (model.Id > 0)
            {
                generalDetail.EstTotalHours = await _projectService.GetProjectTotalHoursAsync(model.Id);
                generalDetail.PlannedStartDate = model.PlannedStartDate == null ? generalDetail.PlannedStartDate : model.PlannedStartDate;
                generalDetail.PlannedEndDate = model.PlannedEndDate == null ? generalDetail.PlannedEndDate : model.PlannedEndDate;
                generalDetail.ActualStartDate = model.ActualStartDate == null ? generalDetail.ActualStartDate : model.ActualStartDate;
                generalDetail.ActualEndDate = model.ActualEndDate == null ? generalDetail.ActualStartDate : model.ActualStartDate;
                generalDetail.ModifiedBy = model.ModifiedBy;
                generalDetail.ModifiedOn = DateTime.UtcNow;
                await _projectService.UpdateProjects(generalDetail);
            }
            else
            {
                generalDetail.PlannedStartDate = model.PlannedStartDate == null ? DateTime.UtcNow : model.PlannedStartDate;
                generalDetail.PlannedEndDate = model.PlannedEndDate == null ? DateTime.UtcNow : model.PlannedEndDate;
                generalDetail.ActualStartDate = model.ActualStartDate == null ? DateTime.UtcNow : model.ActualStartDate;
                generalDetail.ActualEndDate = model.ActualEndDate == null ? DateTime.UtcNow : model.ActualStartDate;
                generalDetail.CreatedBy = model.CreatedBy;
                generalDetail.CreatedOn = DateTime.UtcNow;
                generalDetail.IsDeleted = false;
                await _projectService.InsertProjects(generalDetail);
            }

            return (true, ConstantValues.Success, generalDetail.Id);
        }

        /// <summary>
        /// Delete General Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteGeneralDetail(int id)
        {
            if (id <= 0)
            {
                return (false, ConstantValues.InvalidId);
            }

            var generalDetail = await _projectService.GetProjectsById(id);

            if (generalDetail == null)
            {
                return (false, "No record found for delete");
            }

            // Check if the current user is authorized to delete the record
            if (_workContext.CurrentCustomer == null)
            {
                //if user is super admin then only record can be removed
                generalDetail.ModifiedBy = 1;
            }
            generalDetail.ModifiedOn = DateTime.UtcNow;
            #region Attachments
            // check the attachments if there are any
            var attachmentsToUpdate = await _projectService.GetProjectsAttachmentsByProjectId(generalDetail.Id);
            if (attachmentsToUpdate.Any())
            {
                foreach (var attachment in attachmentsToUpdate)
                {
                    if (_workContext.CurrentCustomer == null)
                    {
                        //if user is super admin then only record can be removed
                        attachment.ModifiedBy = 1;
                    }
                    attachment.IsDeleted = true;
                    attachment.ModifiedOn = DateTime.UtcNow;
                    var attachementData = await _projectService.GetAttachmentById(attachment.AttachmentId);
                    if (attachementData != null)
                    {
                        if (_workContext.CurrentCustomer == null)
                        {
                            //if user is super admin then only record can be removed
                            attachementData.ModifiedBy = 1;
                        }
                        attachementData.ModifiedOn = DateTime.UtcNow;
                        attachementData.IsDeleted = true;
                        await _projectService.UpdateAttachment(attachementData);
                    }
                    await _projectService.UpdateProjectAttachment(attachment);
                }
            }
            #endregion Attachments

            #region Client Detail
            // check the client detail if there are any
            var allClientDetail = await _projectService.GetAllClientDetail();
            var clientDetailToUpdate = allClientDetail.Where(cd => cd.ProjectId == id).ToList();
            if (clientDetailToUpdate.Any())
            {
                foreach (var clientDetail in clientDetailToUpdate)
                {
                    clientDetail.IsDeleted = true;
                    await _projectService.UpdateClientDetail(clientDetail);
                }
            }

            #endregion Client Detail

            #region Meeting
            // check the meeting if there are any
            var allMeeting = await _projectService.GetAllMeeting();
            var meetingToUpdate = allMeeting.Where(cd => cd.ProjectId == id).ToList();
            if (meetingToUpdate.Any())
            {
                foreach (var meeting in meetingToUpdate)
                {
                    meeting.IsDeleted = true;
                    await _projectService.UpdateMeeting(meeting);
                }
            }

            #endregion Meeting

            #region Billing Info
            // check the billing info if there are any
            var allBillingInfo = await _projectService.GetAllBillingInfoAsync();
            var billingInfoToUpdate = allBillingInfo.Where(cd => cd.ProjectId == id).ToList();
            if (billingInfoToUpdate.Any())
            {
                foreach (var billingInfo in billingInfoToUpdate)
                {
                    billingInfo.IsDeleted = true;
                    await _projectService.UpdateBillingInfoAsync(billingInfo);
                }
            }
            #endregion Billing Info

            #region Monthly Planned Hours
            // check the monthly planned hours if there are any
            //var allMonthlyPlannedHrs = await _projectService.GetAllMonthlyHrsAsync();
            //var monthlyPlannedToUpdate = allMonthlyPlannedHrs.Where(cd => cd.Id == id).ToList();
            //if (monthlyPlannedToUpdate.Any())
            //{
            //    foreach (var monthlyPlanned in monthlyPlannedToUpdate)
            //    {
            //        monthlyPlanned.IsDeleted = true;
            //        await _projectService.UpdateMonthlyHrsAsync(monthlyPlanned);
            //    }
            //}
            #endregion Monthly Planned Hours

            #region Change Request
            // check the  change request if there are any
            var allChangeRequest = await _projectService.GetAllChangeRequestAsync();
            var changeRequestToUpdate = allChangeRequest.Where(cd => cd.OrderId == id).ToList();
            if (changeRequestToUpdate.Any())
            {
                foreach (var changeRequest in changeRequestToUpdate)
                {

                    changeRequest.IsDeleted = true;
                    await _projectService.UpdateChangeRequestAsync(changeRequest);
                }
            }
            #endregion Change Request

            #region Other Info
            // check the other info if there are any
            var otherInfo = await _projectService.GetOtherInfoByIdAsync(id);
            if (otherInfo != null)
            {
                otherInfo.IsDeleted = true;
                await _projectService.UpdateOtherInfoAsync(otherInfo);
            }
            #endregion Other Info

            generalDetail.ModifiedOn = DateTime.UtcNow;
            generalDetail.IsDeleted = true;

            // Update the generalDetail record
            await _projectService.UpdateProjects(generalDetail);

            return (true, ConstantValues.Success);
        }

        #endregion Project Detail

        #region Attachment

        /// <summary>
        /// Get All Attachment
        /// </summary>
        /// <returns></returns>
        public async Task<List<AttachmentModel>> GetAllAttachment()
        {
            var data = await _projectService.GetAllAttachment();

            List<AttachmentModel> result = data.Select(x => new AttachmentModel
            {
                Id = x.Id,
                FilePath = x.FilePath,
                FileName = x.FilePath.Split('\\').Last().Trim(),
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn,
            }).ToList();

            return result.OrderByDescending(x => x.Id).ToList();
        }

        /// <summary>
        /// Delete Attachment
        /// </summary>
        /// <param name="attachId"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteAttachment(int attachId)
        {
            if (attachId > 0)
            {
                var attachment = await _projectService.GetAttachmentById(attachId);
                if (attachment != null)
                {
                    attachment.ModifiedOn = DateTime.UtcNow;
                    attachment.IsDeleted = true;
                    await _projectService.UpdateAttachment(attachment);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }

            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate);
            }
        }
        public virtual async Task<List<ProjectAttachmentModel>> GetProjectAttachmentById(int id)
        {
            var result = new ProjectAttachmentModel();
            try
            {
                var attachment = await _projectService.GetProjectsAttachmentsByProjectId(id);
                if (attachment != null)
                {


                    return attachment.Select(x => new ProjectAttachmentModel()
                    {
                        Id = x.Id,
                        IsDeleted = x.IsDeleted,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        AttachBy = x.CreatedBy > 0 ? _customerService.GetCustomerById(x.CreatedBy).Id : 0,
                        ModifiedBy = x.ModifiedBy > 0 ? _customerService.GetCustomerById(x.ModifiedBy).Id : 0,
                        ModifiedOn = x.ModifiedOn,
                        Attachment = x.AttachmentId > 0 ? GetAttachmentById(x.AttachmentId).Result : null,

                    }).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }
        /// <summary>
        /// Get Attachment By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<AttachmentModel> GetAttachmentById(int id)
        {
            var result = new AttachmentModel();
            var attachment = await _projectService.GetAttachmentById(id);
            try
            {
                if (attachment != null)
                {
                    result.Id = attachment.Id;
                    result.FilePath = attachment.FilePath;
                    result.CreatedOn = attachment.CreatedOn;
                    result.AttachedBy = attachment.CreatedBy > 0 ? _customerService.GetCustomerById(attachment.CreatedBy).Id : 0;
                    result.ModifiedOn = attachment.ModifiedOn;
                }
                return result;
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }

        /// <summary>
        /// Delete AttachmentId By CRId
        /// </summary>
        /// <param name="crId"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteAttachmentIdByCRIdAsync(int crId)
        {
            return (false, ConstantValues.NoRecordFoundForUpdate);
        }

        #endregion Attachment

        #region Client Details

        /// <summary>
        /// Get Client Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClientDetailModel> GetClientDetailByIdAsync(int id)
        {
            var model = new ClientDetailModel();
            var clientDetail = await _projectService.GetClientDetailById(id);
            try
            {
                if (clientDetail != null)
                {
                    model = _mapper.Map<ClientDetailModel>(clientDetail);
                    model.CommunicationMode = (await _masterDataService.GetCommunicationModesByIdAsync(clientDetail.CommunicationModeId)).Name;
                }
                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ConstantValues.NoRecordFoundForUpdate, ex);
            }
        }

        /// <summary>
        /// Get ClientDetailId By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>      
        public async Task<ClientDetailModel> GetClientDetailByProjectIdAsync(int projectId)
        {
            try
            {
                var clientDetail = await _projectService.GetClientDetailByProjectIdAsync(projectId);

                if (clientDetail != null && !clientDetail.IsDeleted)
                {
                    var model = _mapper.Map<ClientDetailModel>(clientDetail);
                    model.CommunicationMode = (await _masterDataService.GetCommunicationModesByIdAsync(clientDetail.CommunicationModeId)).Name;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingClientInfo, ex);
            }
        }

        /// <summary>
        /// Insert & Update Client Detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateClientDetailAsync(ClientDetailModel model)
        {
            try
            {
                var clientDetail = new ClientDetail();
                if (model.Id > 0)
                {
                    clientDetail = await _projectService.GetClientDetailById(model.Id);
                    if (clientDetail != null)
                    {
                        clientDetail = _mapper.Map<ClientDetail>(model);
                        clientDetail.ModifiedOn = DateTime.UtcNow;
                        await _projectService.UpdateClientDetail(clientDetail);
                        return (true, ConstantValues.Success, clientDetail.ProjectId);
                    }
                    else
                    {
                        return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                    }
                }
                else
                {
                    clientDetail = _mapper.Map<ClientDetail>(model);
                    clientDetail.CreatedOn = DateTime.UtcNow;
                    clientDetail.IsDeleted = false;
                    await _projectService.InsertClientDetail(clientDetail);
                    return (true, ConstantValues.Success, clientDetail.ProjectId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingClientInfo, ex);
            }
        }

        #endregion Client Details

        #region Meetings  

        /// <summary>
        /// Get Meeting By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<MeetingModel> GetMeetingByIdAsync(int id)
        {
            try
            {
                var meeting = await _projectService.GetMeetingById(id);

                if (meeting != null && !meeting.IsDeleted)
                {
                    var model = _mapper.Map<MeetingModel>(meeting);
                    model.ProjectName = (await _projectService.GetProjectsById(meeting.ProjectId)).Name;
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.ErrorFetchingMeetingInfo);
            }
        }

        /// <summary>
        /// Get Meeting By ProjectId Async
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<MeetingModel> GetMeetingByProjectIdAsync(int projectId)
        {
            var model = new MeetingModel();
            var meeting = await _projectService.GetMeetingByProjectIdAsync(projectId);

            try
            {
                if (meeting != null)
                {
                    model = _mapper.Map<MeetingModel>(meeting);
                    model.ProjectName = (await _projectService.GetProjectsById(meeting.ProjectId)).Name;
                }
                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ConstantValues.ErrorFetchingMeetingInfo, ex);
            }
        }

        /// <summary>
        /// Insert & Update Meeting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateMeetingAsync(MeetingModel model)
        {
            try
            {
                var meeting = new Meetings();
                if (model.Id > 0)
                {
                    meeting = await _projectService.GetMeetingById(model.Id);
                    if (meeting != null)
                    {
                        meeting = _mapper.Map<Meetings>(model);
                        meeting.ModifiedOn = DateTime.UtcNow;
                        await _projectService.UpdateMeeting(meeting);
                        return (true, ConstantValues.Success, meeting.ProjectId);
                    }
                    else
                    {
                        return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                    }
                }
                else
                {
                    meeting = _mapper.Map<Meetings>(model);
                    meeting.CreatedOn = DateTime.UtcNow;
                    meeting.IsDeleted = false;
                    await _projectService.InsertMeeting(meeting);
                    return (true, ConstantValues.Success, meeting.ProjectId);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ConstantValues.ErrorFetchingMeetingInfo, ex);
            }
        }

        #endregion Meetings 

        #region Billing Info

        /// <summary>
        /// Get Billing Info By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<BillingInfoModel> GetBillingInfoByIdAsync(int id)
        {
            try
            {
                var billingInfo = await _projectService.GetBillingInfoByIdAsync(id);

                if (billingInfo != null && !billingInfo.IsDeleted)
                {
                    var model = _mapper.Map<BillingInfoModel>(billingInfo);
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.ErrorFetchingBillingInfo);
            }
        }

        /// <summary>
        /// Insert & Update Billing Info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateBillingInfoAsync(BillingInfoModel model)
        {
            try
            {
                var billingInfo = new BillingInfo();
                if (model.Id > 0)
                {
                    billingInfo = await _projectService.GetBillingInfoByIdAsync(model.Id);
                    if (billingInfo != null)
                    {
                        billingInfo = _mapper.Map<BillingInfo>(model);
                        billingInfo.ModifiedOn = DateTime.UtcNow;
                        await _projectService.UpdateBillingInfoAsync(billingInfo);
                        return (true, ConstantValues.Success, billingInfo.ProjectId);
                    }
                    else
                    {
                        return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                    }
                }
                else
                {
                    billingInfo = _mapper.Map<BillingInfo>(model);
                    billingInfo.CreatedOn = DateTime.UtcNow;
                    billingInfo.IsDeleted = false;
                    await _projectService.InsertBillingInfoAsync(billingInfo);
                    return (true, ConstantValues.Success, billingInfo.ProjectId);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ConstantValues.ErrorFetchingBillingInfo, ex);
            }
        }

        /// <summary>
        /// Delete Billing Info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteBillingInfoAsync(int id)
        {
            if (id > 0)
            {
                var billingInfo = await _projectService.GetBillingInfoByIdAsync(id);
                if (billingInfo != null)
                {
                    if (_workContext.CurrentCustomer == null)
                    {
                        // If user is a super admin, then only the record can be removed
                        billingInfo.ModifiedBy = 1;
                    }
                    billingInfo.ModifiedOn = DateTime.UtcNow;
                    billingInfo.IsDeleted = true;
                    await _projectService.UpdateBillingInfoAsync(billingInfo);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate);
            }
        }

        /// <summary>
        /// Get All BillingInfo By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<BillingInfoModel>> GetAllBillingInfoByProjectIdAsync(int projectId)
        {
            try
            {
                var billingList = new List<BillingInfoModel>();
                var billingInfo = await _projectService.GetAllBillingInfoByProjectIdAsync(projectId);

                if (billingInfo != null)
                {
                    foreach (var item in billingInfo)
                    {
                        var model = _mapper.Map<BillingInfoModel>(item);
                        billingList.Add(model);
                    }
                }

                return billingList;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingBillingInfo, ex);
            }
        }

        #endregion Billing Info

        #region Monthly Planned Hours 

        /// <summary>
        /// Get All MonthlyPlannedHrs
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<MonthlyPlannedHrsModel> GetAllMonthlyPlannedHrs(int orderId)
        {
            var data = await _projectService.GetMonthlyHrsByProjectIdAsync(orderId);

            var totalHours = 0; //await _projectService.GetProjectTotalHoursAsync(projectId);

			var plannedHours = data.Select(x => new MonthlyPlannedModel
            {
                Id = x.Id,
                OrderId = x.OrderId,
                PlannedDate = x.PlannedDate,
                Hours = x.Hours
            }).ToList();

            var result = new MonthlyPlannedHrsModel
            {
                TotalHours = totalHours,
                MonthlyPlannedListModel = plannedHours
            };

            return result;
        }

        /// <summary>
        /// Get MonthlyPlannedHrs By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<MonthlyPlannedModel> GetMonthlyPlannedHrsByIdAsync(int id)
        {
            try
            {
                var monthlyPlanned = await _projectService.GetMonthlyHrsByIdAsync(id);

                if (monthlyPlanned != null && !monthlyPlanned.IsDeleted)
                {
                    var model = _mapper.Map<MonthlyPlannedModel>(monthlyPlanned);
                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.ErrorFetchingBillingInfo);
            }
        }

        /// <summary>
        /// Insert & Update MonthlyPlannedHrs Async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateMonthlyPlannedHrsAsync(MonthlyPlannedModel model)
        {
            try
            {
                var order = await _orderService.GetOrdersById(model.OrderId);
                decimal totalHours = 0;
                var plannedHoursList = await _projectService.GetMonthlyHrsByProjectIdAsync(model.OrderId);
                foreach (var plannedHours in plannedHoursList)
                {
                    totalHours += plannedHours.Hours;
                }

                var monthlyPlanned = new MonthlyPlannedHrs();
                if (model.Id > 0)
                {
                    monthlyPlanned = await _projectService.GetMonthlyHrsByIdAsync(model.Id);

                    if (monthlyPlanned != null)
                    {
                        totalHours -= monthlyPlanned.Hours;
                        totalHours += model.Hours;
                        if (order.InHouse || (!order.InHouse && totalHours <= order.EstimatedTotalHours))
                        {
                            monthlyPlanned = _mapper.Map<MonthlyPlannedHrs>(model);
                            monthlyPlanned.ModifiedOn = DateTime.UtcNow;
                            await _projectService.UpdateMonthlyHrsAsync(monthlyPlanned);
                            return (true, ConstantValues.Success, monthlyPlanned.OrderId);
                        }
                        else
                        {
                            return (false, ConstantValues.ProjectHoursError, monthlyPlanned.OrderId);
                        }
                    }
                    else
                    {
                        return (false, ConstantValues.NoRecordFoundForUpdate, 0);
                    }
                }
                else
                {
                    totalHours += model.Hours;
                    if (order.InHouse || (!order.InHouse && totalHours <= order.EstimatedTotalHours))
                    {
                        monthlyPlanned = _mapper.Map<MonthlyPlannedHrs>(model);
                        monthlyPlanned.CreatedOn = DateTime.UtcNow;
                        monthlyPlanned.IsDeleted = false;
                        await _projectService.InsertMonthlyHrsAsync(monthlyPlanned);
                        return (true, ConstantValues.Success, monthlyPlanned.OrderId);
                    }
                    else
                    {
                        return (false, ConstantValues.ProjectHoursError, 0);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ConstantValues.ErrorFetchingMonthlyPlanned, ex);
            }
        }

        /// <summary>
        /// Delete MonthlyPlannedHrs Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteMonthlyPlannedHrsAsync(int id)
        {
            if (id > 0)
            {
                var monthlyPlanned = await _projectService.GetMonthlyHrsByIdAsync(id);
                if (monthlyPlanned != null)
                {
                    if (_workContext.CurrentCustomer == null)
                    {
                        // If user is a super admin, then only the record can be removed
                        monthlyPlanned.ModifiedBy = 1;
                    }
                    monthlyPlanned.ModifiedOn = DateTime.UtcNow;
                    monthlyPlanned.IsDeleted = true;
                    await _projectService.UpdateMonthlyHrsAsync(monthlyPlanned);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate);
            }
        }

        /// <summary>
        /// Get All MonthlyPlanned By ProjectId Async
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<IList<MonthlyPlannedModel>> GetAllMonthlyPlannedByProjectIdAsync(int orderId)
        {
            try
            {
                var monthlyList = new List<MonthlyPlannedModel>();
                var monthlyPlanned = await _projectService.GetAllMonthlyHrsByProjectIdAsync(orderId);

                if (monthlyPlanned != null)
                {
                    foreach (var item in monthlyPlanned)
                    {
                        var model = _mapper.Map<MonthlyPlannedModel>(item);
                        monthlyList.Add(model);
                    }
                }
                return monthlyList;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingMonthlyPlanned, ex);
            }
        }

        #endregion Monthly Planned Hours 

        #region GetTechStackMapping

        public async Task<List<TechStackMapping>> GetAllTechStackMapping()
        {
            var data = await _projectService.GetAllTechStackMapping();

            var result = data.Select(x => new TechStackMapping

            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                ModifiedBy = x.ModifiedBy,
            }).ToList();

            return result = data
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public async Task<IList<TechStackMapping>> GetAllTechStackMappingByProjectId(int projectid)
        {
            var techStack = await _projectService.GetAllTechStackMappingByProjectId(projectid);

            try
            {
                if (techStack != null)
                {
                    return techStack;
                }
                throw new Exception("No record found.");
            }
            catch (Exception)
            {
                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }

        //Delete TechStack
        public async Task<(bool, string)> DeleteTechStack(int projectId)
        {
            if (projectId > 0)
            {
                var techStackMappings = await _projectService.GetAllTechStackMappingByProjectId(projectId);

                if (techStackMappings != null && techStackMappings.Count > 0)
                {
                    var superAdmin = _workContext.CurrentCustomer == null;

                    foreach (var techStack in techStackMappings)
                    {
                        if (superAdmin)
                        {
                            // If user is super admin, modify as needed
                            techStack.ModifiedBy = 1;
                        }

                        techStack.ModifiedOn = DateTime.UtcNow;
                        techStack.IsDeleted = true;

                        await _projectService.UpdateTechStackMapping(techStack);
                    }

                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate);
            }
        }


        //Insert Update TechStack
        public async Task<(bool, string)> SaveTechStackMapping(techStackList model)
        {
            if (model.Id > 0)
            {
                var techStack = await _projectService.GetTechStackMappingByProjectId(model.Id);
                if (techStack != null)
                {
                    techStack.ModifiedBy = model.ModifiedBy;
                    techStack.ModifiedOn = DateTime.UtcNow;
                    techStack.IsDeleted = false;
                    await _projectService.UpdateTechStackMapping(techStack);
                    await SaveTechIds(model);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.NoRecordFoundForUpdate);
                }
            }
            else
            {
                var techStack = new TechStackMapping();
                await DeleteTechStackMapping(model.ProjectId);
                await _projectService.UpdateTechStackMapping(techStack);
                await SaveTechIds(model);
                return (true, ConstantValues.Success);
            }
        }
        public async Task<bool> SaveTechIds(techStackList model)
        {
            try
            {
                int maxTechStackCount = model.BackendTechStackId.Count > model.FrontendTechStackId.Count ? model.BackendTechStackId.Count : model.FrontendTechStackId.Count;
                for (int i = 0; i < maxTechStackCount; i++)
                {
                    var techStack = new TechStackMapping();
                    techStack.ProjectId = model.ProjectId;
                    techStack.BackendTechStackId = (model.BackendTechStackId.Count() > i) ? model.BackendTechStackId[i] : null;
                    techStack.FrontendTechStackId = (model.FrontendTechStackId.Count() > i) ? model.FrontendTechStackId[i] : null;
                    techStack.Android = model.Android;
                    techStack.IOS = model.IOS;
                    techStack.Hybrid = model.Hybrid;
                    techStack.CreatedBy = model.CreatedBy;
                    techStack.CreatedOn = DateTime.UtcNow;
                    techStack.IsDeleted = false;
                    await _projectService.InsertTechStackMapping(techStack);

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Delete Tech Stack By Id
        public async Task DeleteTechStackMapping(int id)
        {
            var allIds = await _projectService.GetAllTechStackMapping();
            var availbaleIds = allIds.Where(x => x.ProjectId == id).ToList();
            foreach (var item in availbaleIds)
            {
                item.IsDeleted = true;
                await _projectService.UpdateTechStackMapping(item);
            }
        }
        #endregion TechStack Mapping 

        #region Change Request

        /// <summary>
        /// Get ChangeRequest AttachmentId By ProjectId
        /// </summary>
        /// <param name="crId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<List<AttachmentModel>> GetCRAttachmentIdByProjectId(int crId, int projectId)
        {
            return null;
        }

        /// <summary>
        /// Get ChangeRequest ById Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<ChangeRequestResponseModel> GetChangeRequestByIdAsync(int id)
        {
            try
            {
                var result = new ChangeRequestResponseModel();
                var changeRequest = await _projectService.GetChangeRequestByIdAsync(id);

                if (changeRequest != null && !changeRequest.IsDeleted)
                {
                    result = _mapper.Map<ChangeRequestResponseModel>(changeRequest);
                    result.EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(id, changeRequest.OrderId);
                    if (result.EstTotalHours != 0)
                    {
                        result.EstTotalHours = result.EstTotalHours;
                        await _projectService.UpdateChangeRequestAsync(changeRequest);
                    }
                }
                return result;
            }
            catch (Exception)
            {

                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }

        /// <summary>
        /// Get All CR By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<IList<ChangeRequestResponseModel>> GetAllCRByProjectIdAsync(int projectId)
        {
            try
            {
                var changeRequestList = new List<ChangeRequestResponseModel>();
                var changeRequest = await _projectService.GetAllChangeRequestByProjectIdAsync(projectId);

                if (changeRequest != null)
                {
                    foreach (var item in changeRequest)
                    {
                        var model = _mapper.Map<ChangeRequestResponseModel>(item);
                        model.EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(item.Id, projectId);
                        changeRequestList.Add(model);
                    }
                }

                return changeRequestList;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingChangeRequest, ex);
            }
        }

        /// <summary>
        /// Insert & Update Change Request Async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int, int)> InsertChangeRequestAsync(ChangeRequestModel model)
        {
            var changeRequest = new ChangeRequest();
            changeRequest = _mapper.Map<ChangeRequest>(model);
            changeRequest.CreatedOn = DateTime.UtcNow;
            changeRequest.IsDeleted = false;
            await _projectService.InsertChangeRequestAsync(changeRequest);
            return (true, ConstantValues.Success, changeRequest.OrderId, changeRequest.Id);
        }

        /// <summary>
        /// Update Change Request Async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int, int)> UpdateChangeRequestAsync(ChangeRequestModel model)
        {
            var changeRequest = new ChangeRequest();
            changeRequest = await _projectService.GetChangeRequestByIdAsync(model.Id);
            if (changeRequest != null)
            {
                changeRequest = _mapper.Map<ChangeRequest>(model);
                changeRequest.EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(model.Id, model.OrderId);
                changeRequest.ModifiedOn = DateTime.UtcNow;
                await _projectService.UpdateChangeRequestAsync(changeRequest);
                return (true, ConstantValues.Success, changeRequest.OrderId, changeRequest.Id);
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate, 0, 0);
            }
        }

        /// <summary>
        /// Update Change Request Async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int, int)> UpdateChangeRequestAsync(ChangeRequestResponseModel model)
        {
            var changeRequest = new ChangeRequest();
            changeRequest = await _projectService.GetChangeRequestByIdAsync(model.Id);
            if (changeRequest != null)
            {
                changeRequest = _mapper.Map<ChangeRequest>(model);
                changeRequest.EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(model.Id, model.ProjectId);
                changeRequest.ModifiedOn = DateTime.UtcNow;
                await _projectService.UpdateChangeRequestAsync(changeRequest);
                return (true, ConstantValues.Success, changeRequest.OrderId, changeRequest.Id);
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate, 0, 0);
            }
        }

        #region Commented Code
        //public async Task<(bool, string, int, int)> InsertUpdateChangeRequestAsync(ChangeRequestModel model)
        //{

        //    var changeRequest = new ChangeRequest();
        //    if (model.Id > 0)
        //    {
        //        changeRequest = await _projectService.GetChangeRequestByIdAsync(model.Id);
        //        if (changeRequest != null)
        //        {
        //            changeRequest = _mapper.Map<ChangeRequest>(model);
        //            changeRequest.EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(model.Id, model.ProjectId);
        //            changeRequest.ModifiedOn = DateTime.UtcNow;
        //            await _projectService.UpdateChangeRequestAsync(changeRequest);
        //            return (true, ConstantValues.Success, changeRequest.ProjectId, changeRequest.Id);
        //        }
        //        else
        //        {
        //            return (false, ConstantValues.NoRecordFoundForUpdate, 0, 0);
        //        }
        //    }
        //    else
        //    {
        //        changeRequest = _mapper.Map<ChangeRequest>(model);
        //        changeRequest.CreatedOn = DateTime.UtcNow;
        //        changeRequest.IsDeleted = false;
        //        await _projectService.InsertChangeRequestAsync(changeRequest);
        //        return (true, ConstantValues.Success, changeRequest.ProjectId, changeRequest.Id);
        //    }

        //}

        #endregion Commented Code

        /// <summary>
        /// Delete CRId By ProjectId Async
        /// </summary>
        /// <param name="crId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteCRIdByProjectIdAsync(int crId, int projectId)
        {
            if (crId > 0 && projectId > 0)
            {
                // Assuming this method returns a single ChangeRequest object by its ID.
                var changeRequest = await _projectService.GetChangeRequestByIdAsync(crId);
                // Also checking if the retrieved ChangeRequest belongs to the correct projectId.
                if (changeRequest != null && changeRequest.OrderId == projectId)
                {
                    if (_workContext.CurrentCustomer == null)
                    {
                        // If the user is a super admin, then only the record can be removed
                        changeRequest.ModifiedBy = 1;
                    }
                    changeRequest.ModifiedOn = DateTime.UtcNow;
                    changeRequest.IsDeleted = true;
                    await _projectService.UpdateChangeRequestAsync(changeRequest);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }
            }
            else
            {
                return (false, ConstantValues.NoRecordFoundForUpdate);
            }
        }

        #endregion Change Request

        #region Other Info 

        /// <summary>
        /// Get OtherInfo By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<OtherInfoModel> GetOtherInfoByProjectId(int projectId)
        {
            // check if the provided project ID is valid
            if (projectId <= 0)
            {
                throw new ArgumentException(ConstantValues.InvalidProjectId);
            }

            // get total project hours
            var totalProjectHours = await _projectService.GetProjectTotalHoursAsync(projectId);

            // get project cost information
            var projectCost = await _projectService.GetProjectsById(projectId);

            // check if the project exists
            if (projectCost == null)
            {
                throw new InvalidOperationException(ConstantValues.ProjectNotFound);
            }

            // get all change request records for the project
            var crRecords = await _projectService.GetAllChangeRequestByProjectIdAsync(projectId);

            var crList = new List<ChangeRequestModel>();
            int totalCRAmt = 0;
            int totalCRHrs = 0;

            // check change request records if exist
            if (crRecords != null && crRecords.Any())
            {
                foreach (var changeRequest in crRecords)
                {
                    if (changeRequest != null)
                    {
                        // create a model for each change request
                        var crModel = new ChangeRequestModel
                        {
                            Cost = changeRequest.Cost,
                            EstTotalHours = await _projectService.GetChangeRequestTotalHoursAsync(changeRequest.Id, changeRequest.OrderId),
                        };

                        // bind amounts and hours
                        crList.Add(crModel);
                        totalCRAmt += (int)crModel.Cost;
                        totalCRHrs += crModel.EstTotalHours;
                    }
                }
            }

            // Calculate the total amount and hours of project and change request values
            var result = new OtherInfoModel
            {
                TotalAmount = (int)projectCost.ProjectCostValue + totalCRAmt,
                TotalHours = totalProjectHours + totalCRHrs,
            };

            #region Save Other Info Detail
            var otherInfo = new OtherInfo();
            otherInfo.ProjectId = projectId;
            otherInfo.TotalAmount = result.TotalAmount;
            otherInfo.TotalHours = result.TotalHours;
            otherInfo.CreatedBy = projectCost.CreatedBy;
            otherInfo.CreatedOn = DateTime.UtcNow.Date;
            await _projectService.InsertOtherInfoAsync(otherInfo);
            #endregion  Save Other Info Detail


            return result;
        }

        #endregion OtherInfo

        #region Project Managers

        /// <summary>
        /// Get All Project Managers Async
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.ProjectDetail.ProjectManagers>> GetAllProjectManagersAsync()
        {
            var res = await _projectService.GetAllProjectManagers();
            var result = _mapper.Map<List<Models.ProjectDetail.ProjectManagers>>(res);
            return result;
        }

        /// <summary>
        /// Save project manager async
        /// </summary>
        /// <returns></returns>
        public async Task SaveProjectManager(API.Models.ProjectDetail.ProjectManagers manager) =>
           await _projectService.SaveProjectManager(_mapper.Map<Tm.Core.Domain.Pms.ProjectDetail.ProjectManagers>(manager));
        public async Task SaveResources(List<ResourceDTO> resources) => await _projectService.SaveResources(_mapper.Map<List<Resource>>(resources));

        #endregion

        #region Projection
        /// <summary>
        /// Insert projection request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> InsertProjectionRequestAsync(ProjectionRequestModel model)
        {

            var projection = new Projection();
            projection = _mapper.Map<Projection>(model);
            projection.CreatedOn = DateTime.UtcNow;
            projection.CreatedBy = model.RequestedById;
            projection.IsDeleted = false;
            projection.IsActive = true;
            projection.ProjectionDate = projection.ProjectionStartDate;
            List<string> userNameList = new List<string>();

            foreach (var resourceId in model.ResourceIds)
            {
                var projectionList = await _projectService.GetProjectionDetailByDateRangeAsync(model.ProjectId, resourceId, model.ProjectionStartDate, model.ProjectionEndDate);
                if (projectionList.Count > 0)
                {
                    var ADUser = await _userRoleService.GetADUserByADUserId(resourceId);
                    if (ADUser != null)
                        userNameList.Add(ADUser?.Name);

                }
            }
            if (userNameList.Count > 0)
            {
                return (false, "For " + string.Join(", ", userNameList) + " " + ConstantValues.ProjectionDateConflictError);
            }


            foreach (var resourceId in model.ResourceIds)
            {
                var projectionList = await _projectService.GetProjectionDetailByDateRangeAsync(model.ProjectId, resourceId, model.ProjectionStartDate, model.ProjectionEndDate);
                if (projectionList.Count > 0)
                {
                    return (false, ConstantValues.ProjectionDateConflictError);
                }
                var projectionData = projection;
                projectionData.ResourceId = resourceId;
                while (projectionData.ProjectionDate.Date <= projectionData.ProjectionEndDate.Date)
                {
                    await _projectService.InsertProjectionRequestAsync(projectionData);
                    projectionData.ProjectionDate = projectionData.ProjectionDate.Date.AddDays(1);
                }
                projection.ProjectionDate = projection.ProjectionStartDate;
            }
            return (true, ConstantValues.Success);
        }

        /// <summary>
        /// Update projection request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateProjectionRequestAsync(UpdateProjectionRequestModel model)
        {
            var projection = await _projectService.GetProjectionDetailByIdAsync(model.Id);
            if (projection != null)
            {
                projection.ResourceTypeId = model.ResourceTypeId;
                projection.CostType = model.CostType;
                projection.PerHourCost = model.PerHourCost;
                projection.Hours = model.Hours;
                //projection.ProjectionDate = model.ProjectionDate;
                //projection.ProjectionStartDate = model.ProjectionStartDate;
                //projection.ProjectionEndDate = model.ProjectionEndDate;
                projection.ModifiedOn = DateTime.UtcNow;
                projection.ModifiedBy = model.RequestedById;
                await _projectService.UpdatetProjectionRequestAsync(projection);
                return (true, ConstantValues.Success);
            }
            else
            {
                return (true, ConstantValues.ProjectionError);
            }
        }

        /// <summary>
        /// Get Projection by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Projection> GetProjectionDetailByIdAsync(int id)
        {
            return await _projectService.GetProjectionDetailByIdAsync(id);
        }
        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<ProjectionResponseModel> GetAllProjectionListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0)
        {
            ProjectionResponseModel projectionResponseModel = new ProjectionResponseModel();
            projectionResponseModel.Data = new List<ProjectionDataModel>();
            var projectionList = await _projectService.GetAllProjectionListByProjectIdAsync(projectId, startDate, endDate, languageId);
            if (projectionList != null)
            {
                projectionResponseModel.Resources = new List<ResourceData>();
                var resourceGroupData = projectionList.Select(x => new ResourceData() { Name = x.Name, ResourceId = x.ResourceId })
                .GroupBy(x => x.ResourceId).ToList();
                foreach (var resource in resourceGroupData)
                {
                    var userProjectionList = projectionList.Where(x => x.UserId == resource.Key).Select(x => new ProjectionEvents()
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        ResourceTypeId = x.ResourceTypeId,
                        CostType = x.CostType,
                        PerHourCost = x.PerHourCost,
                        Hours = x.Hours,
                        ProjectionDate = x.ProjectionDate,
                        ProjectionStartDate = x.ProjectionStartDate,
                        ProjectionEndDate = x.ProjectionEndDate,
                        CreatedOn = x.CreatedOn,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        IsDeleted = x.IsDeleted,
                        IsActive = x.IsActive,
                        ADUserId = x.ADUserId,
                        UserId = x.UserId,
                        Name = x.Name,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        JobTitle = x.JobTitle

                    }).ToList();
                    if (userProjectionList.Count() > 0)
                    {
                        ProjectionDataModel ProjectionDataModel = new ProjectionDataModel();
                        var singleResourceDetail = userProjectionList.FirstOrDefault();
                        ProjectionDataModel.ResourceId = singleResourceDetail?.UserId;
                        ProjectionDataModel.Name = singleResourceDetail?.Name;
                        ProjectionDataModel.Events = new List<ProjectionEvents>();
                        ProjectionDataModel.Events.AddRange(userProjectionList);


                        projectionResponseModel.Data.Add(ProjectionDataModel);

                        ResourceData resourceData = new ResourceData();
                        resourceData.Name = singleResourceDetail?.Name;
                        resourceData.ResourceId = singleResourceDetail?.UserId;
                        projectionResponseModel.Resources.Add(resourceData);


                    }
                }
            }

            return projectionResponseModel;
        }
        /// <summary>
        /// Get Project calculation by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<List<ProjectionCalculationResponseModel>> GetProjectionCalculationByProjectIdAsync(int projectId, int languageId = 0)
        {
            return await _projectService.GetProjectionCalculationByProjectIdAsync(projectId, languageId);
        }
        #endregion

        #region Attendance
        /// <summary>
        /// Insert Attendance request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> InsertAttendanceRequestAsync(CopyProjectionToAttendanceRequestModel model)
        {

            var attendance = new Attendance();
            List<string> userNotExistInProjection = new List<string>();
            List<string> userExistInAttendanec = new List<string>();
            foreach (var resourceId in model.ResourceIds)
            {
                var projectionList = await _projectService.GetProjectionDetailByDateRangeAsync(model.ProjectId, resourceId, model.StartDate, model.EndDate);
                if (projectionList.Count <= 0)
                {
                    var ADUser = await _userRoleService.GetADUserByADUserId(resourceId);
                    if (ADUser != null)
                        userNotExistInProjection.Add(ADUser?.Name);

                }
            }
            //If no project found then return the users
            if (userNotExistInProjection.Count > 0)
            {
                return (false, "For " + string.Join(", ", userNotExistInProjection) + " " + ConstantValues.ProjectionNotFoundError);
            }

            //Check for attendance alredy added or not
            foreach (var resourceId in model.ResourceIds)
            {
                var attandanceList = await _projectService.GetAttendanceDetailByDateRangeAsync(model.ProjectId, resourceId, model.StartDate, model.EndDate);
                if (attandanceList.Count > 0)
                {
                    var ADUser = await _userRoleService.GetADUserByADUserId(resourceId);
                    if (ADUser != null)
                        userExistInAttendanec.Add(ADUser?.Name);

                }
            }
            //If no project found then return the users
            if (userExistInAttendanec.Count > 0)
            {
                return (false, "For " + string.Join(", ", userExistInAttendanec) + " " + ConstantValues.AttendanceAlreadyExistError);
            }

            var copyProjectionResult = await _projectService.CopyProjectionToAttendanceAsync(model.ProjectId, string.Join(", ", model.ResourceIds), model.RequestedById, model.StartDate, model.EndDate);
            if (copyProjectionResult != null)
            {
                if (copyProjectionResult.Success)
                {
                    return (copyProjectionResult.Success, ConstantValues.Success);
                }
                else
                {
                    return (copyProjectionResult.Success, ConstantValues.AttendanceCopyError);
                }
            }
            else
            {
                return (false, ConstantValues.AttendanceCopyError);
            }
        }

        /// <summary>
        /// Update projection request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAttendanceRequestAsync(UpdateAttendanceRequestModel model)
        {
            var project=await _projectService.GetProjectsById(model.ProjectId);
            if (Convert.ToBoolean(project?.IsAzure))
            {
                var adAttendance = await _projectService.GetADAttendanceDetailByIdAsync(model.Id);
                if (adAttendance != null)
                {
                    adAttendance.ResourceTypeId = model.ResourceTypeId;
                    adAttendance.Hours = model.Hours;
                    adAttendance.ModifiedOn = DateTime.UtcNow;
                    adAttendance.ModifiedBy = model.RequestedById;
                    await _projectService.UpdateADAttendanceRequestAsync(adAttendance);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.ProjectionError);
                }
            }
            else
            {
                var attendance = await _projectService.GetAttendanceDetailByIdAsync(model.Id);
                if (attendance != null)
                {


                    attendance.ResourceTypeId = model.ResourceTypeId;
                    attendance.Hours = model.Hours;
                    attendance.ModifiedOn = DateTime.UtcNow;
                    attendance.ModifiedBy = model.RequestedById;
                    await _projectService.UpdatetAttendanceRequestAsync(attendance);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.ProjectionError);
                }
            }
        }

        /// <summary>
        /// Get Attendance by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Attendance> GetAttendanceDetailByIdAsync(int id)
        {
            return await _projectService.GetAttendanceDetailByIdAsync(id);
        }
        /// <summary>
        /// Get Attendance list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<AttendanceResponseListModel> GetAllAttendanceListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0)
        {
            AttendanceResponseListModel attendanceResponseModel = new AttendanceResponseListModel();
            attendanceResponseModel.Data = new List<AttendanceDataModel>();
            List<AttendanceListResponseModel> projectionList = new List<AttendanceListResponseModel>();
            var projects = await _projectService.GetProjectsById(projectId);
            if (Convert.ToBoolean(projects?.IsAzure))
            {
                // Form Devops
                projectionList = await _projectService.GetAllAttendanceListByProjectIdAsync(projectId, startDate, endDate);
            }
            else
            {
                //Form Local 
                projectionList = await _projectService.GetAllADAttendanceListByProjectIdAsync(projectId, startDate, endDate);
            }
            if (projectionList != null)
            {
                attendanceResponseModel.Resources = new List<AttendanceResourceData>();
                var resourceGroupData = projectionList.Select(x => new AttendanceResourceData() { Name = x.Name, ResourceId = x.ResourceId })
                .GroupBy(x => x.ResourceId).ToList();
                foreach (var resource in resourceGroupData)
                {
                    var userAttendanceList = projectionList.Where(x => x.UserId == resource.Key).Select(x => new AttendanceEvents()
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        ProjectionId = x.ProjectionId,
                        ResourceTypeId = x.ResourceTypeId,
                        CostType = x.CostType,
                        PerHourCost = x.PerHourCost,
                        Hours = x.Hours,
                        AttendanceDate = x.AttendanceDate,
                        CreatedOn = x.CreatedOn,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        IsDeleted = x.IsDeleted,
                        IsActive = x.IsActive,
                        ADUserId = x.ADUserId,
                        UserId = x.UserId,
                        Name = x.Name,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        JobTitle = x.JobTitle
                    }).ToList();
                    if (userAttendanceList.Count() > 0)
                    {
                        AttendanceDataModel attendanceDataModel = new AttendanceDataModel();
                        var singleResourceDetail = userAttendanceList.FirstOrDefault();
                        attendanceDataModel.ResourceId = singleResourceDetail?.UserId;
                        attendanceDataModel.Name = singleResourceDetail?.Name;
                        attendanceDataModel.Events = new List<AttendanceEvents>();
                        attendanceDataModel.Events.AddRange(userAttendanceList);

                        //Add Event & resourse Data in list
                        attendanceResponseModel.Data.Add(attendanceDataModel);
                        //Add only resource List
                        AttendanceResourceData resourceData = new AttendanceResourceData();
                        resourceData.Name = singleResourceDetail?.Name;
                        resourceData.ResourceId = singleResourceDetail?.UserId;
                        attendanceResponseModel.Resources.Add(resourceData);
                    }
                }
            }

            return attendanceResponseModel;
        }
        /// <summary>
        /// Get Users fro Attendance from Projection data
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async Task<List<AttendenceUserList>> GetProjectionResourcesForAttendanceIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _projectService.GetProjectionResourcesForAttendanceIdAsync(projectId, startDate, endDate);
        }
        #endregion

        #region AdAttendance

        /// <summary>
        /// Insert & Update General Detail
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> UpsertAllADTimeLogs()
        {
            string savedProjectName = string.Empty;
            var allProjects = await _projectService.GetAllActiveDevopsProjectsForTimeLogs();

            foreach (var project in allProjects)
            {
                try
                {
                    string personalAccessToken = _adConfig.PersonalAccessToken;
                    string endPoint = _adConfig.TimeLogAPIUrl;
                    string organisationId = _adConfig.OrganisationId;
                    string timeLogClientCode = _adConfig.TimeLogClientCode;
                    int timeLogsFromPreviousDays = _adConfig.TimeLogsFromPreviousDays;
                    DateTime fromDate = DateTime.Now.AddDays(-timeLogsFromPreviousDays);
                    //Replace OrganisationId
                    endPoint = endPoint?.Replace("#OrgId#", organisationId);

                    string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));
                    string parameterForTimeLog = String.Concat("?code=", timeLogClientCode, "&createdOnFromDate=", fromDate.ToString("yyyy-MM-dd"), "&updatedOnFromDate=", fromDate.ToString("yyyy-MM-dd"), "&projectId=", project.Azure_ProjectId);
                    string finalTimeLogAPIUrl = String.Concat(endPoint, parameterForTimeLog);
                    //API call
                    string jsonResponse = "";
                    //use the httpclient
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(endPoint);  //url of your organization
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        //connect to the REST endpoint            
                        HttpResponseMessage response = client.GetAsync(finalTimeLogAPIUrl).Result;

                        //check to see if we have a successful response
                        if (response.IsSuccessStatusCode)
                        {
                            //set the viewmodel from the content in the response
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                jsonResponse = await response.Content.ReadAsStringAsync();
                            }
                            if (!string.IsNullOrEmpty(jsonResponse))
                            {
                                //var projectTimeLogData = JsonSerializer.Deserialize<List<ADTimeLogModels>>(jsonResponse);
                                var adTimeLogResponseList = JsonSerializer.Deserialize<List<ADTimeLogModels>>(jsonResponse);
                                if (adTimeLogResponseList != null)
                                {
                                    var adTimeLogList = adTimeLogResponseList.Select(x => new ADTimeLog()
                                    {
                                        TimeLogId = x.timeLogId,
                                        Comment = x.comment,
                                        Week = x.week,
                                        TimeTypeId = x.timeTypeId,
                                        TimeTypeDescription = x.timeTypeDescription,
                                        Minutes = x.minutes,
                                        Date = x.date,
                                        UserId = x.userId,
                                        UserName = x.userName,
                                        ProjectId = project.Id,
                                        WorkItemId = x.workItemId
                                    }).ToList();
                                    await _projectService.UpsertADAttendanceDataAsync(adTimeLogList);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return (true, ConstantValues.Success);
        }
        #endregion
        #endregion Methods
    }
}

