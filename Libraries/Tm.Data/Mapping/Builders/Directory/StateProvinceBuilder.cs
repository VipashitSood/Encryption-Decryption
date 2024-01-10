using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Directory;
using Tm.Data.Extensions;

namespace Tm.Data.Mapping.Builders.Directory
{
    /// <summary>
    /// Represents a state and province entity builder
    /// </summary>
    public partial class StateProvinceBuilder : TmEntityBuilder<StateProvince>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(StateProvince.Name)).AsString(50).NotNullable()
                .WithColumn(nameof(StateProvince.Abbreviation)).AsString(10).Nullable()
                .WithColumn(nameof(StateProvince.CountryId)).AsInt32().ForeignKey<Country>();
        }

        #endregion
    }
}