﻿using FluentMigrator;
using Tm.Core.Domain.Security;

namespace Tm.Data.Migrations.Indexes
{
    [TmMigration("2020/03/13 11:35:09:1647929")]
    public class AddAclRecordEntityIdEntityNameIX : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_AclRecord_EntityId_EntityName").OnTable(nameof(AclRecord))
                .OnColumn(nameof(AclRecord.EntityId)).Ascending()
                .OnColumn(nameof(AclRecord.EntityName)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}