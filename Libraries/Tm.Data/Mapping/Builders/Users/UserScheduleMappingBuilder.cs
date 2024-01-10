using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Schedules;
using Tm.Core.Domain.Users;
using Tm.Data.Extensions;

namespace Tm.Data.Mapping.Builders.Users
{
    /// <summary>
    /// Represents a user schedule mapping entity builder
    /// </summary>
    public partial class UserScheduleMappingBuilder : TmEntityBuilder<UserScheduleMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserScheduleMapping), nameof(UserScheduleMapping.UserId)))
                    .AsInt32().ForeignKey<User>()
                .WithColumn(NameCompatibilityManager.GetColumnName(typeof(UserScheduleMapping), nameof(UserScheduleMapping.ScheduleId)))
                    .AsInt32().ForeignKey<Schedule>();
        }

        #endregion
    }
}