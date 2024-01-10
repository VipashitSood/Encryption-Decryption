using API.Models.POInformation;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Services.Pms.BillingInformations;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.PmsPOInformation;
using static API.Models.POInformation.PoInfoFilterModel;

namespace API.Factories.POInformation
{
    public class POInfoFactory : IPOInfoFactory
    {
        #region Fields
        private readonly IPOInfoService _poInfoService;
        private readonly ICustomersService _customersService;
        private readonly IOrderService _orderService;
        private readonly IAttachmentsService _attachmentsService;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private readonly IEncryptDecryptService _encryptDecryptService;
        private readonly IBillingInfoService _billingInfoService;
        #endregion

        #region Ctor
        public POInfoFactory(IPOInfoService poInfoService,
            ICustomersService customersService,
            IOrderService orderService,
             IAttachmentsService attachmentsService,
             TmConfig tmConfig,
             IWebHostEnvironment env,
             IEncryptDecryptService encryptDecryptService,
             IBillingInfoService billingInfoService)
        {
            _poInfoService = poInfoService;
            _customersService = customersService;
            _orderService = orderService;
            _attachmentsService = attachmentsService;
            _tmConfig = tmConfig;
            _env = env;
            _encryptDecryptService = encryptDecryptService;
            _billingInfoService= billingInfoService;
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
        #endregion Utilities

        #region Methods
        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> CreatePoInformation(PoInfoModel model)
        {
            try
            {
                POInfo poInformation = new POInfo();
                poInformation.CustomerId = model.CustomerId;
                poInformation.CreatedOn = DateTime.UtcNow;
                poInformation.CreatedBy = model.CreatedBy;
                poInformation.POAmount = model.POAmount;
                poInformation.PONumber = model.PONumber;
                await _poInfoService.InsertPOInfo(poInformation);

                if (poInformation != null)
                {
                    //if (model.OrderModelList != null && model.OrderModelList.Count > 0)
                    //{
                    //    foreach (var item in model.OrderModelList)
                    //    {
                    //        POInfoOrderMapping pOInfoOrderMapping = new POInfoOrderMapping();
                    //        pOInfoOrderMapping.POInfoId = poInformation.Id;
                    //        pOInfoOrderMapping.OrderId = item.OrderId ?? 0; 
                    //        pOInfoOrderMapping.ConsumedAmount = item.ConsumedAmount ?? 0; 
                    //        pOInfoOrderMapping.CreatedOn = DateTime.UtcNow;
                    //        pOInfoOrderMapping.CreatedBy = model.CreatedBy;
                    //        await _poInfoService.InsertPOInfoOrderMapping(pOInfoOrderMapping);
                    //    }
                    //}

                    var path = _tmConfig.AttachmentFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);
                    if (!string.IsNullOrEmpty(model.FileName) && !string.IsNullOrEmpty(model.Base64))
                    {
                        int lastIndex = model.FileName.LastIndexOf('.');
                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, model.FileName);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            // Convert Base64 to Image
                            using (var image = ConvertBase64ToImage(model.Base64))
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
                        attachment.FileName = model.FileName; // Use the newFileName here
                        attachment.CreatedBy = model.CreatedBy;
                        attachment.ModifiedBy = model.ModifiedBy;
                        attachment.CreatedOn = DateTime.UtcNow;
                        attachment.ModifiedOn = DateTime.UtcNow;
                        attachment.ModifiedBy = model.ModifiedBy;
                        await _attachmentsService.InsertAttachment(attachment);

                        // Create and insert a CustomerAttachment object
                        var poInfoAttachment = new POInfoAttachment();
                        poInfoAttachment.POInfoId = poInformation.Id;
                        poInfoAttachment.AttachmentId = attachment.Id;
                        poInfoAttachment.CreatedOn = DateTime.UtcNow;
                        poInfoAttachment.ModifiedOn = DateTime.UtcNow;
                        poInfoAttachment.CreatedBy = model.CreatedBy;
                        poInfoAttachment.ModifiedBy = model.ModifiedBy;
                        poInfoAttachment.IsDeleted = false;
                        await _attachmentsService.InsertPOInfoAttachment(poInfoAttachment);
                    }
                }
                return (true, ConstantValues.Success, poInformation.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> UpdatePoInformation(UpdatePoInfoModel model)
        {
            try
            {
                // Check if the provided model has a valid Po Information ID
                if (model.Id <= 0)
                {
                    return (false, "Invalid PO Information ID", 0);
                }
                var poInformation = await _poInfoService.GetPOInfoById(model.Id);

                // Check if the customer record exists
                if (poInformation == null)
                {
                    return (false, "Po Information not found", 0);
                }

                poInformation.Id = model.Id;
                poInformation.CustomerId = model.CustomerId;
                poInformation.ModifiedOn = DateTime.UtcNow;
                poInformation.ModifiedBy = model.CreatedBy;
                poInformation.POAmount = model.POAmount;
                poInformation.PONumber = model.PONumber;

                //update po information record
                await _poInfoService.UpdatePOInfo(poInformation);

                if (poInformation != null)
                {

                    //if (model.OrderModelList.Count > 0)
                    //{
                    //    foreach (var item in model.OrderModelList)
                    //    {
                    //        var poInfoOrderExist = await _poInfoService.GetByPOInfoIdandOrderId(poInformation.Id, item.OrderId ?? 0);
                    //        if (poInfoOrderExist == null)
                    //        {
                    //            POInfoOrderMapping pOInfoOrderMapping = new POInfoOrderMapping();
                    //            pOInfoOrderMapping.POInfoId = poInformation.Id;
                    //            pOInfoOrderMapping.OrderId = item.OrderId ?? 0;
                    //            pOInfoOrderMapping.ConsumedAmount = item.ConsumedAmount ?? 0;
                    //            pOInfoOrderMapping.CreatedOn = DateTime.UtcNow;
                    //            pOInfoOrderMapping.CreatedBy = model.CreatedBy;
                    //            await _poInfoService.InsertPOInfoOrderMapping(pOInfoOrderMapping);
                    //        }
                    //        else
                    //        {
                    //            poInfoOrderExist.POInfoId = poInformation.Id;
                    //            poInfoOrderExist.OrderId = item.OrderId ?? 0;
                    //            poInfoOrderExist.ConsumedAmount = item.ConsumedAmount ?? 0;
                    //            poInfoOrderExist.CreatedOn = DateTime.UtcNow;
                    //            poInfoOrderExist.CreatedBy = model.CreatedBy;
                    //            await _poInfoService.UpdatePOInfoOrderMapping(poInfoOrderExist);
                    //        }
                        //}
                    //}

                    var path = _tmConfig.AttachmentFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);
                    if (!string.IsNullOrEmpty(model.FileName) && !string.IsNullOrEmpty(model.Base64))
                    {
                        int lastIndex = model.FileName.LastIndexOf('.');

                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, model.FileName);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            // Convert Base64 to Image
                            using (var image = ConvertBase64ToImage(model.Base64))
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
                        attachment.FileName = model.FileName; // Use the newFileName here
                        attachment.CreatedBy = model.CreatedBy;
                        attachment.ModifiedBy = model.ModifiedBy;
                        attachment.CreatedOn = DateTime.UtcNow;
                        attachment.ModifiedOn = DateTime.UtcNow;
                        attachment.ModifiedBy = model.ModifiedBy;
                        await _attachmentsService.InsertAttachment(attachment);

                        var existingPoInfoAttachment = (await _attachmentsService.GetPOInforAttachmentByPOInfoId(poInformation.Id)).LastOrDefault();
                        if (existingPoInfoAttachment != null)
                        {
                            existingPoInfoAttachment.POInfoId = poInformation.Id;
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
                            poInfoAttachment.POInfoId = poInformation.Id;
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
                        var existingCustomerAttachment = await _attachmentsService.GetPOInforAttachmentByPOInfoId(poInformation.Id);
                        foreach (var item in existingCustomerAttachment)
                        {
                            await _attachmentsService.DeletePOInfoAttachment(item);
                        }
                    }
                }
                return (true, ConstantValues.Success, poInformation.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<PoInfoSearchModel>> GetAllPoInformation(int poId, int clientId, string companyName, int pageIndex, int pageSize)
        {

            var poInfoSearchList = new List<PoInfoSearchModel>();
            var poInfoList = await _poInfoService.GetAllPOInfo(poId, clientId, companyName);

            foreach (var poInfo in poInfoList)
            {
                decimal consumedAmount = 0;
                var poInfoSearchModel = new PoInfoSearchModel();
                var customer = await _customersService.GetCustomerById(poInfo.CustomerId);
                var poOrderMapping = await _poInfoService.GetPOInfoOrderMappingByPOInfoId(poInfo.Id);
                var billing = await _billingInfoService.GetBillingInfoPOMappingByIds(poId);
                var allOrderNames = new List<string>();
                // Loop through each poOrderMapping to split and collect order names
                foreach (var mapping in poOrderMapping)
                {
                    var order = await _orderService.GetOrdersById(mapping.OrderId);
                    if (order != null)
                    {
                        // Split the comma-separated order names into an array
                        string[] orderNameArray = order.OrderName?.Split(',');
                        if (orderNameArray?.Length>0)
                        {
                            // Trim each order name to remove leading/trailing spaces
                            var trimmedOrderNames = orderNameArray.Select(name => name.Trim());
                            // Add the individual order names to the list
                            allOrderNames.AddRange(trimmedOrderNames);
                            consumedAmount += mapping.ConsumedAmount;
                        }
                    }
                }


                poInfoSearchModel.Id = poInfo.Id;
                poInfoSearchModel.PONumber = poInfo.PONumber;
                poInfoSearchModel.POAmount = poInfo.POAmount;
                poInfoSearchModel.CustomerId = poInfo.CustomerId;
                poInfoSearchModel.ConsumedAmount = 0;
                poInfoSearchModel.RemainingAmount =poInfo.POAmount- poInfo.PORemainingAmount;
                poInfoSearchModel.ClientName = customer != null ? customer.Name : "";
                poInfoSearchModel.CompanyName = customer != null ? customer.Company : "";
                poInfoSearchModel.OrderName = allOrderNames.Any() ? string.Join(", ", allOrderNames) : ""; // Use an appropriate default message
                poInfoSearchList.Add(poInfoSearchModel);
            }

            return poInfoSearchList;

        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PoInfoFilterModel> PoInformationFilters()
        {

            var poInfoFilterModel = new PoInfoFilterModel();

            var customer = new ListItem { Label = "Select Client", Value = "0" };
            var poNumber = new ListItem { Label = "Select PO Number", Value = "0" };
            var company = new ListItem { Label = "Select Company Name", Value = "0" };

            var customers = await _customersService.GetAllCustomerWithoutPaging(string.Empty);
            poInfoFilterModel.Client.Add(customer);
            poInfoFilterModel.Client.AddRange(customers.Select(userModule =>
                new ListItem { Label = userModule.Name != null ? userModule.Name.ToString() : "", Value = userModule.Id.ToString() }));

            poInfoFilterModel.CompanyName.Add(company);
            poInfoFilterModel.CompanyName.AddRange(customers.Select(userModule =>
                new ListItem { Label = userModule.Company != null ? userModule.Company.ToString() : "", Value = userModule.Id.ToString() }));


            // Get a list of all order names
            var poinformations = await _poInfoService.GetAllPOInfo();
            poInfoFilterModel.PoNmmbers.Add(poNumber);
            poInfoFilterModel.PoNmmbers.AddRange(poinformations.Select(userModule =>
                new ListItem { Label = userModule.PONumber != null ? userModule.PONumber.ToString() : "", Value = userModule.Id.ToString() }));

            return poInfoFilterModel;

        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<POInfoCustomOrderModel>> GetCustomerOrder(int customerId)
        {
            var pOInfoCustomOrderModelList = new List<POInfoCustomOrderModel>();
            var customerOrderList = await _orderService.GetCustomerOrdersById(customerId);
            foreach (var customerOrder in customerOrderList)
            {
                POInfoCustomOrderModel pOInfoCustomOrderModel = new POInfoCustomOrderModel();
                pOInfoCustomOrderModel.OrderNumber = customerOrder.OrderNumber;
                pOInfoCustomOrderModel.Label = customerOrder.OrderName;
                pOInfoCustomOrderModel.Value = customerOrder.Id;
                pOInfoCustomOrderModel.OrderCost = _encryptDecryptService.DecryptString(customerOrder.OrderCost);
                pOInfoCustomOrderModel.CurrencyId = customerOrder.CurrencyId != null ? (int)customerOrder.CurrencyId : 0;
                pOInfoCustomOrderModelList.Add(pOInfoCustomOrderModel);
            }
            return pOInfoCustomOrderModelList;

        }

        /// <summary>
        ///  Customer List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IList<POInfoCustomerModel>> GetCustomer()
        {
            var pOInfoCustomerModelList = new List<POInfoCustomerModel>();
            var customerOrderList = await _customersService.GetAllCustomerWithoutPaging(String.Empty);
            foreach (var customerOrder in customerOrderList)
            {
                POInfoCustomerModel pOInfoCustomOrderModel = new POInfoCustomerModel();
                pOInfoCustomOrderModel.Label = customerOrder.Name;
                pOInfoCustomOrderModel.Value = customerOrder.Id;
                pOInfoCustomOrderModel.Email = customerOrder.Email;
                pOInfoCustomerModelList.Add(pOInfoCustomOrderModel);
            }
            return pOInfoCustomerModelList;
        }


        /// <summary>
        /// Get PoInformation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PoInfoResponseModel> GetPoInformationById(int id)
       {
            var poInfoModel = new PoInfoResponseModel();
            var poInformation = await _poInfoService.GetPOInfoById(id);
            var poOrderMapping = await _poInfoService.GetPOInfoOrderMappingByPOInfoId(poInformation.Id);
            var poattachment = await _attachmentsService.GetPOInfoAttachmentByPoInfoId(poInformation.Id);

            poInfoModel.Id = poInformation.Id;
            poInfoModel.CustomerId = poInformation.CustomerId;
            poInfoModel.PONumber = poInformation.PONumber;
            poInfoModel.POAmount = poInformation.POAmount;

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

                        poInfoModel.FileName = attachment.FileName;
                        poInfoModel.FilePath = completeUrl;
                    }
                }
            }
            foreach (var item in poOrderMapping)
            {
                var order = await _orderService.GetOrdersById(item.Id);
                PoInfoResponseOrderModel poOrderModel = new PoInfoResponseOrderModel();
                poOrderModel.Value = item.OrderId;
                poOrderModel.Label = order != null ? order.OrderName : "";
                poOrderModel.OrderCost = order != null ? _encryptDecryptService.DecryptString(order.OrderCost) : 0;
                poOrderModel.OrderNumber = order != null ? order.OrderNumber : 0;
                poOrderModel.ConsumedAmount = item.ConsumedAmount;
                poOrderModel.CurrencyId = order != null ? (int)order.CurrencyId : 0;
                poInfoModel.OrderModelList.Add(poOrderModel);
            }
            return poInfoModel;

        }
    }
    #endregion
}

