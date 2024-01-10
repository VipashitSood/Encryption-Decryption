using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.SessionTypes;

namespace Tm.Data.Mapping.Builders.SessionTypes
{
    /// <summary>
    /// Represents a session type entity builder
    /// </summary>
    public partial class SessionTypeBuilder : TmEntityBuilder<SessionType>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SessionType.Name)).AsString(50).NotNullable();
        }

        #endregion
    }
}