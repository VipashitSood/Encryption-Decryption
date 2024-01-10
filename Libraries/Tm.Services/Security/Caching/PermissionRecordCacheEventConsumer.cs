using Tm.Core.Domain.Security;
using Tm.Services.Caching;

namespace Tm.Services.Security.Caching
{
    /// <summary>
    /// Represents a permission record cache event consumer
    /// </summary>
    public partial class PermissionRecordCacheEventConsumer : CacheEventConsumer<PermissionRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(PermissionRecord entity)
        {
            var prefix = _cacheKeyService.PrepareKeyPrefix(TmSecurityDefaults.PermissionsAllowedPrefixCacheKey, entity.SystemName);
            RemoveByPrefix(prefix);
            RemoveByPrefix(TmSecurityDefaults.PermissionsAllByCustomerRoleIdPrefixCacheKey);
        }
    }
}
