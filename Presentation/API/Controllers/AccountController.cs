using API.Factories;
using API.Infrastructure.Mapper.Extensions;
using API.Models.Accounts;
using API.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Tm.Core.Domain.Accounts;
using Tm.Services.Accounts;
using Tm.Services.Localization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AccountController : BaseAPIController
    {
        #region Fields

        private readonly IAccountService _accountService;
        private readonly ILocalizationService _localizationService;
        private readonly IAccountModelFactory _accountModelFactory;

        #endregion

        #region Ctor

        public AccountController(IAccountService accountService,
            ILocalizationService localizationService,
            IAccountModelFactory accountModelFactory)
        {
            _accountService = accountService;
            _localizationService = localizationService;
            _accountModelFactory = accountModelFactory;
        }

        #endregion

        #region Methods
        // GET: api/<AccountController>
        [HttpGet]
        public BaseResponseModel Get()
        {
            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), "Welcome");
            ////get accounts from service
            //var accounts = _accountService.GetAllAccounts();

            ////prepare model
            //var responseObj = _accountModelFactory.PrepareAccountModelList(accounts);

            //return success response
            // return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), responseObj);
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public BaseResponseModel Get(int id)
        {
            //get account by id
            var account = _accountService.GetAccountById(id);

            //return error if not found
            if (account == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //prepare model
            var responseObj = _accountModelFactory.PrepareAccountModel(account);

            //return success response
            return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), responseObj);
        }

        // POST api/<AccountController>
        [HttpPost]
        public BaseResponseModel Post([FromBody]AccountModel model)
        {
            if (!ModelState.IsValid)
                return InvalidateModelResponse(ModelState, _localizationService.GetResource("Tm.API.InvalidModel"));

            try
            {
                var account = new Account();
                if (model.Id > 0)
                {
                    account = _accountService.GetAccountById(model.Id);

                    //return error if not found
                    if (account == null)
                        return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

                    account.AccountNo = model.AccountNo;
                    account.AccountHolderName = model.AccountHolderName;
                    account.UpdatedOnUtc = DateTime.UtcNow;

                    //update account
                    _accountService.UpdateAccount(account);
                }
                else
                {
                    account = model.ToEntity(account);

                    //insert account
                    _accountService.InsertAccount(account);
                }

                //return success response
                return SuccessResponse(_localizationService.GetResource("Tm.API.Success"), account);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }


        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public BaseResponseModel Delete(int id)
        {
            //get account by id
            var account = _accountService.GetAccountById(id);

            //return error if not found
            if (account == null)
                return ErrorResponse(_localizationService.GetResource("Tm.API.NotFound"), HttpStatusCode.NotFound);

            //delete record
            _accountService.DeleteAccount(account);

            //return success response
            return SuccessResponse(_localizationService.GetResource("Tm.API.Success.Delete"));
        }
        #endregion
    }
}
