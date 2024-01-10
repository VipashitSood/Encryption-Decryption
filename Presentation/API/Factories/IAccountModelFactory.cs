using API.Models.Accounts;
using System.Collections.Generic;
using Tm.Core.Domain.Accounts;

namespace API.Factories
{
    /// <summary>
    /// Represents the interface of the account model factory
    /// </summary>
    public partial interface IAccountModelFactory
    {
        /// <summary>
        /// Prepare account model
        /// </summary>
        /// <param name="account">Account entity</param>
        /// <returns>Account model</returns>
        AccountModel PrepareAccountModel(Account account);

        /// <summary>
        /// Prepare account models list
        /// </summary>
        /// <param name="accounts">Account entity list</param>
        /// <returns>Account models list</returns>
        List<AccountModel> PrepareAccountModelList(List<Account> accounts);
    }
}
