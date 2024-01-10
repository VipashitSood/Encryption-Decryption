using Tm.Core.Domain.Media;
using Tm.Services.Caching;

namespace Tm.Services.Media.Caching
{
    /// <summary>
    /// Represents a picture cache event consumer
    /// </summary>
    public partial class PictureCacheEventConsumer : CacheEventConsumer<Picture>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Picture entity)
        {
            RemoveByPrefix(TmMediaDefaults.ThumbsExistsPrefixCacheKey);
        }
    }
}
