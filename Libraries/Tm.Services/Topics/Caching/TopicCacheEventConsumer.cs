using Tm.Core.Domain.Topics;
using Tm.Services.Caching;

namespace Tm.Services.Topics.Caching
{
    /// <summary>
    /// Represents a topic cache event consumer
    /// </summary>
    public partial class TopicCacheEventConsumer : CacheEventConsumer<Topic>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void ClearCache(Topic entity)
        {
            RemoveByPrefix(TmTopicDefaults.TopicsAllPrefixCacheKey);
            var prefix = _cacheKeyService.PrepareKeyPrefix(TmTopicDefaults.TopicBySystemNamePrefixCacheKey, entity.SystemName);
            RemoveByPrefix(prefix);
        }
    }
}
