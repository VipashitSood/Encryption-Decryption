﻿using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Messages;

namespace Tm.Data.Mapping.Builders.Messages
{
    /// <summary>
    /// Represents an email account entity builder
    /// </summary>
    public partial class EmailAccountBuilder : TmEntityBuilder<EmailAccount>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(EmailAccount.DisplayName)).AsString(255).Nullable()
                .WithColumn(nameof(EmailAccount.Email)).AsString(255).NotNullable()
                .WithColumn(nameof(EmailAccount.Host)).AsString(255).NotNullable()
                .WithColumn(nameof(EmailAccount.Username)).AsString(255).NotNullable()
                .WithColumn(nameof(EmailAccount.Password)).AsString(255).NotNullable();
        }

        #endregion
    }
}