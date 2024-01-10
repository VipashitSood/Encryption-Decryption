using Tm.Core.Domain.Customers;
using Tm.Services.Caching;

namespace Tm.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer attribute value cache event consumer
    /// </summary>
    public partial class CustomerAttributeValueCacheEventConsumer : CacheEventConsumer<CustomerAttributeValue>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(CustomerAttributeValue entity)
        {
            Remove(TmCustomerServicesDefaults.CustomerAttributesAllCacheKey);
            Remove(_cacheKeyService.PrepareKey(TmCustomerServicesDefaults.CustomerAttributeValuesAllCacheKey, entity.CustomerAttributeId));
        }
    }
}