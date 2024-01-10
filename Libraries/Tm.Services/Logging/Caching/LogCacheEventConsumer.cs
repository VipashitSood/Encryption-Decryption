using Tm.Core.Domain.Logging;
using Tm.Services.Caching;

namespace Tm.Services.Logging.Caching
{
    /// <summary>
    /// Represents a log cache event consumer
    /// </summary>
    public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
    {
    }
}
