using Tm.Core.Domain.Customers;
using Tm.Services.Caching;

namespace Tm.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer customer role mapping cache event consumer
    /// </summary>
    public partial class CustomerCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<CustomerCustomerRoleMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerCustomerRoleMapping entity)
        {
            RemoveByPrefix(TmCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
        }
    }
}