using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core;
using Tm.Core.Configuration;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Services.Customers;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.ProjectDetail;

namespace API.Factories.ChangeRequestDetail
{
	public class ChangeRequestFactory : IChangeRequestFactory
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
        private readonly ICustomersService _iCustomersService;
        private static IHttpContextAccessor HttpContextAccessor;
        private readonly IEncryptDecryptService _encryptDecryptService;
        #endregion

        #region Ctor
        public ChangeRequestFactory(IProjectsService projectService,
            IMasterDataService masterDataService,
            IWorkContext workContext,
            ICustomerService customerService,
            IMapper mapper,
            TmConfig tmConfig,
            IWebHostEnvironment env,
            IAttachmentsService attachmentService, IOrderService orderService,
            ICustomersService iCustomersService,
            IHttpContextAccessor httpContextAccessor,
            IEncryptDecryptService encryptDecryptService)
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
            _iCustomersService = iCustomersService;
            HttpContextAccessor = httpContextAccessor;
            _encryptDecryptService = encryptDecryptService;
        }
        #endregion

        #region Methods
        #region ChangeRequest Listing 

        /// <summary>
        /// Get All chnage request by project ID
        /// </summary>
        /// <returns></returns>
        public async Task<List<ChangeRequestResponseModel>> GetAllChangeRequestByProjectAsync(int projectId)
        {
            try
            {
                List<ChangeRequestResponseModel> result = new List<ChangeRequestResponseModel>();

                var data = await _projectService.GetAllChangeRequestByProjectIdAsync(projectId);

                foreach (var changeRequest in data)
                {
                    ChangeRequestResponseModel model = new ChangeRequestResponseModel();
                    model.Id = changeRequest.Id;
                    model.ProjectId = changeRequest.ProjectId;
                    model.OrderId = changeRequest.OrderId;
                    model.CRName = changeRequest.CRName;
                    model.EstimatedDuration = changeRequest.EstimatedDuration;
                    model.EstimatedEfforts = changeRequest.EstimatedEfforts;
                    model.Cost = changeRequest.Cost;
                    model.EstTotalHours = changeRequest.EstTotalHours;
                    model.PlannedStartDate = Convert.ToString(changeRequest.PlannedStartDate);
                    model.PlannedEndDate = Convert.ToString(changeRequest.PlannedEndDate);
                    model.ActualStartDate = Convert.ToString(changeRequest.ActualStartDate);
                    model.ActualEndDate = Convert.ToString(changeRequest.ActualEndDate);
                    model.DelayReasonStartDate = changeRequest.DelayReasonStartDate;
                    model.DelayReasonEndDate = changeRequest.DelayReasonEndDate;
                    //No need to add attachment
                    //var attachments = await _attachmentService.GetChangeRequestAttachmentsByCRId(changeRequest.Id);
                    //if (attachments != null)
                    //{
                    //    foreach (var attch in attachments)
                    //    {
                    //        var docAttach = await _attachmentService.GetAttachmentById(attch.AttachmentId);
                    //        if (docAttach != null)
                    //        {
                    //            //model.CRAttachmentData.Add(docAttach);
                    //        }
                    //    }
                    //}
                    if (changeRequest.OrderId > 0)
                    {
                        model.Order = await OrderDataMapping(changeRequest.OrderId);
                    }
                    result.Add(model);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ConstantValues.ErrorFetchingProjectData, ex);
            }
        }

        #endregion Project Listing 


        public async Task<ChangeRequestResponseModel> GetAllChangeRequestByIdAsync(int id)
        {
            try
            {
                ChangeRequestResponseModel changeRequestResponseModel = new ChangeRequestResponseModel();
                var changeRequest = await _projectService.GetChangeRequestByIdAsync(id);
                if (changeRequest != null)
                {
                    changeRequestResponseModel.Id = changeRequest.Id;
                    changeRequestResponseModel.ProjectId = changeRequest.ProjectId;
                    changeRequestResponseModel.OrderId = changeRequest.OrderId;
                    changeRequestResponseModel.CRName = changeRequest.CRName;
                    changeRequestResponseModel.EstimatedDuration = changeRequest.EstimatedDuration;
                    changeRequestResponseModel.EstimatedEfforts = changeRequest.EstimatedEfforts;
                    changeRequestResponseModel.Cost = changeRequest.Cost;
                    changeRequestResponseModel.EstTotalHours = changeRequest.EstTotalHours;
                    changeRequestResponseModel.PlannedStartDate = Convert.ToString(changeRequest.PlannedStartDate);
                    changeRequestResponseModel.PlannedEndDate = Convert.ToString(changeRequest.PlannedEndDate);
                    changeRequestResponseModel.ActualStartDate = Convert.ToString(changeRequest.ActualStartDate);
                    changeRequestResponseModel.ActualEndDate = Convert.ToString(changeRequest.ActualEndDate);
                    changeRequestResponseModel.DelayReasonStartDate = changeRequest.DelayReasonStartDate;
                    changeRequestResponseModel.DelayReasonEndDate = changeRequest.DelayReasonEndDate;
                    if (changeRequest.OrderId > 0)
                    {
                        OrderDetailResponseModel result = new OrderDetailResponseModel();
                        changeRequestResponseModel.Order = await OrderDataMapping(changeRequest.OrderId);
                    }
                }
                return changeRequestResponseModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<OrderDetailResponseModel> OrderDataMapping(int OrderId)
        {
            OrderDetailResponseModel result = new OrderDetailResponseModel();
            var orderDetail = await _orderService.GetOrdersById(OrderId);
            if (orderDetail != null)
            {
                result.Id = orderDetail.Id;
                result.OrderNumber = orderDetail.OrderNumber;
                result.OrderName = orderDetail.OrderName;
                result.SowDocType = orderDetail.SOWDocumentId;
                var sowDocType = await _masterDataService.GetProjectTypeById(orderDetail.SOWDocumentId);
                result.SowDocTypeName = sowDocType != null ? sowDocType.Name : null;
                result.EstimatedEfforts = orderDetail.EstimatedEfforts;
                result.EstimatedHours = orderDetail.EstimatedTotalHours;
                result.HourlyCost = orderDetail.EstimatedHourlyCost;
                result.EstimatedHours = orderDetail.EstimatedTotalHours;
                result.OrderCost = _encryptDecryptService.DecryptString(orderDetail.OrderCost);
                if (orderDetail.CurrencyId != null)
                {
                    var currencyDetail = _masterDataService.GetCurrencyById(Convert.ToInt32(orderDetail.CurrencyId));
                    if (currencyDetail != null)
                    {
                        result.CurrencyIcon = string.Concat(HttpContextAccessor.HttpContext.Request.Scheme, "://", HttpContextAccessor.HttpContext.Request.Host,  currencyDetail.Icon);
                    }
                }
                var clientDetail = await _iCustomersService.GetCustomerById(orderDetail.CustomerId);
                if (clientDetail != null)
                {
                    result.ClientName = clientDetail.Name;
                    result.Company = clientDetail.Company;
                    result.ClientEmail = clientDetail.Email;
                    result.ClientPhoneNumber = clientDetail.PhoneNo;
                    result.ContactPersonName = clientDetail.ContactPersonName;
                    result.ContactPersonEmail = clientDetail.ContactEmail;
                    result.ContactPersonPhoneNumber = clientDetail.ContactPhoneNo;
                    result.Address = clientDetail.Address;
                }
                // Get Order Attachments
                var orderAttachments = await _attachmentService.GetOrderAttachmentByOrderId(orderDetail.Id, true, false);
                if (orderAttachments.FirstOrDefault() != null)
                {
                    var AttachmentObject = orderAttachments.FirstOrDefault();
                    if (AttachmentObject != null)
                    {
                        result.OrderAttachments =await GetAttachment(AttachmentObject.AttachmentId);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Get Image from attachment table By AttachmentID 
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        private async Task<Attachments> GetAttachment(int attachmentId)
        {
            Attachments attachments = new Attachments();
            attachments = await _attachmentService.GetAttachmentById(attachmentId);
            if (attachments != null)
            {
                string contentDirectory = "Content"; // The directory you want to extract

                int contentIndex = attachments.FilePath.IndexOf(contentDirectory);
                if (contentIndex != -1)
                {
                    string relativePath = attachments.FilePath.Substring(contentIndex);

                    // Replace backslashes with forward slashes in the relative path
                    attachments.FilePath = string.Concat(HttpContextAccessor.HttpContext.Request.Scheme, "://", HttpContextAccessor.HttpContext.Request.Host, "\\", relativePath).Replace("\\", "/"); ;
                }
            }
            return attachments;
        }


        /// <summary>
        /// Insert & Update Chnage request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> InsertUpdateChangeRequest(ChangeRequestModel model)
        {
            ChangeRequest changeRequest = new ChangeRequest();
            changeRequest.ProjectId = model.ProjectId;
            changeRequest.OrderId = model.OrderId;
            changeRequest.CRName = model.CRName;
            changeRequest.EstimatedDuration = model.EstimatedDuration;
            changeRequest.EstimatedEfforts = model.EstimatedEfforts;
            changeRequest.Cost = model.Cost;
            changeRequest.EstTotalHours = model.EstTotalHours;
            changeRequest.PlannedStartDate = model.PlannedStartDate;
            changeRequest.PlannedEndDate = model.PlannedEndDate;
            changeRequest.ActualStartDate = model.ActualStartDate;
            changeRequest.ActualEndDate = model.ActualEndDate;
            changeRequest.DelayReasonStartDate = model.DelayReasonStartDate;
            changeRequest.DelayReasonEndDate = model.DelayReasonEndDate;
            changeRequest.Attachment = model.Attachment;
            changeRequest.CreatedBy = model.CreatedBy;
            changeRequest.CreatedOn = model.CreatedOn;

            if (model.Id > 0)
            {

                //generalDetail.ModifiedBy = _workContext.CurrentCustomer.Id;
                changeRequest.Id = model.Id;
                changeRequest.ModifiedBy = model.ModifiedBy;
                changeRequest.ModifiedOn = DateTime.UtcNow;
                await _projectService.UpdateChangeRequestAsync(changeRequest);
            }
            else
            {
                //generalDetail.CreatedBy = _workContext.CurrentCustomer.Id;
                changeRequest.CreatedBy = model.CreatedBy;
                changeRequest.CreatedOn = DateTime.UtcNow;
                changeRequest.IsDeleted = false;
                await _projectService.InsertChangeRequestAsync(changeRequest);
            }

            //if (model.AttachFiles != null)
            //{
            //    var path = _tmConfig.CRAttachmentFolderPath;
            //    string filePath = string.Concat(_env.ContentRootPath, path);
            //    foreach (var fileData in model.AttachFiles)
            //    {
            //        if (fileData != null)
            //        {
            //            int lastIndex = fileData.FileName.LastIndexOf('.');

            //            // Create the full file path where you want to save the image
            //            string fullPath = Path.Combine(filePath, fileData.FileName);

            //            //create folder if not exist
            //            if (!Directory.Exists(path))
            //                Directory.CreateDirectory(path);
            //            // Check if the file already exists and delete it if needed
            //            if (File.Exists(fullPath))
            //            {
            //                File.Delete(fullPath);
            //            }
            //            try
            //            {
            //                //get file extension
            //                FileInfo fileInfo = new FileInfo(fileData.FileName);
            //                string fileName = fileData.FileName + fileInfo.Extension;

            //                string fileNameWithPath = Path.Combine(path, fileName);

            //                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            //                {
            //                    fileData.CopyTo(stream);
            //                }

            //            }
            //            catch (Exception ex)
            //            {
            //                // Log the exception message for debugging
            //                Console.WriteLine($"Image.Save Error: {ex.Message}");
            //                // Handle the exception or rethrow it as needed
            //            }

            //            // Create and insert an Attachment object with the newFileName
            //            var attachment = new Attachments();
            //            attachment.FilePath = fullPath;
            //            attachment.FileName = fileData.FileName; // Use the newFileName here
            //            attachment.CreatedBy = model.CreatedBy;
            //            attachment.ModifiedBy = model.ModifiedBy;
            //            attachment.CreatedOn = DateTime.UtcNow;
            //            attachment.ModifiedOn = DateTime.UtcNow;
            //            attachment.ModifiedBy = model.ModifiedBy;
            //            await _attachmentService.InsertAttachment(attachment);

            //            // Create and insert a CRAttachment object
            //            var CRAttachment = new ChangeRequestAttachment();
            //            CRAttachment.CRId = changeRequest.Id;
            //            CRAttachment.AttachmentId = attachment.Id;
            //            CRAttachment.CreatedOn = DateTime.UtcNow;
            //            CRAttachment.ModifiedOn = DateTime.UtcNow;
            //            CRAttachment.CreatedBy = model.CreatedBy;
            //            CRAttachment.ModifiedBy = model.ModifiedBy;
            //            CRAttachment.IsDeleted = false;
            //            await _attachmentService.InsertChangeRequestAttachment(CRAttachment);
            //        }
            //    }
            //}
            return (true, ConstantValues.Success, changeRequest.Id);
        }

        #endregion
    }
}
