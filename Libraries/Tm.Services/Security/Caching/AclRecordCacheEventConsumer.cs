using Tm.Core.Domain.Security;
using Tm.Services.Caching;

namespace Tm.Services.Security.Caching
{
    /// <summary>
    /// Represents a ACL record cache event consumer
    /// </summary>
    public partial class AclRecordCacheEventConsumer : CacheEventConsumer<AclRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(AclRecord entity)
        {
            var cacheKey = _cacheKeyService.PrepareKey(TmSecurityDefaults.AclRecordByEntityIdNameCacheKey, entity.EntityId, entity.EntityName);
            Remove(cacheKey);
        }
    }
}
