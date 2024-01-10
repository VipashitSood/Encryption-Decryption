using API.Models.GeneralDetail;
using API.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core;
using Tm.Core.Configuration;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Services.Customers;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.GeneralDetail;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.UserRole;

namespace API.Factories.GeneralDetail
{
    public class GeneralDetailModelFactory : IGeneralDetailModelFactory
    {
        #region Fields
        private readonly IGeneralDetailService _generalDetailService;
        private IMasterDataService _masterDataService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IUserRoleService _userRoleService;
        private readonly ICustomersService _iCustomersService;
        private readonly IMasterDataService _iMasterDataService;
        private readonly IAttachmentsService _attachmentService;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptDecryptService _encryptDecryptService;
        #endregion

        #region Ctor
        public GeneralDetailModelFactory(IGeneralDetailService generalDetailService,
            IMasterDataService masterDataService,
            IWorkContext workContext,
            ICustomerService customerService,
            IOrderService orderService,
            IUserRoleService userRoleService,
            ICustomersService iCustomersService,
            IMasterDataService iMasterDataService,
            IAttachmentsService attachmentService,
            IHttpContextAccessor httpContextAccessor,
            IEncryptDecryptService encryptDecryptService)
        {
            _generalDetailService = generalDetailService;
            _masterDataService = masterDataService;
            _workContext = workContext;
            _customerService = customerService;
            _orderService = orderService;
            _userRoleService = userRoleService;
            _iMasterDataService = iMasterDataService;
            _iCustomersService = iCustomersService;
            _attachmentService = attachmentService;
            _httpContextAccessor = httpContextAccessor;
            _encryptDecryptService = encryptDecryptService;
        }

        #endregion

        #region Utilities
        private string GetCustomerUsername(int customerId)
        {
            try
            {
                var customer = _customerService.GetCustomerById(customerId);
                return customer?.Username ?? "Unknown User";
            }
            catch (Exception ex)
            {
                //get unknow user if user isn't register
                return "Unknown User";
            }
        }
        #endregion Utilities

        /// <summary>
        /// Get Dropdown list of Orders, Managers, Project Status
        /// </summary>
        /// <returns></returns>
        public async Task<DropDownModel> GetAllDropDownLists()
        {
            var model = new DropDownModel();
            var ordersData = await _orderService.GetAllOrders();

            List<OrderResponseModel> orderList = ordersData.Select(x => new OrderResponseModel
            {
                Value = Convert.ToString(x.Id),
                Label = x.OrderName
            }).ToList();

            //orderList.OrderByDescending(x => x.OrderName).ToList();
            model.OrderNameList = orderList;


            var projectManagers = await _userRoleService.GetAllProjectManagers();
            if (projectManagers != null)
            {
                List<ProjectManagerResponseModel> projectManagersList = projectManagers.Select(x => new ProjectManagerResponseModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    ManagerName = x.DisplayName
                }).ToList();

                projectManagersList.OrderByDescending(x => x.ManagerName).ToList();
                model.ProjectManagersList = projectManagersList;
            }

            var projectStatusData = await _masterDataService.GetAllProjectStatusWithoutPaging();
            if (projectStatusData != null)
            {
                List<MasterDataResponseModel> projectStatusList = projectStatusData.Select(x => new MasterDataResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                projectStatusList.OrderByDescending(x => x.Name).ToList();

                model.ProjectStatusList = projectStatusList;
            }
            return model;
        }

        /// <summary>
        /// Get Order Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDetailResponseModel> GetOrderDetailById(int id)
        {
            var result = new OrderDetailResponseModel();
            var orderDetail = await _orderService.GetOrdersById(id);

            //Get Order attachments (TO DO)

            //var clientDetail = await _generalDetailService.GetClientDetailByOrderId(id);
            if (orderDetail != null)
            {
                result.Id = orderDetail.Id;
                result.OrderNumber = orderDetail.OrderNumber;
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
                    var currencyDetail = _iMasterDataService.GetCurrencyById(Convert.ToInt32(orderDetail.CurrencyId));
                    if (currencyDetail != null)
                    {
                        result.CurrencyIcon = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", currencyDetail.Icon).Replace("\\", "/");
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
                        result.OrderAttachments = await GetAttachment(AttachmentObject.AttachmentId);
                    }
                }
            }
            return result;
        }
        /// <summary>7
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
                    attachments.FileName = attachments.FileName;
                    string relativePath = attachments.FilePath.Substring(contentIndex);
                    // Replace backslashes with forward slashes in the relative path
                    attachments.FilePath = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", relativePath).Replace("\\", "/");
                }
            }
            return attachments;
        }

        /// <summary>
        /// Get Project ManagerDetail By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ProjectManagerDetailResponseModel> GetProjectManagerDetailById(string userId)
        {
            var result = new ProjectManagerDetailResponseModel();
            var projectManagerDetail = await _userRoleService.GetADUserByADUserId(userId);
            if (projectManagerDetail != null)
            {
                result.Id = projectManagerDetail.Id;
                result.Email = projectManagerDetail.Email;
                result.PhoneNumber = projectManagerDetail.Mobile;
            }
            return result;
        }
        /// <summary>
        /// Get project manager by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProjectManagerDetailResponseModel> GetProjectManagerDetailById(int id)
        {
            var result = new ProjectManagerDetailResponseModel();
            var projectManagerDetail = await _userRoleService.GetADUserById(id);
            if (projectManagerDetail != null)
            {
                result.Id = projectManagerDetail.Id;
                result.Name = projectManagerDetail.Name;
                result.Email = projectManagerDetail.Email;
                result.PhoneNumber = projectManagerDetail.Mobile;
            }

            return result;
        }
    }
}