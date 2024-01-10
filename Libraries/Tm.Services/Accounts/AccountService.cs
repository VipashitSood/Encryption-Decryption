using System;
using System.Collections.Generic;
using System.Linq;
using Tm.Core.Domain.Accounts;
using Tm.Data;

namespace Tm.Services.Accounts
{
    /// <summary>
    /// Account service
    /// </summary>
    public partial class AccountService : IAccountService
    {
        #region Fields

        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Ctor

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an account
        /// </summary>
        /// <param name="account">Account</param>
        public virtual void DeleteAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            _accountRepository.Delete(account);
        }

        /// <summary>
        /// Gets an account by account identifier 
        /// </summary>
        /// <param name="accountId">Account identifier</param>
        /// <returns>Account</returns>
        public virtual Account GetAccountById(int accountId)
        {
            if (accountId == 0)
                return null;

            return _accountRepository.Table.FirstOrDefault(a => a.Id.Equals(accountId));
        }

        /// <summary>
        /// Gets all accounts
        /// </summary>
        /// <returns>Accounts list</returns>
        public virtual List<Account> GetAllAccounts()
        {
            return _accountRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts an account
        /// </summary>
        /// <param name="account">Account</param>
        public virtual void InsertAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            account.CreatedOnUtc = DateTime.UtcNow;

            _accountRepository.Insert(account);
        }

        /// <summary>
        /// Updates the account
        /// </summary>
        /// <param name="account">Account</param>
        public virtual void UpdateAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            _accountRepository.Update(account);
        }

        #endregion
    }
}