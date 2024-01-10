using API.Models.Orders;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core;
using Tm.Core.Configuration;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.PmsOrders;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;

namespace API.Factories.Orders
{
    public class PmsOrdersFactory : IPmsOrdersFactory
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly ICustomersService _customersService;
        private readonly IMasterDataService _masterService;
        private readonly IAttachmentsService _attachmentService;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptDecryptService _encryptDecryptService;
        #endregion

        #region Ctor
        public PmsOrdersFactory(IOrderService orderService,
            IWorkContext workContext,
            ICustomersService customersService,
            IMasterDataService masterService,
            IMapper mapper,
            IAttachmentsService attachmentService,
            TmConfig tmConfig,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            IEncryptDecryptService encryptDecryptService)
        {
            _orderService = orderService;
            _workContext = workContext;
            _mapper = mapper;
            _customersService = customersService;
            _masterService = masterService;
            _attachmentService = attachmentService;
            _tmConfig = tmConfig;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
           _encryptDecryptService = encryptDecryptService;
        }
        #endregion

        #region Utilites
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
        #endregion

        #region Methods 
        public async Task<IList<OrderListModel>> GetAllOrders(
         int customerId,
         int orderId,
         int sowDocumentId,
         bool? isPoRequired, bool? inHouse)
        {
            var data = await _orderService.GetAllOrders();
            // Filter the result based on the selected values
            data = data.ToList().Where(x => (!isPoRequired.HasValue || (isPoRequired.HasValue && isPoRequired.Value == x.IsPoRequired)) && !x.IsDeleted).ToList();
            if (inHouse != null)
            {
                data = data.ToList().Where(x => x.InHouse == inHouse).ToList();
            }
            if (customerId > 0)
            {
                data = data.ToList().Where(x => x.CustomerId == customerId).ToList();
            }
            if (orderId > 0)
            {
                data = data.ToList().Where(x => x.Id == orderId).ToList();
            }
            if (sowDocumentId > 0)
            {
                data = data.ToList().Where(x => x.SOWDocumentId == sowDocumentId).ToList();
            }

            data = data
                .OrderByDescending(x => x.Id)
                .ToList();

            IList<OrderListModel> orders = new List<OrderListModel>();
            foreach (var order in data)
            {
                var customer = await _customersService.GetCustomerById(order.CustomerId);
                var projectType = await _masterService.GetProjectTypeById(order.SOWDocumentId);

                OrderListModel item = new OrderListModel();
                item.Id = order.Id;
                item.ProjectId = order.ProjectId;
                item.OrderNumber = Convert.ToString(order.OrderNumber);
                item.OrderName = order.OrderName;
                item.PoRequired = order.IsPoRequired;
                item.ProjectCost = _encryptDecryptService.DecryptString(order.OrderCost);
                item.CustomerName = customer != null ? customer.Name : "";
                item.SowTypeName = projectType != null ? projectType.Name : "";
                item.SOWSigningDate = order.SOWSigningDate;
                if (order.CurrencyId != null)
                {
                    var currencyDetail =  _masterService.GetCurrencyById(Convert.ToInt32(order.CurrencyId));
                    if (currencyDetail != null)
                    {
                        item.CurrencyUrl = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", currencyDetail.Icon).Replace("\\", "/");
                    }
                }
                orders.Add(item);
            }
            return orders;
        }

        /// <summary>
        /// Filter Data for Order 
        /// </summary>
        /// <returns></returns>
        public async Task<FilteredOrderModel> OrderFilter()
        {
            // Initialize the FilteredOrderModel
            var filteredOrderModel = new FilteredOrderModel();

            // Create a static SelectListItem with value 0 for all items
            var allCustomers = new SelectListItem { Text = "Select All Company", Value = "0" };
            var allProjectTypes = new SelectListItem { Text = "Select All Order Type", Value = "0" };
            var allOrders = new SelectListItem { Text = "Select All Order", Value = "0" };
            var poAll = new SelectListItem { Text = "Selct All PO Required", Value = "null" };


            // Get a list of all customers
            var customers = await _customersService.GetAllCustomerWithoutPaging(string.Empty);
            filteredOrderModel.Companys.Add(allCustomers);
            filteredOrderModel.Companys.AddRange(customers.Select(userModule =>
                new SelectListItem { Text = (userModule.Company != null ? userModule.Company.ToString() : ""), Value = userModule.Id.ToString() }));

            // Get a list of all project types
            var projectTypes = await _masterService.GetAllProjectTypeWithoutPaging();
            filteredOrderModel.ProjectTypes.Add(allProjectTypes);
            filteredOrderModel.ProjectTypes.AddRange(projectTypes.Select(userModule =>
                new SelectListItem { Text = userModule.Name.ToString(), Value = userModule.Id.ToString() }));

            // Get a list of all order names
            var orderName = await _orderService.GetAllOrders();
            filteredOrderModel.OrderName.Add(allOrders);
            filteredOrderModel.OrderName.AddRange(orderName.Select(userModule =>
                new SelectListItem { Text = userModule.OrderName != null ? userModule.OrderName.ToString() : "", Value = userModule.Id.ToString() }));

            // Add a static SelectListItem for IsPORequired
            filteredOrderModel.IsPORequired.Add(poAll);
            filteredOrderModel.IsPORequired.Add(new SelectListItem { Text = "False", Value = "0" });
            filteredOrderModel.IsPORequired.Add(new SelectListItem { Text = "True", Value = "1" });

            // Add a static SelectListItem for IsInhouse
            filteredOrderModel.InHouse.Add(new SelectListItem { Text = "False", Value = "0", Selected = true });
            filteredOrderModel.InHouse.Add(new SelectListItem { Text = "True", Value = "1" });

            // Return the filteredOrderModel
            return filteredOrderModel;
        }



        /// <summary>
        /// Get Orders By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderModel> GetOrdersById(int id)
        {
            var order = await _orderService.GetOrdersById(id);

            if (order != null)
            {
                OrderModel orderModel = new OrderModel();
                orderModel.CustomerId = order.CustomerId;
                var customer = await _customersService.GetCustomerById(order.CustomerId);
                orderModel.CustomerName = customer != null ? customer.Name : "";
                orderModel.Id = order.Id;
                orderModel.CurrencyId = Convert.ToInt32(order.CurrencyId);
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderName = order.OrderName;
                orderModel.SOWDocumentId = order.SOWDocumentId;
                orderModel.SOWSigningDate = order.SOWSigningDate;
                orderModel.OrderCost = _encryptDecryptService.DecryptString(order.OrderCost);
                orderModel.EstimatedEfforts = order.EstimatedEfforts;
                orderModel.EstimatedTotalHours = order.EstimatedTotalHours;
                orderModel.EstimatedHourlyCost = order.EstimatedHourlyCost;
                orderModel.InHouse = order.InHouse;
                orderModel.IsPoRequired = order.IsPoRequired;
                orderModel.Notes = order.Notes;
                orderModel.CreatedBy = order.CreatedBy;
                orderModel.ModifiedBy = order.ModifiedBy;
                orderModel.TimeUnitId = order.TimeUnitId;
                orderModel.ProjectDomainId = order.ProjectDomainId;
                orderModel.ProjectId = order.ProjectId;
                orderModel.DeliveryHeadId = order.DeliveryHeadId;

                var SOWDocument = (await _attachmentService.GetOrderAttachmentByOrderId(order.Id, isSwoDocument: true, isPoUpload: false)).LastOrDefault();
                if (SOWDocument != null)
                {
                    var attachment = await _attachmentService.GetAttachmentById(SOWDocument.AttachmentId);

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

                            orderModel.SOWDocumentFilePath = completeUrl != null ? completeUrl : "";
                            orderModel.SOWDocumentFileName = attachment != null ? attachment.FileName : "";
                        }
                    }
                   
                }
                orderModel.SOWDocumentBase64 = "";
                return orderModel;
            }

            throw new Exception(ConstantValues.RecordNotFound);
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> CreateOrder(IList<OrderModel> orderModels)
        {
            try
            {
                foreach (var model in orderModels)
                {
                    // Create a new PmsOrders instance and populate it with data from the OrderModel
                    // Initialize the Random object
                    Random random = new Random();
                    // Generate a random four-digit number between 1000 and 9999
                    int randomNumber = random.Next(1000, 10000);
                    var newOrder = new PmsOrders
                    {
                        CustomerId = model.CustomerId,
                        ProjectDomainId = model.ProjectDomainId,
                        ProjectId = model.ProjectId,
                        DeliveryHeadId = model.DeliveryHeadId,
                        OrderNumber = randomNumber,
                        OrderName = model.OrderName,
                        SOWDocumentId = model.SOWDocumentId,
                        SOWSigningDate = DateTime.UtcNow,
                        OrderCost = _encryptDecryptService.EncryptString(model.OrderCost),
                        EstimatedEfforts = model.EstimatedEfforts,
                        EstimatedTotalHours = model.EstimatedTotalHours,
                        EstimatedHourlyCost = model.EstimatedHourlyCost,
                        InHouse = model.InHouse,
                        IsPoRequired = model.IsPoRequired,
                        Notes = model.Notes,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = model.CreatedBy,
                        ModifiedBy = model.ModifiedBy,
                        ModifiedOn = null,
                        IsDeleted = false,
                        CurrencyId = model.CurrencyId,
                        TimeUnitId = model.TimeUnitId,
                    };

                    // Insert the new order into your data store (replace _orderService.InsertOrder with your actual service method)
                    await _orderService.InsertOrder(newOrder);

                    if (newOrder != null)
                    {
                        var path = _tmConfig.OrderAttachmentFolderPath;
                        string filePath = string.Concat(_env.ContentRootPath, path);

                        if (!string.IsNullOrEmpty(model.SOWDocumentFileName))
                        {
                            int lastIndex = model.SOWDocumentFileName.LastIndexOf('.');
                            // Create the full file path where you want to save the image
                            string fullPath = Path.Combine(filePath, model.SOWDocumentFileName);
                            // Check if the file already exists and delete it if needed
                            if (File.Exists(fullPath))
                            {
                                File.Delete(fullPath);
                            }
                            try
                            {
                                // Convert Base64 to Image
                                using (var image = ConvertBase64ToImage(model.SOWDocumentBase64))
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

                            //save attachment in table 

                            Attachments attachment = new Attachments();
                            attachment.FilePath = fullPath;
                            attachment.FileName = model.SOWDocumentFileName;
                            attachment.CreatedBy = model.CreatedBy;
                            attachment.ModifiedBy = model.ModifiedBy;
                            attachment.CreatedOn = DateTime.UtcNow;
                            attachment.ModifiedOn = DateTime.UtcNow;
                            attachment.ModifiedBy = model.ModifiedBy;
                            await _attachmentService.InsertAttachment(attachment);
                            if (attachment != null)
                            {
                                OrderAttachment orderAttachment = new OrderAttachment();
                                orderAttachment.AttachmentId = attachment.Id;
                                orderAttachment.OrderId = newOrder.Id;
                                orderAttachment.IsSOWDocument = true;
                                orderAttachment.IsPOUpload = false;
                                orderAttachment.CreatedOn = DateTime.UtcNow;
                                orderAttachment.ModifiedOn = DateTime.UtcNow;
                                orderAttachment.CreatedBy = model.CreatedBy;
                                orderAttachment.ModifiedBy = model.ModifiedBy;
                                orderAttachment.IsDeleted = false;
                                await _attachmentService.InsertOrderAttachment(orderAttachment);
                            }
                        }
                    }
                }
                return (true, "Order created successfully", 0);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        /// <summary>
        ///Update Order 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> UpdateOrder(OrderModel model)
        {
            try
            {
                // Validate the model or perform any necessary checks here.

                // Fetch the existing order from the database based on the OrderId (assuming you have an OrderId property in the model).
                var existingOrder = await _orderService.GetOrdersById(model.Id);

                if (existingOrder == null)
                {
                    return (false, "Order not found.", 0);
                }

                // Update the order properties with the new values from the model.
                // Here, you can update specific properties of the existing order.
                existingOrder.CustomerId = model.CustomerId;
                existingOrder.OrderName = model.OrderName;
                existingOrder.SOWSigningDate = model.SOWSigningDate;
                existingOrder.SOWDocumentId = model.SOWDocumentId;
                existingOrder.PONumber = model.PONumber;
                existingOrder.OrderCost = _encryptDecryptService.EncryptString(model.OrderCost);
                existingOrder.EstimatedEfforts = model.EstimatedEfforts;
                existingOrder.EstimatedTotalHours = model.EstimatedTotalHours;
                existingOrder.EstimatedHourlyCost = model.EstimatedHourlyCost;
                existingOrder.InHouse = model.InHouse;
                existingOrder.IsPoRequired = model.IsPoRequired;
                existingOrder.Notes = model.Notes;
                existingOrder.ModifiedOn = DateTime.UtcNow;
                existingOrder.CurrencyId = model.CurrencyId;
                existingOrder.TimeUnitId = model.TimeUnitId;
                existingOrder.ProjectDomainId = model.ProjectDomainId;
                existingOrder.ProjectId = model.ProjectId;
                existingOrder.DeliveryHeadId = model.DeliveryHeadId;
                // Save the updated order to the database.
                await _orderService.UpdateOrder(existingOrder);

                if (existingOrder != null)
                {
                    var path = _tmConfig.OrderAttachmentFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);
                    if (!string.IsNullOrEmpty(model.SOWDocumentFileName) && !string.IsNullOrEmpty(model.SOWDocumentBase64))
                    {
                        int lastIndex = model.SOWDocumentFileName.LastIndexOf('.');
                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, model.SOWDocumentFileName);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            // Convert Base64 to Image
                            using (var image = ConvertBase64ToImage(model.SOWDocumentBase64))
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

                        //save attachment in table 

                        Attachments attachment = new Attachments();
                        attachment.FilePath = fullPath;
                        attachment.FileName = model.SOWDocumentFileName;
                        attachment.CreatedBy = model.CreatedBy;
                        attachment.ModifiedBy = model.ModifiedBy;
                        attachment.CreatedOn = DateTime.UtcNow;
                        attachment.ModifiedOn = DateTime.UtcNow;
                        attachment.ModifiedBy = model.ModifiedBy;
                        await _attachmentService.InsertAttachment(attachment);
                        var existingOrderAttachment = (await _attachmentService.GetOrderAttachmentByOrderId(existingOrder.Id, isSwoDocument: true, isPoUpload: false)).LastOrDefault();
                        if (existingOrderAttachment != null)
                        {
                            existingOrderAttachment.AttachmentId = attachment.Id;
                            existingOrderAttachment.OrderId = existingOrder.Id;
                            existingOrderAttachment.IsSOWDocument = true;
                            existingOrderAttachment.IsPOUpload = false;
                            existingOrderAttachment.CreatedOn = DateTime.UtcNow;
                            existingOrderAttachment.ModifiedOn = DateTime.UtcNow;
                            existingOrderAttachment.CreatedBy = model.CreatedBy;
                            existingOrderAttachment.ModifiedBy = model.ModifiedBy;
                            existingOrderAttachment.IsDeleted = false;
                            await _attachmentService.UpdateOrderAttachment(existingOrderAttachment);
                        }
                        else
                        {
                            OrderAttachment orderAttachment = new OrderAttachment();
                            orderAttachment.AttachmentId = attachment.Id;
                            orderAttachment.OrderId = existingOrder.Id;
                            orderAttachment.IsSOWDocument = true;
                            orderAttachment.IsPOUpload = false;
                            orderAttachment.CreatedOn = DateTime.UtcNow;
                            orderAttachment.ModifiedOn = DateTime.UtcNow;
                            orderAttachment.CreatedBy = model.CreatedBy;
                            orderAttachment.ModifiedBy = model.ModifiedBy;
                            orderAttachment.IsDeleted = false;
                            await _attachmentService.InsertOrderAttachment(orderAttachment);
                        }
                    }
                    else
                    {
                        //var existingOrderAttachment = await _attachmentService.GetOrderAttachmentByOrderId(existingOrder.Id, isSwoDocument: true, isPoUpload: false);
                        //foreach (var item in existingOrderAttachment)
                        //{
                        //    await _attachmentService.DeleteOrderAttachment(item);
                        //}
                    }
                }
                return (true, "Order updated successfully.", existingOrder.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }


        /// <summary>
        /// Delete Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteOrders(int id)
        {
            if (id > 0)
            {
                var customer = await _orderService.GetOrdersById(id);
                if (customer != null)
                {
                    customer.ModifiedOn = DateTime.UtcNow;
                    customer.IsDeleted = true;
                    await _orderService.UpdateOrder(customer);
                    return (true, ConstantValues.Success);
                }
                else
                {
                    return (false, ConstantValues.CantUpdateRecord);
                }

            }
            else
            {
                return (false, ConstantValues.RecordNotFound);
            }
        }

    }
    #endregion
}
