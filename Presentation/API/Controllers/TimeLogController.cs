using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using System;
using Tm.Core.Configuration;
using API.Controllers;
using API.Factories.MasterData;
using Microsoft.AspNetCore.Hosting;
using Tm.Services.Customers;
using Tm.Services.Localization;
using Tm.Services.Pms.MasterData;
using System.Linq;
using Tm.Core.Constants;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using API.Models.AppSetting;
using Microsoft.Extensions.Options;
using API.Auth;
using API.Helpers;
using DocumentFormat.OpenXml.EMMA;
using API.Factories.ProjectDetail;

[Route("api/[controller]")]
[ApiController]
public class TimeLogController : BaseAPIController
{
    #region Fields
    private readonly ILocalizationService _localizationService;
    private readonly TmConfig _tmConfig;
    private IMasterDataFactory _masterDataFactory;
    private IMasterDataService _masterDataService;
    private readonly IWebHostEnvironment _env;
    private readonly ICustomerService _customerService;
    private readonly IProjectsFactory _projectFactory;
    private readonly AD _ADconfig;
    #endregion

    #region Ctor

    private readonly TokenCreator _tokenCreator;

    public TimeLogController(
    ILocalizationService localizationService,
    TmConfig tmConfig,
    IMasterDataFactory masterDataFactory,
    IMasterDataService masterDataService,
    IWebHostEnvironment env,
    ICustomerService customerService,
    TokenCreator tokenCreator,
    IOptions<AD> ADconfig,
    IProjectsFactory projectFactory) 
    {
        _ADconfig = ADconfig.Value;
        _tokenCreator = tokenCreator;
        _localizationService = localizationService;
        _tmConfig = tmConfig;
        _masterDataFactory = masterDataFactory;
        _masterDataService = masterDataService;
        _env = env;
        _customerService = customerService;
        _projectFactory = projectFactory;
    }
    #endregion

    #region AD TimeLogs

    [NonAction]
    [HttpGet]
    [Route("UpsertAllADTimeLogs")]
    public async Task<BaseResponseModel> UpsertAllADTimeLogs()
    {
        try
        {
            var clientDetail = await _projectFactory.UpsertAllADTimeLogs();
            if (!clientDetail.Item1)
                return ErrorResponse(clientDetail.Item2, HttpStatusCode.NoContent);

            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"));
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex, HttpStatusCode.InternalServerError);
        }
    }

    #endregion TimeLogs

}



 