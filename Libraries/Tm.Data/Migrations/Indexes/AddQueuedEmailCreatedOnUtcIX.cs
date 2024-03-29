﻿using FluentMigrator;
using Tm.Core.Domain.Messages;

namespace Tm.Data.Migrations.Indexes
{
    [TmMigration("2020/03/13 09:36:08:9037687")]
    public class AddQueuedEmailCreatedOnUtcIX : AutoReversingMigration
    {
        #region Methods          

        public override void Up()
        {
            Create.Index("IX_QueuedEmail_CreatedOnUtc").OnTable(nameof(QueuedEmail))
                .OnColumn(nameof(QueuedEmail.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}