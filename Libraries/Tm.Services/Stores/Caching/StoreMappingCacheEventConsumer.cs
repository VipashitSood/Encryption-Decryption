using Tm.Core.Domain.Stores;
using Tm.Services.Caching;

namespace Tm.Services.Stores.Caching
{
    /// <summary>
    /// Represents a store mapping cache event consumer
    /// </summary>
    public partial class StoreMappingCacheEventConsumer : CacheEventConsumer<StoreMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(StoreMapping entity)
        {
            var entityId = entity.EntityId;
            var entityName = entity.EntityName;

            var key = _cacheKeyService.PrepareKey(TmStoreDefaults.StoreMappingsByEntityIdNameCacheKey, entityId, entityName);

            Remove(key);

            key = _cacheKeyService.PrepareKey(TmStoreDefaults.StoreMappingIdsByEntityIdNameCacheKey, entityId, entityName);
            
            Remove(key);
        }
    }
}
