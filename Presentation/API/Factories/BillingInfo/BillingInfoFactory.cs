using API.Models.BillingInformation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Data;
using Tm.Services.Pms.BillingInformations;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.PmsPOInformation;

namespace API.Factories.BillingInformations
{
    public class BillingInfoFactory : IBillingInfoFactory
    {
        #region Fields
        private readonly IBillingInfoService _billingInfoService;
        private readonly IOrderService _orderService;
        private readonly IAttachmentsService _attachmentsService;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private readonly IPOInfoService _pOInfoService;
        private readonly IMasterDataService _masterDataService;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptDecryptService _encryptDecryptService;
        private readonly ICustomersService _customerService;
        #endregion

        #region Ctor
        public BillingInfoFactory(IBillingInfoService billingInfoService,
            IOrderService orderService,
             IAttachmentsService attachmentsService,
             TmConfig tmConfig,
             IWebHostEnvironment env,
             IPOInfoService pOInfoService,
             IMasterDataService masterDataService,
             IHttpContextAccessor httpContextAccessor,
             IEncryptDecryptService encryptDecryptService,
             ICustomersService customerService)
        {
            _billingInfoService = billingInfoService;
            _orderService = orderService;
            _attachmentsService = attachmentsService;
            _tmConfig = tmConfig;
            _env = env;
            _pOInfoService = pOInfoService;
            _masterDataService = masterDataService;
            _httpContextAccessor = httpContextAccessor;
            _encryptDecryptService = encryptDecryptService;
            _customerService= customerService;

        }
        #endregion

        #region Utilities

        private Image ConvertBase64ToImage(string base64String)
        {
            try
            {
                // Convert the Base64 string to a byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Create a memory stream from the byte array
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    // Attempt to create an Image object from the memory stream
                    try
                    {
                        Image image = Image.FromStream(ms);
                        return image;
                    }
                    catch (Exception exImage)
                    {
                        // Log the specific image-related exception
                        Console.WriteLine($"Error creating Image object: {exImage.Message}");
                        return null; // Return null to indicate a problem with image creation
                    }
                }
            }
            catch (Exception exBase64)
            {
                // Log the Base64 conversion exception
                Console.WriteLine($"Base64 to Image conversion error: {exBase64.Message}");
                return null; // Return null to indicate a problem with Base64 conversion
            }
        }

        private async Task<IEnumerable<string>> GetPONumbersForBillingInfo(int billingInfoId)
        {
            var POInfoList = await _billingInfoService.GetBillingInfoPOMappingByBillingInfoId(billingInfoId);
            var allPONumbers = new List<string>(); // Create a list to store all PONumbers

            foreach (var item in POInfoList)
            {
                var poInfo = await _pOInfoService.GetPOInfoById(item.POId);
                allPONumbers.AddRange(poInfo.PONumber.Split(',').Select(name => name.Trim()));
            }

            return allPONumbers;
        }
        #endregion Utilities

        #region Methods
        /// <summary>
        /// Create Billing Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> CreateBillingInformation(BillingInformationModel model)
        {
            try
            {
                // Get order and associated customer information
                var order = await _orderService.GetOrdersById(model.OrderId);
                var customer = _customerService.GetCustomerById(order.CustomerId);

                var billingInfoList = await _billingInfoService.GetAllBillingInformation(model.CustomerId);
                decimal total = 0;

                foreach (var billingInfo in billingInfoList)
                {
                    total += billingInfo.ProjectCost;
                }

                total += model.ProjectCost;

                if (order.InHouse || (!order.InHouse && total <= _encryptDecryptService.DecryptString(order.OrderCost)))
                {
                    BillingInformation billingInformation = new BillingInformation();
                    billingInformation.ProjectId = model.ProjectId;
                    billingInformation.OrderId = model.OrderId;
                    billingInformation.MilestoneName = model.MilestoneName;
                    billingInformation.Deliverables = model.Deliverables;
                    billingInformation.ProjectCost = model.ProjectCost;
                    billingInformation.ProjectedHours = model.ProjectedHours;
                    billingInformation.TimePeriod = model.TimePeriod;
                    billingInformation.ToBeRaised = model.ToBeRaised;
                    billingInformation.ManagerAction = model.ManagerAction;
                    billingInformation.DHAction = model.DHAction;
                    billingInformation.AHAction = model.AHAction;
                    billingInformation.RaiseDate = null;
                    billingInformation.CurrencyId = model.CurrencyId;
                    billingInformation.CreatedOn = DateTime.UtcNow;
                    billingInformation.CreatedBy = model.CreatedBy;
                    billingInformation.ActualBilling = model.ActualBilling;
                    // Set CustomerId
                    billingInformation.CustomerId = model.CustomerId;

                    await _billingInfoService.InsertBillingInformation(billingInformation);

                    if (billingInformation != null)
                    {
                        if (model.BillingInfoOrderPOList.Count > 0)
                        {
                            foreach (var item in model.BillingInfoOrderPOList)
                            {
                                if (!string.IsNullOrEmpty(item.Text))
                                {
                                    BillingInfoPOMapping billingInfoPOMapping = new BillingInfoPOMapping();
                                    billingInfoPOMapping.BillingInfoId = billingInformation.Id;
                                    billingInfoPOMapping.POId = item.Value;
                                    billingInfoPOMapping.POValue = item.POValue;
                                    billingInfoPOMapping.CreatedOn = DateTime.UtcNow;
                                    billingInfoPOMapping.CreatedBy = model.CreatedBy;
                                    await _billingInfoService.InsertBillingInfoPOMapping(billingInfoPOMapping);

                                    var path = _tmConfig.AttachmentFolderPath;
                                    string filePath = string.Concat(_env.ContentRootPath, path);
                                    if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.Base64))
                                    {
                                        int lastIndex = item.FileName.LastIndexOf('.');

                                        // Create the full file path where you want to save the image
                                        string fullPath = Path.Combine(filePath, item.FileName);
                                        // Check if the file already exists and delete it if needed
                                        if (File.Exists(fullPath))
                                        {
                                            File.Delete(fullPath);
                                        }
                                        try
                                        {
                                            // Convert Base64 to Image
                                            using (var image = ConvertBase64ToImage(item.Base64))
                                            {
                                                // Save the image to the specified file path
                                                image.Save(fullPath);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Log the exception message for debugging
                                            Console.WriteLine($"Image.Save Error: {ex.Message}");
                                            // Handle the exception or rethrow it as needed
                                        }

                                        // Create and insert an Attachment object with the newFileName
                                        var attachment = new Attachments();
                                        attachment.FilePath = fullPath;
                                        attachment.FileName = item.FileName; // Use the newFileName here
                                        attachment.CreatedBy = model.CreatedBy;
                                        attachment.ModifiedBy = model.ModifiedBy;
                                        attachment.CreatedOn = DateTime.UtcNow;
                                        attachment.ModifiedOn = DateTime.UtcNow;
                                        attachment.ModifiedBy = model.ModifiedBy;
                                        await _attachmentsService.InsertAttachment(attachment);

                                        // Create and insert a CustomerAttachment object
                                        var poInfoAttachment = new POInfoAttachment();
                                        poInfoAttachment.POInfoId = item.Value;
                                        poInfoAttachment.AttachmentId = attachment.Id;
                                        poInfoAttachment.CreatedOn = DateTime.UtcNow;
                                        poInfoAttachment.ModifiedOn = DateTime.UtcNow;
                                        poInfoAttachment.CreatedBy = model.CreatedBy;
                                        poInfoAttachment.ModifiedBy = model.ModifiedBy;
                                        poInfoAttachment.IsDeleted = false;
                                        await _attachmentsService.InsertPOInfoAttachment(poInfoAttachment);
                                    }
                                }
                            }
                        }
                    }

                    return (true, ConstantValues.Success, billingInformation.Id);
                }
                else
                {
                    return (false, ConstantValues.ProjectCostError, 0);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        /// <summary>
        /// Update Billing Information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> UpdateBillingInformation(UpdateBillingInformationModel model)
        {
            try
            {
                // Check if the provided model has a valid Po Information ID
                if (model.Id <= 0)
                {
                    return (false, "Invalid Billing Information ID", 0);
                }

                //Get order details by id
                var order = await _orderService.GetOrdersById(model.OrderId);

                //Get Billing info by order id
                var billingInfoList = await _billingInfoService.GetAllBillingInformation(model.CustomerId);
                decimal total = 0;
                foreach (var billingInfo in billingInfoList)
                {
                    total += billingInfo.ProjectCost;
                }

                //Billing info by id
                var billingInformation = await _billingInfoService.GetBillingInformationById(model.Id);

                total -= billingInformation.ProjectCost;
                total += model.ProjectCost;

                if (order.InHouse || (!order.InHouse && total <= _encryptDecryptService.DecryptString(order.OrderCost)))
                {
                    billingInformation.ProjectId = model.ProjectId;
                    billingInformation.Id = model.Id;
                    billingInformation.ProjectCost = model.ProjectCost;
                    billingInformation.ProjectedHours = model.ProjectedHours;
                    billingInformation.OrderId = model.OrderId;
                    billingInformation.MilestoneName = model.MilestoneName;
                    billingInformation.Deliverables = model.Deliverables;
                    billingInformation.TimePeriod = model.TimePeriod;
                    billingInformation.ToBeRaised = model.ToBeRaised;
                    billingInformation.ManagerAction = model.ManagerAction;
                    billingInformation.DHAction = model.DHAction;
                    billingInformation.AHAction = model.AHAction;
                    billingInformation.RaiseDate = null;
                    billingInformation.CurrencyId = model.CurrencyId;
                    billingInformation.CreatedOn = DateTime.UtcNow;
                    billingInformation.CreatedBy = model.CreatedBy;
                    billingInformation.ActualBilling = model.ActualBilling;
                    billingInformation.CustomerId = model.CustomerId;

                    await _billingInfoService.UpdateBillingInformation(billingInformation);
                    if (billingInformation != null)
                    {

                        if (model.BillingInfoOrderPOList.Count > 0)
                        {
                            foreach (var item in model.BillingInfoOrderPOList)
                            {
                                var po = await _billingInfoService.GetPONumberByPOValue(item.Text);
                                var billingInfoPOMapping = await _billingInfoService.GetBillingInfoPOMappingBybillingInfoandPoId(item.BillingInfoId, po.Id);
                                if (billingInfoPOMapping != null)
                                {
                                    billingInfoPOMapping.BillingInfoId = billingInformation.Id;
                                    billingInfoPOMapping.POId = item.Value;
                                    billingInfoPOMapping.POValue = item.POValue;
                                    billingInfoPOMapping.ModifiedOn = DateTime.UtcNow;
                                    billingInfoPOMapping.ModifiedBy = model.CreatedBy;
                                    await _billingInfoService.UpdateBillingInfoPOMapping(billingInfoPOMapping);

                                    //Update POAmount 
                                    po.PORemainingAmount = po.POAmount - item.POValue;
                                    await _pOInfoService.UpdatePOInfo(po);
                                }
                                else
                                {

                                    BillingInfoPOMapping bInfoPOMapping = new BillingInfoPOMapping();
                                    bInfoPOMapping.BillingInfoId = billingInformation.Id;
                                    bInfoPOMapping.POId = item.Value;
                                    bInfoPOMapping.POValue = item.POValue;
                                    bInfoPOMapping.CreatedOn = DateTime.UtcNow;
                                    bInfoPOMapping.CreatedBy = model.CreatedBy;
                                    await _billingInfoService.InsertBillingInfoPOMapping(bInfoPOMapping);

                                    po.PORemainingAmount = item.POAmountRemaining - item.POValue;
                                    await _pOInfoService.UpdatePOInfo(po);
                                }
                                var path = _tmConfig.AttachmentFolderPath;
                                string filePath = string.Concat(_env.ContentRootPath, path);
                                if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.Base64))
                                {
                                    int lastIndex = item.FileName.LastIndexOf('.');

                                    // Create the full file path where you want to save the image
                                    string fullPath = Path.Combine(filePath, item.FileName);
                                    // Check if the file already exists and delete it if needed
                                    if (File.Exists(fullPath))
                                    {
                                        File.Delete(fullPath);
                                    }
                                    try
                                    {
                                        // Convert Base64 to Image
                                        using (var image = ConvertBase64ToImage(item.Base64))
                                        {
                                            // Save the image to the specified file path
                                            image.Save(fullPath);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log the exception message for debugging
                                        Console.WriteLine($"Image.Save Error: {ex.Message}");
                                        // Handle the exception or rethrow it as needed
                                    }

                                    // Create and insert an Attachment object with the newFileName
                                    var attachment = new Attachments();
                                    attachment.FilePath = fullPath;
                                    attachment.FileName = item.FileName; // Use the newFileName here
                                    attachment.CreatedBy = model.CreatedBy;
                                    attachment.ModifiedBy = model.ModifiedBy;
                                    attachment.CreatedOn = DateTime.UtcNow;
                                    attachment.ModifiedOn = DateTime.UtcNow;
                                    attachment.ModifiedBy = model.ModifiedBy;
                                    await _attachmentsService.InsertAttachment(attachment);

                                    var existingPoInfoAttachment = (await _attachmentsService.GetPOInforAttachmentByPOInfoId(item.Value)).LastOrDefault();
                                    if (existingPoInfoAttachment != null)
                                    {
                                        existingPoInfoAttachment.POInfoId = item.Value;
                                        existingPoInfoAttachment.AttachmentId = attachment.Id;
                                        existingPoInfoAttachment.CreatedOn = DateTime.UtcNow;
                                        existingPoInfoAttachment.ModifiedOn = DateTime.UtcNow;
                                        existingPoInfoAttachment.CreatedBy = model.CreatedBy;
                                        existingPoInfoAttachment.ModifiedBy = model.ModifiedBy;
                                        existingPoInfoAttachment.IsDeleted = false;
                                        await _attachmentsService.UpdatePOInfoAttachment(existingPoInfoAttachment);
                                    }
                                    else
                                    {
                                        var poInfoAttachment = new POInfoAttachment();
                                        poInfoAttachment.POInfoId = item.Value;
                                        poInfoAttachment.AttachmentId = attachment.Id;
                                        poInfoAttachment.CreatedOn = DateTime.UtcNow;
                                        poInfoAttachment.ModifiedOn = DateTime.UtcNow;
                                        poInfoAttachment.CreatedBy = model.CreatedBy;
                                        poInfoAttachment.ModifiedBy = model.ModifiedBy;
                                        poInfoAttachment.IsDeleted = false;
                                        await _attachmentsService.InsertPOInfoAttachment(poInfoAttachment);
                                    }
                                }
                                else
                                {
                                    var existingPOInfoAttachment = await _attachmentsService.GetPOInforAttachmentByPOInfoId(item.Value);
                                    foreach (var itemPO in existingPOInfoAttachment)
                                    {
                                        await _attachmentsService.DeletePOInfoAttachment(itemPO);
                                    }
                                }
                            }
                        }
                    }
                    return (true, ConstantValues.Success, billingInformation.Id);
                }
                else
                {
                    return (false, ConstantValues.ProjectCostError, billingInformation.Id);
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }
        /// <summary>
        /// Get Billing Info Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<BillingInfoOrderModel>> GetBillingInfoOrder()
        {
            var billingOrderModelList = new List<BillingInfoOrderModel>();
            var orderList = await _orderService.GetAllOrders();
            foreach (var order in orderList)
            {
                var sowDocType = await _masterDataService.GetProjectTypeById(order.SOWDocumentId);
                BillingInfoOrderModel billingOrderModel = new BillingInfoOrderModel();
                billingOrderModel.OrderNumber = order.OrderNumber;
                billingOrderModel.Label = order.OrderName;
                billingOrderModel.Value = order.Id;
                billingOrderModel.CurrencyId = order?.CurrencyId ?? 0;
                billingOrderModel.OrderType = sowDocType?.Name ?? "";
                billingOrderModelList.Add(billingOrderModel);
            }
            return billingOrderModelList;

        }

        /// <summary>
        /// Get Order POInfo List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<OrderPOInfoModel>> GetCustomerPOList(int customerId)
        {
            var orderPOInfoModelList = new List<OrderPOInfoModel>();
            var POInfoList = await _pOInfoService.GetPOInfoByOrderId(customerId); 

            foreach (var poInfoMapping in POInfoList)
            {
                var poInformation = await _pOInfoService.GetPOInfoById(poInfoMapping.CustomerId);
                var order = await _orderService.GetCustomerOrderById(customerId);
                var billing = await _billingInfoService.GetBillingInfoPOMappingByIds(poInfoMapping.POInfoId);

                // Check if PORemainingAmount is greater than 0
                if (poInformation != null && poInformation.PORemainingAmount > 0)
                {
                    OrderPOInfoModel orderPOInfoModel = new OrderPOInfoModel();
                    orderPOInfoModel.Label = poInformation.PONumber ?? "";
                    orderPOInfoModel.Value = poInformation.Id;
                    orderPOInfoModel.CustomerId = customerId;
                    orderPOInfoModel.POAmount = poInformation.POAmount;

                    // Find the corresponding POId in billing
                    // Check if corresponding billing is found
                    if (billing != null && billing.POId == poInfoMapping.Id)
                    {
                        orderPOInfoModel.POAmountRemaining =  poInformation.PORemainingAmount;
                    }
                    else
                    {
                        // Handle the case where corresponding billing is not found
                        orderPOInfoModel.POAmountRemaining = poInformation.PORemainingAmount;
                    }
                    if (order.CurrencyId != null)
                    {
                        var currencyDetail = _masterDataService.GetCurrencyById(Convert.ToInt32(order.CurrencyId));
                        if (currencyDetail != null)
                        {
                            orderPOInfoModel.CurrencyPath = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", currencyDetail.Icon).Replace("\\", "/");
                        }
                    }
                    if (poInformation != null)
                    {
                        var poattachment = await _attachmentsService.GetPOInfoAttachmentByPoInfoId(poInformation.Id);
                        if (poattachment != null)
                        {
                            var attachment = await _attachmentsService.GetAttachmentById(poattachment.AttachmentId);
                            if (attachment != null)
                            {
                                string contentDirectory = "Content"; // The directory you want to extract

                                int contentIndex = attachment.FilePath.IndexOf(contentDirectory);
                                if (contentIndex != -1)
                                {
                                    string relativePath = attachment.FilePath.Substring(contentIndex);

                                    // Replace backslashes with forward slashes in the relative path
                                    relativePath = relativePath.Replace("\\", "/");

                                    // Construct the complete URL by combining the base URL and relative path
                                    string completeUrl = _tmConfig.SiteUrl.TrimEnd('/') + "/" + relativePath;

                                    orderPOInfoModel.FileName = attachment.FileName;
                                    orderPOInfoModel.FilePath = completeUrl;
                                }
                            }
                        }
                        else
                        {
                            orderPOInfoModel.FileName = "";
                            orderPOInfoModel.FilePath = "";
                        }
                    }
                    orderPOInfoModelList.Add(orderPOInfoModel);
                }
            }
            return orderPOInfoModelList;
        }
        /// <summary>
        /// Get ALL Billing Info List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<BillingInformationListModel>> GetAllBillingInfo(int customerId)
        {
            var billingOrderModelList = new List<BillingInformationListModel>();
            var billingInfoList = await _billingInfoService.GetAllBillingInformation(customerId);
            foreach (var billingInfo in billingInfoList)
            {
                var order = await _orderService.GetOrdersById(billingInfo.OrderId);
                var allPONumber = await GetPONumbersForBillingInfo(billingInfo.Id);

                BillingInformationListModel billingInfoModel = new BillingInformationListModel
                {
                    Id = billingInfo.Id,
                    MilestoneName = billingInfo.MilestoneName,
                    Deliverables = billingInfo.Deliverables,
                    ProjectCost = billingInfo.ProjectCost,
                    ProjectedHours = order?.EstimatedTotalHours ?? 0,
                    TimePeriod = billingInfo.TimePeriod,
                    ToBeRaised = billingInfo.ToBeRaised,
                    OrderType = order?.SOWDocumentId ?? 0,
                    OrderName = order?.OrderName ?? "",
                    OrderNumber = order?.OrderNumber ?? 0,
                    ManagerAction = billingInfo.ManagerAction,
                    DHAction = billingInfo.DHAction,
                    AHAction = billingInfo.AHAction,
                    RaiseDate = billingInfo.RaiseDate,
                    PONumberList = string.Join(", ", allPONumber),
                    CurrencyId = billingInfo.CurrencyId,
                    ActualBilling = billingInfo.ActualBilling,
                    CustomerId= billingInfo.CustomerId,
                };

                billingOrderModelList.Add(billingInfoModel);
            }
            return billingOrderModelList;
        }


        public async Task<(bool, string, int)> BillingInfoAction(int billingInfoId, int managerAction, int dHAction, int aHAction, DateTime? raiseDate, decimal actualBilling)
        {
            try
            {
                if (billingInfoId <= 0)
                {
                    return (false, "Invalid Billing Information ID", 0);
                }
                var billingInformation = await _billingInfoService.GetBillingInformationById(billingInfoId);

                if (billingInformation != null)
                {
                    billingInformation.ManagerAction = managerAction;
                    billingInformation.DHAction = dHAction;
                    billingInformation.AHAction = aHAction;
                    billingInformation.RaiseDate = raiseDate;
                    billingInformation.ActualBilling = actualBilling;
                    await _billingInfoService.UpdateBillingInformation(billingInformation);
                }

                return (true, ConstantValues.Success, billingInfoId);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }


        /// <summary>
        /// Get Order POInfo List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<GetBillingInformationModel> GetBillingInfoById(int billingInfoId)
        {
            if (billingInfoId <= 0)
            {
                return null;
            }

            var billingInfo = await _billingInfoService.GetBillingInformationById(billingInfoId);
            var order = await _orderService.GetOrdersById(billingInfo.OrderId);

            if (billingInfo == null)
            {
                return null;
            }

            var billingInfoPoList = await _billingInfoService.GetBillingInfoPOMappingByBillingInfoId(billingInfo.Id);
            var billingInfoOrderPOLists = new List<BillingInfoPOModel>();

            foreach (var item in billingInfoPoList)
            {
                var poInfo = await _pOInfoService.GetPOInfoById(item.POId);
                var poattachment = await _attachmentsService.GetPOInfoAttachmentByPoInfoId(item.POId);

                BillingInfoPOModel POInfoModel = new BillingInfoPOModel
                {
                    Id = item.Id,
                    POValue = item.POValue,
                    Value = poInfo?.Id ?? 0,
                    Text = poInfo?.PONumber ?? "",
                    POAmountRemaining = poInfo.POAmount -poInfo.PORemainingAmount ,
                    FileName = "",
                    FilePath = ""
                };

                if (poattachment != null)
                {
                    var attachment = await _attachmentsService.GetAttachmentById(poattachment.AttachmentId);

                    if (attachment != null)
                    {
                        string contentDirectory = "Content"; // The directory you want to extract
                        int contentIndex = attachment.FilePath.IndexOf(contentDirectory);

                        if (contentIndex != -1)
                        {
                            string relativePath = attachment.FilePath.Substring(contentIndex)
                                .Replace("\\", "/");

                            string completeUrl = _tmConfig.SiteUrl.TrimEnd('/') + "/" + relativePath;

                            POInfoModel.FileName = attachment.FileName;
                            POInfoModel.FilePath = completeUrl;
                        }
                    }
                }

                billingInfoOrderPOLists.Add(POInfoModel);
            }

            return new GetBillingInformationModel
            {
                Id = billingInfo.Id,
                MilestoneName = billingInfo.MilestoneName,
                Deliverables = billingInfo.Deliverables,
                OrderId = order?.Id ?? 0,
                ProjectCost = billingInfo.ProjectCost,
                ProjectedHours = billingInfo.ProjectedHours,
                TimePeriod = billingInfo.TimePeriod,
                ToBeRaised = billingInfo.ToBeRaised,
                OrderName = order?.OrderName ?? "",
                OrderNumber = order?.OrderNumber ?? 0,
                ManagerAction = billingInfo.ManagerAction,
                DHAction = billingInfo.DHAction,
                AHAction = billingInfo.AHAction,
                RaiseDate = billingInfo.RaiseDate,
                CurrencyId = billingInfo.CurrencyId,
                BillingInfoOrderPOList = billingInfoOrderPOLists
            };
        }


        public async Task<(bool, string, int)> AddPOBillingInfo(IList<AddBillingInfoPOModel> model)
        {
            foreach (var item in model)
            {
                var billingInformation = _billingInfoService.GetBillingInformationById(item.BillingInfoId);
                var order = await _orderService.GetOrdersById(item.OrderId);
                if (billingInformation != null)
                {

                    try
                    {
                        POInfo poInformation = new POInfo();
                        poInformation.CustomerId = order!=null ? order.CustomerId : 0;
                        poInformation.CreatedOn = DateTime.UtcNow;
                        poInformation.CreatedBy = item.CreatedBy;
                        poInformation.POAmount = item.POAmount;
                        poInformation.PONumber = item.PONUmber;
                        poInformation.PORemainingAmount = item.POAmount;
                        poInformation.CustomerId = item.CustomerId;
                        await _pOInfoService.InsertPOInfo(poInformation);
                        if (poInformation != null)
                        {

                            POInfoOrderMapping pOInfoOrderMapping = new POInfoOrderMapping();
                            pOInfoOrderMapping.POInfoId = poInformation.Id;
                            pOInfoOrderMapping.OrderId = item.OrderId;
                            pOInfoOrderMapping.CustomerId = item.CustomerId;
                            pOInfoOrderMapping.ConsumedAmount = 0;
                            pOInfoOrderMapping.CreatedOn = DateTime.UtcNow;
                            pOInfoOrderMapping.CreatedBy = item.CreatedBy;   
                            await _pOInfoService.InsertPOInfoOrderMapping(pOInfoOrderMapping);


                            var path = _tmConfig.AttachmentFolderPath;
                            string filePath = string.Concat(_env.ContentRootPath, path);
                            if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.Base64))
                            {
                                int lastIndex = item.FileName.LastIndexOf('.');

                                // Create the full file path where you want to save the image
                                string fullPath = Path.Combine(filePath, item.FileName);
                                // Check if the file already exists and delete it if needed
                                if (File.Exists(fullPath))
                                {
                                    File.Delete(fullPath);
                                }
                                try
                                {
                                    // Convert Base64 to Image
                                    using (var image = ConvertBase64ToImage(item.Base64))
                                    {
                                        // Save the image to the specified file path
                                        image.Save(fullPath);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log the exception message for debugging
                                    Console.WriteLine($"Image.Save Error: {ex.Message}");
                                    // Handle the exception or rethrow it as needed
                                }

                                // Create and insert an Attachment object with the newFileName
                                var attachment = new Attachments();
                                attachment.FilePath = fullPath;
                                attachment.FileName = item.FileName; // Use the newFileName here
                                attachment.CreatedBy = item.CreatedBy;
                                attachment.ModifiedBy = item.ModifiedBy;
                                attachment.CreatedOn = DateTime.UtcNow;
                                attachment.ModifiedOn = DateTime.UtcNow;
                                attachment.ModifiedBy = item.ModifiedBy;
                                await _attachmentsService.InsertAttachment(attachment);

                                // Create and insert a CustomerAttachment object
                                var poInfoAttachment = new POInfoAttachment();
                                poInfoAttachment.POInfoId = poInformation.Id;
                                poInfoAttachment.AttachmentId = attachment.Id;
                                poInfoAttachment.CreatedOn = DateTime.UtcNow;
                                poInfoAttachment.ModifiedOn = DateTime.UtcNow;
                                poInfoAttachment.CreatedBy = item.CreatedBy;
                                poInfoAttachment.ModifiedBy = item.ModifiedBy;
                                poInfoAttachment.IsDeleted = false;
                                await _attachmentsService.InsertPOInfoAttachment(poInfoAttachment);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        return (false, ex.Message, 0);
                    }
                }
            }
            return (true, ConstantValues.Success, 0);
        }

    }
    #endregion
}

