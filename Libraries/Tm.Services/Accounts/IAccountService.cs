using System.Collections.Generic;
using Tm.Core.Domain.Accounts;

namespace Tm.Services.Accounts
{
    /// <summary>
    /// Account service interface
    /// </summary>
    public partial interface IAccountService
    {
        /// <summary>
        /// Deletes an account
        /// </summary>
        /// <param name="account">Account</param>
        void DeleteAccount(Account account);

        /// <summary>
        /// Gets an account by account identifier
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <returns>Account</returns>
        Account GetAccountById(int accountId);

        /// <summary>
        /// Gets all accounts
        /// </summary>
        /// <returns>Accounts list</returns>
        List<Account> GetAllAccounts();

        /// <summary>
        /// Inserts an account
        /// </summary>
        /// <param name="account">Account</param>
        void InsertAccount(Account account);

        /// <summary>
        /// Updates the account
        /// </summary>
        /// <param name="account">Account</param>
        void UpdateAccount(Account account);
    }
}