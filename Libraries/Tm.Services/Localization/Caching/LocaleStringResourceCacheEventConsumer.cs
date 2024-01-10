using Tm.Core.Domain.Localization;
using Tm.Services.Caching;

namespace Tm.Services.Localization.Caching
{
    /// <summary>
    /// Represents a locale string resource cache event consumer
    /// </summary>
    public partial class LocaleStringResourceCacheEventConsumer : CacheEventConsumer<LocaleStringResource>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(LocaleStringResource entity)
        {
            RemoveByPrefix(TmLocalizationDefaults.LocaleStringResourcesPrefixCacheKey);
        }
    }
}
