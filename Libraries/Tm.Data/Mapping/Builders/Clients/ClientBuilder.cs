using FluentMigrator.Builders.Create.Table;
using Tm.Core.Domain.Clients;
using Tm.Core.Domain.Common;
using Tm.Data.Extensions;

namespace Tm.Data.Mapping.Builders.Clients
{
    /// <summary>
    /// Represents a client entity builder
    /// </summary>
    public partial class ClientBuilder : TmEntityBuilder<Client>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Client.Name)).AsString(200).NotNullable()
                .WithColumn(nameof(Client.AddressId)).AsInt32().ForeignKey<Address>()
                .WithColumn(nameof(Client.Email)).AsString(50).NotNullable();
        }

        #endregion
    }
}