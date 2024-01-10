using API.Infrastructure.Mapper.Extensions;
using API.Models.Accounts;
using System.Collections.Generic;
using System.Linq;
using Tm.Core.Domain.Accounts;
using Tm.Services.Accounts;

namespace API.Factories
{
    /// <summary>
    /// Represents the account model factory
    /// </summary>
    public partial class AccountModelFactory : IAccountModelFactory
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Ctor

        public AccountModelFactory(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare account model
        /// </summary>
        /// <param name="account">Account entity</param>
        /// <returns>Account model</returns>
        public virtual AccountModel PrepareAccountModel(Account account)
        {
            if (account == null)
                return null;

            return account.ToModel<AccountModel>();
        }

        /// <summary>
        /// Prepare account models list
        /// </summary>
        /// <param name="accounts">Account entity list</param>
        /// <returns>Account models list</returns>
        public virtual List<AccountModel> PrepareAccountModelList(List<Account> accounts)
        {
            if (accounts == null)
                return null;

            return accounts.Select(account => account.ToModel<AccountModel>()).ToList();
        }

        #endregion
    }
}