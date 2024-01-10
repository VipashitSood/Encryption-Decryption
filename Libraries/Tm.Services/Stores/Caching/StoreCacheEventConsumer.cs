using Tm.Core.Domain.Stores;
using Tm.Services.Caching;
using Tm.Services.Localization;
//using Tm.Services.Orders;

namespace Tm.Services.Stores.Caching
{
    /// <summary>
    /// Represents a store cache event consumer
    /// </summary>
    public partial class StoreCacheEventConsumer : CacheEventConsumer<Store>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Store entity)
        {
            Remove(TmStoreDefaults.StoresAllCacheKey);
            //RemoveByPrefix(TmOrderDefaults.ShoppingCartPrefixCacheKey);

            var prefix = _cacheKeyService.PrepareKeyPrefix(TmLocalizationDefaults.LanguagesByStoreIdPrefixCacheKey, entity);

            RemoveByPrefix(prefix);
        }
    }
}
