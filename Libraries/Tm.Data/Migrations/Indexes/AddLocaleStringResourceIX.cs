﻿using FluentMigrator;
using Tm.Core.Domain.Localization;

namespace Tm.Data.Migrations.Indexes
{
    [TmMigration("2020/03/13 09:36:08:9037677")]
    public class AddLocaleStringResourceIX : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_LocaleStringResource").OnTable(nameof(LocaleStringResource))
                .OnColumn(nameof(LocaleStringResource.ResourceName)).Ascending()
                .OnColumn(nameof(LocaleStringResource.LanguageId)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}
