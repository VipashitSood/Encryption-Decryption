using Tm.Core.Domain.Localization;
using Tm.Services.Caching;

namespace Tm.Services.Localization.Caching
{
    /// <summary>
    /// Represents a localized property cache event consumer
    /// </summary>
    public partial class LocalizedPropertyCacheEventConsumer : CacheEventConsumer<LocalizedProperty>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(LocalizedProperty entity)
        {
            Remove(TmLocalizationDefaults.LocalizedPropertyAllCacheKey);

            var cacheKey = _cacheKeyService.PrepareKey(TmLocalizationDefaults.LocalizedPropertyCacheKey,
                entity.LanguageId, entity.EntityId, entity.LocaleKeyGroup, entity.LocaleKey);

            Remove(cacheKey);
        }
    }
}
