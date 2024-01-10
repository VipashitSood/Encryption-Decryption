using Tm.Core.Domain.Customers;
using Tm.Services.Caching;

namespace Tm.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer role cache event consumer
    /// </summary>
    public partial class CustomerRoleCacheEventConsumer : CacheEventConsumer<CustomerRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerRole entity)
        {
            RemoveByPrefix(TmCustomerServicesDefaults.CustomerRolesPrefixCacheKey);
            RemoveByPrefix(TmCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
        }
    }
}
