using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Directory;
using Tm.Data.Extensions;

namespace Tm.Data.Mapping.Builders.Common
{
    /// <summary>
    /// Represents a address entity builder
    /// </summary>
    public partial class AddressBuilder : TmEntityBuilder<Address>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Address.CountryId)).AsInt32().NotNullable().ForeignKey<Country>()
                .WithColumn(nameof(Address.StateProvinceId)).AsInt32().NotNullable().ForeignKey<StateProvince>()
                .WithColumn(nameof(Address.City)).AsString(50).NotNullable();
        }

        #endregion
    }
}