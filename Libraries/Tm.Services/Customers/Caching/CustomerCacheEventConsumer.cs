using Tm.Core.Domain.Customers;
using Tm.Services.Caching;
using Tm.Services.Events;
//using Tm.Services.Orders;

namespace Tm.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer cache event consumer
    /// </summary>
    public partial class CustomerCacheEventConsumer : CacheEventConsumer<Customer>, IConsumer<CustomerPasswordChangedEvent>
    {
        #region Methods

        /// <summary>
        /// Handle password changed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(CustomerPasswordChangedEvent eventMessage)
        {
            Remove(_cacheKeyService.PrepareKey(TmCustomerServicesDefaults.CustomerPasswordLifetimeCacheKey, eventMessage.Password.CustomerId));
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Customer entity)
        {
            RemoveByPrefix(TmCustomerServicesDefaults.CustomerCustomerRolesPrefixCacheKey);
            RemoveByPrefix(TmCustomerServicesDefaults.CustomerAddressesPrefixCacheKey);
            //RemoveByPrefix(TmOrderDefaults.ShoppingCartPrefixCacheKey);
        }

        #endregion
    }
}