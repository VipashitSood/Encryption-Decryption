using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Clients;
using Tm.Core.Domain.Schedules;
using Tm.Core.Domain.Users;
using Tm.Data.Extensions;

namespace Tm.Data.Mapping.Builders.Schedules
{
    /// <summary>
    /// Represents a schedule builder
    /// </summary>
    public partial class ScheduleBuilder : TmEntityBuilder<Schedule>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table) => table
            .WithColumn(nameof(Schedule.Name)).AsString(50).NotNullable()
            .WithColumn(nameof(Schedule.CreatedBy)).AsInt32().NotNullable().ForeignKey<User>()
            .WithColumn(nameof(Schedule.ClientId)).AsInt32().NotNullable().ForeignKey<Client>();

        #endregion
    }
}