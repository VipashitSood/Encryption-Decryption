﻿using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Directory;

namespace Tm.Data.Mapping.Builders.Directory
{
    /// <summary>
    /// Represents a country entity builder
    /// </summary>
    public partial class CountryBuilder : TmEntityBuilder<Country>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table) => table
            .WithColumn(nameof(Country.Name)).AsString(50).NotNullable()
            .WithColumn(nameof(Country.TwoLetterIsoCode)).AsString(2).Nullable()
            .WithColumn(nameof(Country.ThreeLetterIsoCode)).AsString(3).Nullable()
            .WithColumn(nameof(Country.NumericIsoCode)).AsInt32().NotNullable();

        #endregion
    }
}