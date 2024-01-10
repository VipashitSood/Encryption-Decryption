using Tm.Core.Domain.Customers;
using Tm.Services.Caching;

namespace Tm.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer attribute cache event consumer
    /// </summary>
    public partial class CustomerAttributeCacheEventConsumer : CacheEventConsumer<CustomerAttribute>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerAttribute entity)
        {
            Remove(TmCustomerServicesDefaults.CustomerAttributesAllCacheKey);
            Remove(_cacheKeyService.PrepareKey(TmCustomerServicesDefaults.CustomerAttributeValuesAllCacheKey, entity));
        }
    }
}
