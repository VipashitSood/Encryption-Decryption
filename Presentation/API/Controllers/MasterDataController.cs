using API.Factories.MasterData;
using API.Helpers;
using API.Models.BaseModels;
using API.Models.MasterData;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tm.Core.Configuration;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Services.Localization;
using Tm.Services.Pms.MasterData;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : BaseAPIController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly TmConfig _tmConfig;
        private IMasterDataFactory _masterDataFactory;
        private IMasterDataService _masterDataService;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Ctor
        public MasterDataController(ILocalizationService localizationService,
            TmConfig tmConfig,
            IMasterDataFactory masterDataFactory,
            IMasterDataService masterDataService,
            IWebHostEnvironment env)
        {
            _localizationService = localizationService;
            _tmConfig = tmConfig;
            _masterDataFactory = masterDataFactory;
            _masterDataService = masterDataService;
            _env = env;
        }
        #endregion

        #region Methods 

        #region Project Name

        //Get All Project Name i.e List
        [Route("GetProjectName")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectName(int pageIndex, int pageSize)
        {
            try
            {
                // Get project name by id
                var projectName = await _masterDataFactory.GetAllProjectName();
                if (projectName == null || !projectName.Any())
                {
                    // If projectName is null or empty
                    return ErrorResponse("No project names found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = projectName.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = projectName.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectName);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetProjectNameById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectNameById(int id)
        {
            try
            {
                //get project name by id
                var projectName = await _masterDataFactory.GetProjectNameById(id);

                //return error if not found
                if (projectName == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectName);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Project Name
        [Route("SaveProjectName")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateProjectName([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var projectName = await _masterDataFactory.InsertUpdateProjectName(model);
                if (!projectName.Item1)
                    return ErrorResponse(projectName.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectName.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Project Name
        [Route("projectNameId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteProjectName(int projectNameId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get project name by id
            try
            {
                var projectName = await _masterDataFactory.DeleteProjectName(projectNameId);
                if (!projectName.Item1)
                    return ErrorResponse(projectName.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), projectName.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Project Name

        #region Project Status

        //Get All Project Status i.e List
        [Route("GetProjectStatus")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectStatus(int pageIndex, int pageSize)
        {
            try
            {
                // Get projectStatus by id
                var projectStatus = await _masterDataFactory.GetAllProjectStatus(pageIndex, pageSize);
                if (projectStatus == null || !projectStatus.Any())
                {
                    // If project status is null or empty
                    return ErrorResponse("No project status found.", HttpStatusCode.NotFound);
                }
                var singleRecord = projectStatus.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null)
                {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages = Convert.ToInt32(singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = projectStatus
                };
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Project status By Id
        [Route("GetProjectStatusById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectStatusById(int id)
        {
            try
            {
                //get Project Status by id
                var projectStatus = await _masterDataFactory.GetProjectStatusById(id);

                //return error if not found
                if (projectStatus == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectStatus);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Project Status
        [Route("SaveProjectStatus")]
        [HttpPost]
        public async Task<BaseResponseModel> InsProjectStatus([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var projectStatus = await _masterDataFactory.InsertProjectStatus(model);
                if (!projectStatus.Item1)
                    return ErrorResponse(projectStatus.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectStatus.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Project Status
        [Route("projectStatusId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteProjectStatus(int projectStatusId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get project status by id
            try
            {
                var projectStatus = await _masterDataFactory.DeleteProjectStatus(projectStatusId);
                if (!projectStatus.Item1)
                    return ErrorResponse(projectStatus.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), projectStatus.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }
        #endregion Project Status

        #region Project Type

        //Get All Project Types i.e List
        [Route("GetProjectType")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectType(int pageIndex, int pageSize)
        {
            try
            {
                // Get project type by id
                var projectType = await _masterDataFactory.GetAllProjectType(pageIndex, pageSize);
                if (projectType == null || !projectType.Any())
                {
                    // If project type is null or empty
                    return ErrorResponse("No project type found.", HttpStatusCode.NotFound);
                }
                var singleRecord = projectType.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null)
                {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages = Convert.ToInt32(singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = projectType
                };
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Project Type By Id
        [Route("GetProjectTypeById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectTypeById(int id)
        {
            try
            {
                //get project type by id
                var projectType = await _masterDataFactory.GetProjectTypeById(id);

                //return error if not found
                if (projectType == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectType);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Project Type
        [Route("SaveProjectType")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateProjectType([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var projectType = await _masterDataFactory.InsertUpdateProjectType(model);
                if (!projectType.Item1)
                    return ErrorResponse(projectType.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectType.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        // Delete ProjectType
        [Route("projectTypeId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteProjectType(int projectTypeId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get project type by id
            try
            {
                var projectType = await _masterDataFactory.DeleteProjectType(projectTypeId);
                if (!projectType.Item1)
                    return ErrorResponse(projectType.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), projectType.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Project Type

        #region Currency

        //Get All Currency i.e List
        [Route("GetCurrency")]
        [HttpGet]
        public async Task<BaseResponseModel> Currency(int pageIndex, int pageSize)
        {
            try
            {
                // Get currency by id
                var currency = await _masterDataFactory.GetAllCurrency(pageIndex, pageSize);
                if (currency == null || !currency.Any())
                {
                    // If currency is null or empty
                    return ErrorResponse("No currency found.", HttpStatusCode.NotFound);
                }
                var singleRecord = currency.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null)
                {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages = Convert.ToInt32(singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = currency
                };
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Currency By Id
        [Route("GetCurrencyById")]
        [HttpGet]
        public async Task<BaseResponseModel> CurrencyById(int id)
        {
            try
            {
                //get currency by id
                var currency = await _masterDataFactory.GetCurrencyById(id);

                //return error if not found
                if (currency == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), currency);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Currency
        [Route("SaveCurrency")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateCurrency([FromForm] MasterDataCurrencyModel model)
        {
            try
            {
                if (model.AttachFiles == null && model.Id <= 0 )
                {
                    ModelState.AddModelError(string.Empty, "Currency image required!");
                    return ErrorResponse("Currency image required!", HttpStatusCode.NotFound);
                }
                //check of update case
                if (model.Id > 0 && model.AttachFiles == null)
                {
                    var currency = await _masterDataFactory.InsertUpdateCurrency(model);
                    if (!currency.Item1)
                        return ErrorResponse(currency.Item2, HttpStatusCode.NoContent);

                    return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), currency.Item2);
                }


                if (model.AttachFiles == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                // Save the uploaded file to disk or perform any necessary processing
                if (model.Id <= 0 || (model.AttachFiles != null && model.AttachFiles.Count > 0))
                {
                    var path = _tmConfig.CurrencyIconImagesFolderPath;
                    string filePath = string.Concat(_env.ContentRootPath, path);
                    foreach (var file in model.AttachFiles)
                    {
                        //Split the string by character . to get file extension type
                        int lastIndex = file.FileName.LastIndexOf('.');
                        if (lastIndex + 1 < file.FileName.Length)
                        {
                            string firstPart = file.FileName.Substring(0, lastIndex);
                            string secondPart = file.FileName.Substring(lastIndex + 1);

                            string newFileName = $"{firstPart}-{DateTime.UtcNow:yyyyMMdd_hhmmss}." + secondPart;

                            var finalPath = path.Replace("wwwroot\\", string.Empty) + newFileName;
                            model.CurrencyIcon = finalPath;
                            var data = await _masterDataFactory.InsertUpdateCurrency(model);
                            if (!data.Item1)
                                return ErrorResponse(data.Item2, HttpStatusCode.NotModified);
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            using (Stream stream = new FileStream(filePath +newFileName, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }

                }
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), "Image uploaded successfully.");

            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }

        }

        // Delete Currency
        [Route("currencyId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteCurrency(int currencyId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get currency by id
            try
            {
                var currency = await _masterDataFactory.DeleteCurrency(currencyId);
                if (!currency.Item1)
                    return ErrorResponse(currency.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), currency.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Currency

        #region Project Domain

        //Get Project Domain By Id
        [Route("GetProjectDomain")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllProjectDomain(int pageIndex, int pageSize, string projectName = "", string HODName = "")
        {
            try
            {
                // Get Customers by id
                var projectDomain = await _masterDataFactory.GetAllProjectDomain(pageIndex, pageSize, projectName, HODName);
                if (projectDomain == null || !projectDomain.Any())
                {
                    // If Customers is null or empty
                    return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
                }
                var singleRecord = projectDomain.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null)
                {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages = Convert.ToInt32(singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = projectDomain
                };

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get All Project Domain i.e List
        [Route("GetProjectDomainById")]
        [HttpGet]
        public async Task<BaseResponseModel> ProjectDomainById(int id)
        {
            try
            {
                //get project domain by id
                var projectDomain = await _masterDataFactory.GetProjectDomainById(id);

                //return error if not found
                if (projectDomain == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectDomain);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Project Domain
        [Route("SaveProjectDomain")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateProjectDomain([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var projectDomain = await _masterDataFactory.InsertUpdateProjectDomain(model);
                if (!projectDomain.Item1)
                    return ErrorResponse(projectDomain.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), projectDomain.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Project Domain
        [Route("projectDomainId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteProjectDomain(int projectDomainId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get project domain by id
            try
            {
                var projectDomain = await _masterDataFactory.DeleteProjectDomain(projectDomainId);
                if (!projectDomain.Item1)
                    return ErrorResponse(projectDomain.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), projectDomain.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Project Domain

        #region Tech Department

        //Get All Tech Department i.e List
        [Route("GetTechDepartment")]
        [HttpGet]
        public async Task<BaseResponseModel> TechDepartment(int pageIndex, int pageSize)
        {
            try
            {
                // Get tech depart by id
                var techDepart = await _masterDataFactory.GetAllTechDepartment();
                if (techDepart == null || !techDepart.Any())
                {
                    // If tech depart is null or empty
                    return ErrorResponse("No tech depart found.", HttpStatusCode.NotFound);
                }

                // Check for pagination
                if (pageIndex > 0 && pageSize >= 1)
                {
                    // Apply pagination
                    var totalItems = techDepart.Count();
                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    var results = techDepart.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                    // Create the response object
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

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techDepart);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Tech Department By Id
        [Route("GetTechDepartmentById")]
        [HttpGet]
        public async Task<BaseResponseModel> TechDepartmentById(int id)
        {
            try
            {
                //get project department by id
                var techDepartment = await _masterDataFactory.GetTechDepartmentById(id);

                //return error if not found
                if (techDepartment == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techDepartment);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Tech Department
        [Route("SaveTechDepartment")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateTechDepartment([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var techDepart = await _masterDataFactory.InsertUpdateTechDepartment(model);
                if (!techDepart.Item1)
                    return ErrorResponse(techDepart.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techDepart.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Tech Department
        [Route("techDepartId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteTechDepart(int techDepartId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get tech depart by id
            try
            {
                var techDepart = await _masterDataFactory.DeleteTechDepartment(techDepartId);
                if (!techDepart.Item1)
                    return ErrorResponse(techDepart.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), techDepart.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Tech Department

        #region Tech Stack

        //Get All Tech Stack i.e List
        [Route("GetTechStack")]
        [HttpGet]
        public async Task<BaseResponseModel> TechStack(int pageIndex, int pageSize)
        {
            try
            {
                // Get tech stack by id
                var techStack = await _masterDataFactory.GetAllTechStack(pageIndex, pageSize);
                if (techStack == null || !techStack.Any())
                {
                    // If tech stack is null or empty
                    return ErrorResponse("No tech stack found.", HttpStatusCode.NotFound);
                }
                var singleRecord = techStack.FirstOrDefault();
                int TotalItems = 0;
                int TotalPages = 0;
                if (singleRecord != null)
                {
                    TotalItems = singleRecord.TotalCount;
                    TotalPages = Convert.ToInt32(singleRecord.TotalCount / pageSize);
                }
                var response = new
                {
                    TotalItems = TotalItems,
                    TotalPages = TotalPages,
                    Page = pageIndex,
                    PageSize = pageSize,
                    Results = techStack
                };
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }


        //Get Tech Stack By Id
        [Route("GetTechStackById")]
        [HttpGet]
        public async Task<BaseResponseModel> TechStackById(int id)
        {
            try
            {
                //get project stack by id
                var techStack = await _masterDataFactory.GetTechStackById(id);

                //return error if not found
                if (techStack == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techStack);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NoContent);
            }
        }

        [Route("GetTechStackMappingByDeptId")]
        [HttpGet]
        public async Task<BaseResponseModel> GetTechStackMappingByDeptId()
        {
            try
            {
                //get project stack by id
                var techStack = await _masterDataFactory.GetTechStackMappingByDeptId();

                //return error if not found
                if (techStack == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techStack);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NoContent);
            }
        }

        //Insert & Update Tech Stack
        [Route("SaveTechStack")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateTechStack([FromBody] MasterDataInputStackModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
            try
            {
                var techStack = await _masterDataFactory.InsertUpdateTechStack(model);
                if (!techStack.Item1)
                    return ErrorResponse(techStack.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), techStack.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Delete Tech Stack
        [Route("techStackId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteTechStack(int techStackId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            //get tech Stack by id
            try
            {
                var techStack = await _masterDataFactory.DeleteTechStack(techStackId);
                if (!techStack.Item1)
                    return ErrorResponse(techStack.Item2, HttpStatusCode.NoContent);

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), techStack.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Tech Stack

        #region Communication Mode

        //Get All Communication Mode i.e List
        [Route("GetCommunicationMode")]
        [HttpGet]
        public async Task<BaseResponseModel> CommunicationModeAsync()
        {
            try
            {
                // Get communication mode by id
                var communicationMode = await _masterDataFactory.GetAllCommunicationModeAsync();

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), communicationMode);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Get Communication Mode By Id
        [Route("GetCommunicationModeById")]
        [HttpGet]
        public async Task<BaseResponseModel> CommunicationModeByIdAsync(int id)
        {
            try
            {
                // Get communication mode by id
                var communicationMode = await _masterDataFactory.GetCommunicationModeByIdAsync(id);

                // Return error if not found
                if (communicationMode == null)
                    return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), communicationMode);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        //Insert & Update Communication Mode
        [Route("SaveCommunicationMode")]
        [HttpPost]
        public async Task<BaseResponseModel> InsertUpdateCommunicationModeAsync([FromBody] MasterDataInputModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));

            try
            {
                var communicationMode = await _masterDataFactory.InsertUpdateAsync(model);
                if (!communicationMode.Item1)
                    return ErrorResponse(communicationMode.Item2, HttpStatusCode.NoContent);

                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), communicationMode.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.InternalServerError);
            }
        }

        // Delete Communication Mode
        [Route("communicationModeId")]
        [HttpDelete]
        public async Task<BaseResponseModel> DeleteCommunicationModeAsync(int communicationModeId)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            // Get communication mode by id
            try
            {
                var communicationMode = await _masterDataFactory.DeleteCommunicationModeAsync(communicationModeId);
                if (!communicationMode.Item1)
                    return ErrorResponse(communicationMode.Item2, HttpStatusCode.NoContent);

                // Return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"), communicationMode.Item2);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }

        #endregion Communication Mode
        #region ResourceType

        [Route("GetResourceTypeList")]
        [HttpGet]
        public async Task<BaseResponseModel> GetAllResourceTypeListAsync()
        {
            // Get All list 
            try
            {
                var resourceType = await _masterDataFactory.GetAllResourceTypeListAsync();

                // Return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), resourceType);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, HttpStatusCode.NotFound);
            }
        }
        #endregion
        #endregion Methods


    }
}
