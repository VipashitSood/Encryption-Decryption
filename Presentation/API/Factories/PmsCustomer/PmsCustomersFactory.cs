using API.Helpers;
using API.Models.Customer;
using API.Models.PmsCustomer;
using API.Models.ProjectDetail;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
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
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Core.Domain.Pms.PmsOrders;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Services.Pms.EncryptDecrypt;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsAttachments;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.ProjectDetail;

namespace API.Factories.Customer
{
    public class PmsCustomersFactory : IPmsCustomersFactory
    {
        #region Fields
        private readonly ICustomersService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly IOrderService _ordersService;
        private readonly IAttachmentsService _attachmentService;
        private readonly TmConfig _tmConfig;
        private readonly IWebHostEnvironment _env;
        private readonly IProjectsService _projectsService;
        private readonly IMasterDataService _masterDataService;
        private readonly IEncryptDecryptService _encryptDecryptService;
        #endregion

        #region Ctor
        public PmsCustomersFactory(ICustomersService customerService,
            IWorkContext workContext,
            IMapper mapper,
            IOrderService ordersService,
            IAttachmentsService attachmentService,
            TmConfig tmConfig,
            IWebHostEnvironment env,
            IProjectsService projectsService,
            IMasterDataService masterDataService,
            IEncryptDecryptService encryptDecryptService)
        {
            _customerService = customerService;
            _workContext = workContext;
            _mapper = mapper;
            _ordersService = ordersService;
            _attachmentService = attachmentService;
            _tmConfig = tmConfig;
            _env = env;
            _projectsService = projectsService;
            _masterDataService = masterDataService;
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

        #region Customers



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ProjectName>> GetProjectDetailNames()
        {
            var generalDetails = await _projectsService.GetAllProjects();
            List<ProjectName> projectList = new List<ProjectName>();
            try
            {
                if (generalDetails != null && generalDetails.Any())
                {
                    foreach (var project in generalDetails)
                    {
                        ProjectName projectName = new ProjectName();
                        projectName.Name = project.Name;
                        projectName.Id = project.Id;
                        projectList.Add(projectName);

                    }

                    return projectList.OrderBy(x => x.Name).ToList();
                }

                return new List<ProjectName>();
            }
            catch (Exception)
            {
                throw new Exception(ConstantValues.NoRecordFoundForUpdate);
            }
        }



        /// <summary>
        /// Filter Data for Order 
        /// </summary>
        /// <returns></returns>
        public async Task<FilterCustomerModel> CustomerFilter()
        {
            // Initialize the FilteredOrderModel
            var filterCustomerModel = new FilterCustomerModel();

            // Create a static SelectListItem with value 0 for all items
            var customer = new SelectListItem { Text = "Select Customer Name", Value = "0" };
            var company = new SelectListItem { Text = "Select Company Name", Value = "0" };


            // Get a list of all customers Name
            var customers = await _customerService.GetAllCustomerWithoutPaging(string.Empty);
            filterCustomerModel.CustomerName.Add(customer);
            filterCustomerModel.CustomerName.AddRange(customers.Where(x => x.Name != "" && !string.IsNullOrEmpty(x.Name)).Select(userModule =>
                new SelectListItem { Text = userModule.Name != null ? userModule.Name : "", Value = userModule.Name != null ? userModule.Name : "" }));

            // Get a list of  all company Name
            filterCustomerModel.CompanyName.Add(company);
            filterCustomerModel.CompanyName.AddRange(customers.Where(x => x.Company != "" && !string.IsNullOrEmpty(x.Company)).Select(userModule =>
                new SelectListItem { Text = userModule.Company != null ? userModule.Company.ToString() : "", Value = userModule.Company != null ? userModule.Company.ToString() : "" }));

            // Return the filteredOrderModel
            return filterCustomerModel;
        }

        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<CustomerResponseModel>> GetAllCustomer(int pageNumber, int pageSize, string companyName = "", string customerName = "")
        {
            return await _customerService.GetAllCustomer(pageNumber, pageSize,companyName, customerName);
        }

        /// <summary>
        /// Get Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UpdatePmsCustomerModel> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            try
            {
                if (customer != null)
                {
                    UpdatePmsCustomerModel customerModel = new UpdatePmsCustomerModel();
                    customerModel.Id = customer.Id;
                    customerModel.ModifiedBy = customer.ModifiedBy;
                    customerModel.Name = customer.Name;
                    customerModel.Company = customer.Company;
                    customerModel.Address = customer.Address;
                    customerModel.PhoneNo = customer.PhoneNo;
                    customerModel.Email = customer.Email;
                    customerModel.ContactPersonName = customer.ContactPersonName;
                    customerModel.ContactEmail = customer.ContactEmail;
                    customerModel.ContactPhoneNo = customer.ContactPhoneNo;
                    customerModel.SameInfo = Convert.ToBoolean(customer.SameInfo) ? "Yes" : "No";
                    var msaAttachment = (await _attachmentService.GetCustomerAttachmentByCustomerId(customer.Id, msa: true, nda: false)).LastOrDefault();
                    if (msaAttachment != null)
                    {
                        var attachment = await _attachmentService.GetAttachmentById(msaAttachment.AttachmentId);
                        customerModel.MSAFilePath = attachment != null ? attachment.FilePath : "";
                        customerModel.MSAFileName = attachment != null ? attachment.FileName : "";
                    }
                    customerModel.MSABase64 = "";
                    customerModel.NDABase64 = "";
                    var ndaAttachment = (await _attachmentService.GetCustomerAttachmentByCustomerId(customer.Id, msa: false, nda: true)).LastOrDefault();
                    if (ndaAttachment != null)
                    {
                        var attachment = await _attachmentService.GetAttachmentById(ndaAttachment.AttachmentId);
                        customerModel.NDAFilePath = attachment != null ? attachment.FilePath : "";
                        customerModel.NDAFileName = attachment != null ? attachment.FileName : "";
                    }
                    return customerModel;
                }
                throw new Exception("No record found.");
            }
            catch (Exception)
            {
                throw new Exception(ConstantValues.RecordNotFound);
            };
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string, int)> CreateCustomer(PmsCustomerModel model)
        {
            try
            {
                var customers = await _customerService.GetAllCustomerWithoutPaging(String.Empty);
                if (customers.Any(x => x.Name == model.Name))
                {
                    return (false, ConstantValues.CustomerNameAlreadyExist, 0);
                }

                if (customers.Any(x => x.PhoneNo == model.PhoneNo))
                {
                    return (false, ConstantValues.CustomerPhoneAlreadyExist, 0);
                }
                if (model.Id > 0)
                {
                    return (false, ConstantValues.UnknownUser, -1);
                }
                else
                {
                    var customer = new PmsCustomer
                    {
                        Name = model.Name,
                        Company = model.Company,
                        Address = model.Address,
                        PhoneNo = model.PhoneNo,
                        Email = model.Email,
                        ContactPersonName = model.ContactPersonName,
                        ContactEmail = model.ContactEmail,
                        ContactPhoneNo = model.ContactPhoneNo,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = model.CreatedBy,
                        ModifiedBy = model.CreatedBy,
                        ModifiedOn = null,
                        IsDeleted = false,
                        SameInfo = (!string.IsNullOrEmpty(model.SameInfo) ? (model.SameInfo.ToLower() == "yes" ? true : false) : false)
                    };

                    await _customerService.InsertCustomer(customer);

                    if (customer != null)
                    {
                        var path = _tmConfig.CustomerAttachmentFolderPath;
                        string filePath = string.Concat(_env.ContentRootPath, path);

                        if (!string.IsNullOrEmpty(model.MSAFileName))
                        {
                            int lastIndex = model.MSAFileName.LastIndexOf('.');

                            // Create the full file path where you want to save the image
                            string fullPath = Path.Combine(filePath, model.MSAFileName);

                            //save file
                            FileHelper.Savefile(filePath,fullPath, model.MSABase64);

                            // Create and insert an Attachment object with the newFileName
                            var attachment = new Attachments();
                            attachment.FilePath = fullPath;
                            attachment.FileName = model.MSAFileName; // Use the newFileName here
                            attachment.CreatedBy = model.CreatedBy;
                            attachment.ModifiedBy = model.ModifiedBy;
                            attachment.CreatedOn = DateTime.UtcNow;
                            attachment.ModifiedOn = DateTime.UtcNow;
                            attachment.ModifiedBy = model.ModifiedBy;
                            await _attachmentService.InsertAttachment(attachment);

                            // Create and insert a CustomerAttachment object
                            var customerAttachment = new CustomerAttachment();
                            customerAttachment.CustomerId = customer.Id;
                            customerAttachment.AttachmentId = attachment.Id;
                            customerAttachment.MSA = true;
                            customerAttachment.NDA = false;
                            customerAttachment.CreatedOn = DateTime.UtcNow;
                            customerAttachment.ModifiedOn = DateTime.UtcNow;
                            customerAttachment.CreatedBy = model.CreatedBy;
                            customerAttachment.ModifiedBy = model.ModifiedBy;
                            customerAttachment.IsDeleted = false;
                            await _attachmentService.InsertCustomerAttachment(customerAttachment);
                        }


                        if (!string.IsNullOrEmpty(model.NDAFileName))
                        {
                            int lastIndex = model.NDAFileName.LastIndexOf('.');
                            // Create the full file path where you want to save the image
                            string fullPath = Path.Combine(filePath, model.NDAFileName);

                            //save file
                            FileHelper.Savefile(filePath,fullPath, model.NDABase64);
                            
                            //save attachment in table 
                            var attachment = new Attachments();
                            attachment.FilePath = fullPath;
                            attachment.FileName = model.NDAFileName; // Use the newFileName here
                            attachment.CreatedBy = model.CreatedBy;
                            attachment.ModifiedBy = model.ModifiedBy;
                            attachment.CreatedOn = DateTime.UtcNow;
                            attachment.ModifiedOn = DateTime.UtcNow;
                            attachment.ModifiedBy = model.ModifiedBy;
                            await _attachmentService.InsertAttachment(attachment);

                            if (attachment != null)
                            {
                                CustomerAttachment customerAttachment = new CustomerAttachment();
                                customerAttachment.AttachmentId = attachment.Id;
                                customerAttachment.CustomerId = customer.Id;
                                customerAttachment.MSA = false;
                                customerAttachment.NDA = true;
                                customerAttachment.CreatedOn = DateTime.UtcNow;
                                customerAttachment.ModifiedOn = DateTime.UtcNow;
                                customerAttachment.CreatedBy = model.CreatedBy;
                                customerAttachment.ModifiedBy = model.ModifiedBy;
                                customerAttachment.IsDeleted = false;
                                await _attachmentService.InsertCustomerAttachment(customerAttachment);
                            }
                        }

                        if (model.OrderModelList.Count > 0)
                        {
                            foreach (var item in model.OrderModelList)
                            {
                                // Initialize the Random object
                                Random random = new Random();
                                // Generate a random four-digit number between 1000 and 9999
                                int randomNumber = random.Next(1000, 10000);

                                var order = new PmsOrders
                                {
                                    CustomerId = customer.Id,
                                    OrderNumber = randomNumber,
                                    OrderName = item.OrderName,
                                    SOWDocumentId = item.SOWDocumentId,
                                    SOWSigningDate = item.SOWSigningDate,
                                    IsPoRequired = item.IsPoRequired,
                                    PONumber = item.PONumber,
                                    OrderCost = _encryptDecryptService.EncryptString(item.OrderCost),
                                    EstimatedEfforts = item.EstimatedEfforts,
                                    EstimatedTotalHours = item.EstimatedTotalHours,
                                    EstimatedHourlyCost = item.EstimatedHourlyCost,
                                    InHouse = item.InHouse,
                                    Notes = item.Notes,
                                    CreatedBy = model.CreatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    ModifiedOn = DateTime.UtcNow,
                                    ModifiedBy = model.ModifiedBy,
                                    IsDeleted = false,
                                    CurrencyId = item.CurrencyId,
                                    TimeUnitId = item.TimeUnitId,
                                    ProjectDomainId = item.ProjectDomainId,
                                    ProjectId = item.ProjectId,
                                    DeliveryHeadId = item.DeliveryHeadId,
                                };
                                await _ordersService.InsertOrder(order);

                                if (!string.IsNullOrEmpty(item.ProjectName))
                                {
                                    Projects project = new Projects();
                                    project.Name = item.ProjectName;
                                    project.OrderId = order.Id;
                                    project.CreatedBy = model.CreatedBy;
                                    project.CreatedOn = DateTime.UtcNow;
                                    await _projectsService.InsertProjects(project);
                                    order.ProjectId = project.Id;
                                    await _ordersService.UpdateOrder(order);
                                }
                                if (!string.IsNullOrEmpty(item.ProjectDomain))
                                {
                                    ProjectDomain projectDomain = new ProjectDomain();
                                    projectDomain.Name = item.ProjectDomain;
                                    projectDomain.CreatedBy = "";//model.CreatedBy;
                                    projectDomain.CreatedOn = DateTime.UtcNow;
                                    await _masterDataService.InsertProjectDomain(projectDomain);
                                    order.ProjectDomainId = projectDomain.Id;
                                    await _ordersService.UpdateOrder(order);
                                }

                                if (order != null)
                                {
                                    if (!string.IsNullOrEmpty(item.SOWDocumentFileName))
                                    {

                                        int lastIndex = item.SOWDocumentFileName.LastIndexOf('.');
                                        // Create the full file path where you want to save the image
                                        string fullPath = Path.Combine(filePath, item.SOWDocumentFileName);
                                        //save file
                                        FileHelper.Savefile(filePath,fullPath, item.SOWDocumentBase64);
                                        
                                        //save attachment in table 

                                        Attachments attachment = new Attachments();
                                        attachment.FilePath = fullPath;
                                        attachment.FileName = item.SOWDocumentFileName;
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
                                            orderAttachment.OrderId = order.Id;
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
                                    if (!string.IsNullOrEmpty(item.POUploadFileName))
                                    {
                                        int lastIndex = item.POUploadFileName.LastIndexOf('.');
                                        // Create the full file path where you want to save the image
                                        string fullPath = Path.Combine(filePath, item.POUploadFileName);

                                        FileHelper.Savefile(filePath,fullPath, item.POUploadBase64);
                                        //save attachment in table 

                                        Attachments attachment = new Attachments();
                                        attachment.FilePath = fullPath;
                                        attachment.FileName = item.POUploadFileName;
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
                                            orderAttachment.OrderId = order.Id;
                                            orderAttachment.IsSOWDocument = false;
                                            orderAttachment.IsPOUpload = true;
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
                        }
                    }
                    return (true, ConstantValues.Success, customer.Id);
                }

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
        public async Task<(bool, string, int)> UpdateCustomer(UpdatePmsCustomerModel model)
        {
            try
            {
                // Check if the provided model has a valid customer ID
                if (model.Id <= 0)
                {
                    return (false, "Invalid customer ID", 0);
                }

                // Retrieve the existing customer record by ID
                var existingCustomer = await _customerService.GetCustomerById(model.Id);

                // Check if the customer record exists
                if (existingCustomer == null)
                {
                    return (false, "Customer not found", 0);
                }

                // Update the customer record with the new data
                existingCustomer.Name = model.Name;
                existingCustomer.Company = model.Company;
                existingCustomer.Address = model.Address;
                existingCustomer.PhoneNo = model.PhoneNo;
                existingCustomer.Email = model.Email;
                existingCustomer.ContactPersonName = model.ContactPersonName;
                existingCustomer.ContactEmail = model.ContactEmail;
                existingCustomer.ContactPhoneNo = model.ContactPhoneNo;
                existingCustomer.ModifiedBy = model.ModifiedBy;
                existingCustomer.SameInfo = (!string.IsNullOrEmpty(model.SameInfo) ? (model.SameInfo.ToLower() == "yes" ? true : false) : false);
                existingCustomer.ModifiedOn = DateTime.UtcNow;

                // Save the updated customer record
                await _customerService.UpdateCustomer(existingCustomer);

                if (existingCustomer != null)
                {
                    var path = _tmConfig.CustomerAttachmentFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);

                    if (!string.IsNullOrEmpty(model.MSAFileName) && !string.IsNullOrEmpty(model.MSABase64))
                    {
                        int lastIndex = model.MSAFileName.LastIndexOf('.');

                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, model.MSAFileName);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            // Convert Base64 to Image
                            using (var image = ConvertBase64ToImage(model.MSABase64))
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
                        attachment.FileName = model.MSAFileName; // Use the newFileName here
                        attachment.CreatedBy = model.CreatedBy;
                        attachment.ModifiedBy = model.ModifiedBy;
                        attachment.CreatedOn = DateTime.UtcNow;
                        attachment.ModifiedOn = DateTime.UtcNow;
                        attachment.ModifiedBy = model.ModifiedBy;
                        await _attachmentService.InsertAttachment(attachment);

                        var existingCustomerAttachment = (await _attachmentService.GetCustomerAttachmentByCustomerId(existingCustomer.Id, msa: true, nda: false)).LastOrDefault();
                        if (existingCustomerAttachment != null)
                        {
                            existingCustomerAttachment.CustomerId = existingCustomer.Id;
                            existingCustomerAttachment.AttachmentId = attachment.Id;
                            existingCustomerAttachment.MSA = true;
                            existingCustomerAttachment.NDA = false;
                            existingCustomerAttachment.CreatedOn = DateTime.UtcNow;
                            existingCustomerAttachment.ModifiedOn = DateTime.UtcNow;
                            existingCustomerAttachment.CreatedBy = model.CreatedBy;
                            existingCustomerAttachment.ModifiedBy = model.ModifiedBy;
                            existingCustomerAttachment.IsDeleted = false;
                            await _attachmentService.UpdateCustomerAttachment(existingCustomerAttachment);
                        }
                        else
                        {
                            var customerAttachment = new CustomerAttachment();
                            customerAttachment.CustomerId = existingCustomer.Id;
                            customerAttachment.AttachmentId = attachment.Id;
                            customerAttachment.MSA = true;
                            customerAttachment.NDA = false;
                            customerAttachment.CreatedOn = DateTime.UtcNow;
                            customerAttachment.ModifiedOn = DateTime.UtcNow;
                            customerAttachment.CreatedBy = model.CreatedBy;
                            customerAttachment.ModifiedBy = model.ModifiedBy;
                            customerAttachment.IsDeleted = false;
                            await _attachmentService.InsertCustomerAttachment(customerAttachment);
                        }
                    }
                    else
                    {
                        //var existingCustomerAttachment = await _attachmentService.GetCustomerAttachmentByCustomerId(existingCustomer.Id, msa: true, nda: false);
                        //foreach (var item in existingCustomerAttachment)
                        //{
                        //    await _attachmentService.DeleteCustomerAttachment(item);
                        //}
                    }

                    if (!string.IsNullOrEmpty(model.NDAFileName) && !string.IsNullOrEmpty(model.NDABase64))
                    {
                        int lastIndex = model.NDAFileName.LastIndexOf('.');
                        // Create the full file path where you want to save the image
                        string fullPath = Path.Combine(filePath, model.NDAFileName);
                        // Check if the file already exists and delete it if needed
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        try
                        {
                            // Convert Base64 to Image
                            using (var image = ConvertBase64ToImage(model.NDABase64))
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
                        var attachment = new Attachments();
                        attachment.FilePath = fullPath;
                        attachment.FileName = model.NDAFileName; // Use the newFileName here
                        attachment.CreatedBy = model.CreatedBy;
                        attachment.ModifiedBy = model.ModifiedBy;
                        attachment.CreatedOn = DateTime.UtcNow;
                        attachment.ModifiedOn = DateTime.UtcNow;
                        attachment.ModifiedBy = model.ModifiedBy;
                        await _attachmentService.InsertAttachment(attachment);

                        if (attachment != null)
                        {

                            var existingCustomerAttachment = (await _attachmentService.GetCustomerAttachmentByCustomerId(existingCustomer.Id, msa: false, nda: true)).LastOrDefault();
                            if (existingCustomerAttachment != null)
                            {
                                existingCustomerAttachment.AttachmentId = attachment.Id;
                                existingCustomerAttachment.CustomerId = existingCustomer.Id;
                                existingCustomerAttachment.MSA = false;
                                existingCustomerAttachment.NDA = true;
                                existingCustomerAttachment.CreatedOn = DateTime.UtcNow;
                                existingCustomerAttachment.ModifiedOn = DateTime.UtcNow;
                                existingCustomerAttachment.CreatedBy = model.CreatedBy;
                                existingCustomerAttachment.ModifiedBy = model.ModifiedBy;
                                existingCustomerAttachment.IsDeleted = false;
                                await _attachmentService.UpdateCustomerAttachment(existingCustomerAttachment);
                            }
                            else
                            {
                                var customerAttachment = new CustomerAttachment();
                                customerAttachment.CustomerId = existingCustomer.Id;
                                customerAttachment.AttachmentId = attachment.Id;
                                customerAttachment.MSA = false;
                                customerAttachment.NDA = true;
                                customerAttachment.CreatedOn = DateTime.UtcNow;
                                customerAttachment.ModifiedOn = DateTime.UtcNow;
                                customerAttachment.CreatedBy = model.CreatedBy;
                                customerAttachment.ModifiedBy = model.ModifiedBy;
                                customerAttachment.IsDeleted = false;
                                await _attachmentService.InsertCustomerAttachment(customerAttachment);
                            }
                        }
                    }
                    else
                    {
                        //var existingCustomerAttachment = await _attachmentService.GetCustomerAttachmentByCustomerId(existingCustomer.Id, msa: false, nda: true);
                        //foreach (var item in existingCustomerAttachment)
                        //{
                        //    await _attachmentService.DeleteCustomerAttachment(item);
                        //}
                    }
                }

                return (true, "Customer updated successfully", existingCustomer.Id);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, 0);
            }
        }

        /// <summary>
        ///  Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteCustomer(int id)
        {
            if (id > 0)
            {
                // Check if the customer's ID exists in the GetAllOrders method
                var orders = await _ordersService.GetAllOrders();
                if (orders.Any(order => order.CustomerId == id && order.IsDeleted == false))
                {
                    return (false, ConstantValues.CustomerOrderExist);
                }

                var customer = await _customerService.GetCustomerById(id);
                if (customer != null)
                {
                    customer.ModifiedOn = DateTime.UtcNow;
                    customer.IsDeleted = true;
                    await _customerService.UpdateCustomer(customer);
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

        /// <summary>
        /// Get Customer Attachment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IList<CustomerAttachmentModel>> GetCustomerAttachment(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            try
            {
                if (customer != null)
                {
                    IList<CustomerAttachmentModel> customerAttachmentListModel = new List<CustomerAttachmentModel>();
                    var customerAttachment = await _attachmentService.GetCustomerAttachmentByCustomerId(customer.Id, false, false);
                    foreach (var item in customerAttachment)
                    {
                        var attachment = await _attachmentService.GetAttachmentById(item.AttachmentId);
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

                                CustomerAttachmentModel customerAttachmentModel = new CustomerAttachmentModel();
                                customerAttachmentModel.FileName = attachment.FileName;
                                customerAttachmentModel.FilePath = completeUrl;
                                customerAttachmentListModel.Add(customerAttachmentModel);
                            }
                        }
                    }
                    return customerAttachmentListModel;
                }
                throw new Exception("No record found.");
            }
            catch (Exception)
            {
                throw new Exception(ConstantValues.RecordNotFound);
            };
        }
    }
}
#endregion Customers
