using API.Helpers;
using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using Tm.Core.Configuration;
using API.Controllers;
using API.Factories.MasterData;
using API.Factories.ProjectDetail;
using Microsoft.AspNetCore.Hosting;
using Tm.Services.Customers;
using Tm.Services.Localization;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.UserRole;
using System.Linq;
using APDetails = API.Models.UserRole;
using API.Factories.UserRole;
using Tm.Core.Constants;
using API.Models.UserRole;
using System.Collections.Generic;
using Tm.Core.Domain.Pms.UserRole;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using API.Models.AppSetting;
using Microsoft.Extensions.Options;
using API.Auth;
using DocumentFormat.OpenXml.Spreadsheet;

[Route("api/[controller]")]
[ApiController]
public class UserRoleController : BaseAPIController
{
    #region Fields
    private readonly ILocalizationService _localizationService;
    private IUserRoleFactory _userRoleFactory;
    private readonly TmConfig _tmConfig;
    private IMasterDataFactory _masterDataFactory;
    private IMasterDataService _masterDataService;
    private IUserRoleFactory userRoleFactory;
    private readonly IWebHostEnvironment _env;
    private IUserRoleService _userRoleService;
    private readonly ICustomerService _customerService;
    private readonly AD _ADconfig;
    #endregion

    #region Ctor

    private readonly TokenCreator _tokenCreator;

    public UserRoleController(
    ILocalizationService localizationService,
    IUserRoleFactory userRoleFactory,
    TmConfig tmConfig,
    IMasterDataFactory masterDataFactory,
    IMasterDataService masterDataService,
    IWebHostEnvironment env,
    IUserRoleService userRoleService,
    ICustomerService customerService,
    TokenCreator tokenCreator,
    IOptions<AD> ADconfig) 
    {
        _ADconfig = ADconfig.Value;
        _tokenCreator = tokenCreator;
        _localizationService = localizationService;
        _userRoleFactory = userRoleFactory;
        _tmConfig = tmConfig;
        _masterDataFactory = masterDataFactory;
        _masterDataService = masterDataService;
        _env = env;
        _userRoleService = userRoleService;
        _customerService = customerService;
    }
    #endregion


    #region UserRoles

    //Get All User Roles i.e List
    [Route("GetAllUserRole")]
    [HttpGet]
    public async Task<BaseResponseModel> GetAllUserRole(int pageIndex, int pageSize)
    {
        try
        {
            // Get UserRole by id
            var userRole = await _userRoleFactory.GetAllUserRoles();
            if (userRole == null || !userRole.Any())
            {
                // If UserRole is null or empty
                return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
            }

            // Check for pagination
            if (pageIndex > 0 && pageSize >= 1)
            {
                // Apply pagination
                var totalItems = userRole.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                var results = userRole.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }


    [HttpGet]
    [Route("GetAllUsersByAD")]
    public async Task<BaseResponseModel> GetAllUsersByAD(string token)
    {
        try
        {
            // Make sure _ADconfig is properly initialized
            if (_ADconfig == null)
            {
                return ErrorResponse("AD configuration is not properly initialized.", HttpStatusCode.InternalServerError);
            }

            string jsonResponse = "";
            string endPoint = _ADconfig.AllUserEndPoint;
            var query = new Dictionary<string, string>()
            {
                [ConstantValues.AdCount] = _ADconfig.Count
            };
            var uri = QueryHelpers.AddQueryString(endPoint, query);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue(ConstantValues.Bearer, token);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
            }
            var res = JsonSerializer.Deserialize<Root>(jsonResponse);
            //open new background task to update AD data into table
            Task.Run(async () =>
            {
                //save ad user into db for internal purposes.
                foreach (var responseADUser in res.value)
                {
                    if (responseADUser.mail != null && responseADUser.mail.Contains("tmotions.com"))
                    {
                        var adUserModel = new ADUserModel
                        {
                            UserId = responseADUser.id,
                            Name = responseADUser.givenName,
                            LastName = responseADUser.surname,
                            DisplayName = responseADUser.displayName,
                            Email = responseADUser.mail,
                            JobTitle = responseADUser.jobTitle,
                            Mobile = responseADUser.mobilePhone,
                        };
                        await _userRoleFactory.SaveADUserAsync(adUserModel);
                    }
                }

                //delete AdUser from db if not found in response 
                var allADUserIds = (await _userRoleService.GetAllUserByAD())?.
                    Select(adUser => adUser.UserId)?.ToList();

                //select userIds need to delete
                var adUsersNeedToDelete = allADUserIds.FindAll(adUserId => res.value.Select(x => x.id).Contains(adUserId) == false);
                foreach (var adUserId in adUsersNeedToDelete)
                {
                    //delete ad user from table
                    var user = await _userRoleService.GetADUserByADUserId(adUserId);
                    if (user != null)
                        await _userRoleService.DeleteADUser(user);
                }
            });

            return new BaseResponseModel() { StatusCode = 200, Data = res, Message = ConstantValues.Success };
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("GetAllHODUsersByAD")]
    public async Task<BaseResponseModel> GetAllHODUsersByAD(string token)
    {
        try
        {
            // Make sure _ADconfig is properly initialized
            if (_ADconfig == null)
            {
                return ErrorResponse("AD configuration is not properly initialized.", HttpStatusCode.InternalServerError);
            }

            string jsonResponse = "";
            string endPoint = _ADconfig.AllUserEndPoint;
            var query = new Dictionary<string, string>()
            {
                [ConstantValues.AdCount] = _ADconfig.Count
            };
            var uri = QueryHelpers.AddQueryString(endPoint, query);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue(ConstantValues.Bearer, token);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
            }
            var res = JsonSerializer.Deserialize<Root>(jsonResponse);

            ////save ad user into db for internal purposes.
            //foreach (var responseADUser in res.value)
            //{
            //    var adUserModel = new ADUserModel
            //    {
            //        UserId = responseADUser.id,
            //        Name = responseADUser.givenName,
            //        Email = responseADUser.mail,
            //        JobTitle = responseADUser.jobTitle,
            //        Mobile = responseADUser.mobilePhone,
            //    };
            //    await _userRoleFactory.SaveADUserAsync(adUserModel);
            //}

            ////delete AdUser from db if not found in response 
            //var allADUserIds = (await _userRoleService.GetAllUserByAD())?.
            //    Select(adUser => adUser.UserId)?.ToList();

            ////select userIds need to delete
            //var adUsersNeedToDelete = allADUserIds.FindAll(adUserId => res.value.Select(x => x.id).Contains(adUserId) == false);
            //foreach (var adUserId in adUsersNeedToDelete)
            //{
            //    //delete ad user from table
            //    var user = await _userRoleService.GetADUserByADUserId(adUserId);
            //    if (user != null)
            //        await _userRoleService.DeleteADUser(user);
            //}
            var hodFilteredData = res.value.Where(x => x.jobTitle == "Head of Delivery").Select(x => new ADHODUserResponseModel()
            {
                Label = x.displayName,
                Value = x.id,
                Email = x.mail,
                Phone = x.mobilePhone
            }).ToList();
            return new BaseResponseModel() { StatusCode = 200, Data = hodFilteredData, Message = ConstantValues.Success };
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    //Get UserRole By Id
    [Route("GetUserRoleById")]
    [HttpGet]
    public async Task<BaseResponseModel> UserRoleById(int id)
    {
        try
        {
            //get UserRole by id
            var userRole = await _userRoleFactory.GetUserRoleById(id);

            //return error if not found
            if (userRole == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }
    // Delete UserRole
    [Route("DeleteUserRole")]
    [HttpDelete]
    public async Task<BaseResponseModel> DeleteUserRole(int Id)
    {
        if (!ModelState.IsValid)
            return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

        //get UserRole by id
        var userRole = await _userRoleService.GetUserRoleById(Id);

        //return error if not found
        if (userRole == null)
            return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

        try
        {
            var data = await _userRoleFactory.DeleteUserRoleAndMenuById(Id);
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

    //Insert  UserRole
    [Route("CreateUserRole")]
    [HttpPost]
    public async Task<BaseResponseModel> InsertUserRole([FromBody] APDetails.UserRolesModel model)
    {
        if (!ModelState.IsValid)
            return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
        try
        {
            var userRole = await _userRoleFactory.CreateUserRole(model);
            if (!userRole.Item1)
                return ErrorResponse(userRole.Item2, HttpStatusCode.NoContent);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole.Item3);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }
    //Update UserRole
    [Route("UpdateUserRole")]
    [HttpPut]
    public async Task<BaseResponseModel> UpdateUserRole([FromBody] APDetails.UserRolesModel model)
    {
        if (!ModelState.IsValid)
            return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));
        try
        {
            var userRole = await _userRoleFactory.UpdateUserRole(model);
            if (!userRole.Item1)
                return ErrorResponse(userRole.Item2, HttpStatusCode.NoContent);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRole.Item3);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    #endregion UserRoles

    //#region Role

    //[HttpPost]
    //[Route("SaveRole")]
    //public async Task<BaseResponseModel> SaveRole([FromBody] ADUserModel adModel)
    //{
    //    await _userRoleFactory.SaveRoleAsync(adModel);
    //    return new BaseResponseModel() { StatusCode = 200, Message = ConstantValues.Success };
    //}

    //#endregion

    #region UserRole mapping

    [Route("GetAllUserRoleMapping")]
    [HttpGet]
    public async Task<BaseResponseModel> GetAllUserRoleMapping(int roleId, string userId, int pageIndex, int pageSize)
    {
        try
        {
            var userRoleMapping = await _userRoleFactory.GetAllUserRoleMapping(roleId, userId);
            if (userRoleMapping == null)
            {
                // If UserRole is null or empty
                return ErrorResponse("Tm.API.NotFound.", HttpStatusCode.NotFound);
            }

            // Check for pagination
            if (pageIndex > 0 && pageSize >= 1)
            {
                // Apply pagination
                var totalItems = userRoleMapping.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                var results = userRoleMapping.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), userRoleMapping);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    [Route("SaveUserRoleMapping")]
    [HttpPost]
    public async Task<BaseResponseModel> AddUserRoleMapping([FromBody] ADUserRoleMappingModel model)
    {        
        if(model == null)
            return ErrorResponse(_localizationService.GetResource("Tm.API.SaveUserRoleMapping.Model.NullOrEmpty", model.LanguageId), HttpStatusCode.BadRequest);

        if (!ModelState.IsValid)
            return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel", model.LanguageId));

        try
        {
            var userRoleMapping = await _userRoleFactory.SaveUserRoleMapping(model);
            if (!userRoleMapping.Item1)
                return ErrorResponse(userRoleMapping.Item2, HttpStatusCode.NoContent);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    [Route("UpdateUserRoleMapping")]
    [HttpPut]
    public async Task<BaseResponseModel> UpdateUserRoleMapping([FromBody] List<ADUserRoleListModel> model)
    {
        if (model == null)
            return ErrorResponse(_localizationService.GetResource("Tm.API.SaveUserRoleMapping.Model.NullOrEmpty"), HttpStatusCode.BadRequest);

        if (!ModelState.IsValid)
            return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

        try
        {
            //get all user roles
            var allUserRoles = await _userRoleService.GetAllUserRoles();
            foreach (var aDUserRoleModel in model)
            {
                //get all role mapping of user by userId
                var userRoleMapping = await _userRoleService.GetUserRoleMappingById(aDUserRoleModel.Id);
                if (userRoleMapping != null)
                {
                    //get user all existing role Ids
                    var userRoleIds =  _userRoleService.GetUserRoleIdsByUserId(userRoleMapping.ADUserId);
                    foreach (var userRole in allUserRoles)
                    {
                        var userRoleModel = aDUserRoleModel.RoleModels.FirstOrDefault(rm => rm.Id == userRole.Id);
                        if (userRoleModel != null)
                        {
                            //new
                            if(userRoleIds.All(roleId=>roleId != userRole.Id))
                            {
                                var priority = _userRoleService.GetUserRoleById(userRoleModel.Id).Result;
                                var newUserRoleMapping = new ADUserRoleMapping
                                {
                                    ADUserId = userRoleMapping.ADUserId,
                                    UserRoleId = userRole.Id,
                                    CreatedOn = DateTime.UtcNow,
                                    IsDeleted = !userRoleModel.IsSelected
                                };

                                await _userRoleService.InsertUserRoleMapping(newUserRoleMapping);
                            }
                            else
                            {
                                //update
                                var existingUserRoleMapping = (await _userRoleService.GetAllUserRoleMapping(userRole.Id, userRoleMapping.ADUserId))?.FirstOrDefault();
                                if (existingUserRoleMapping != null)
                                {
                                    existingUserRoleMapping.IsDeleted = !userRoleModel.IsSelected;
                                    existingUserRoleMapping.ModifiedOn = DateTime.UtcNow;
                                    await _userRoleService.UpdateUserRoleMapping(existingUserRoleMapping);
                                }
                            }
                        }
                    }
                }
            }

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    #endregion
}



 