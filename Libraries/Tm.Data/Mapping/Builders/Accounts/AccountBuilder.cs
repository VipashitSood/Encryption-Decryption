using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Accounts;

namespace Tm.Data.Mapping.Builders.Accounts
{
    /// <summary>
    /// Represents a account entity builder
    /// </summary>
    public partial class AccountBuilder : TmEntityBuilder<Account>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Account.AccountNo)).AsString(100).NotNullable()
                .WithColumn(nameof(Account.AccountHolderName)).AsString(500).NotNullable();
        }

        #endregion
    }
}