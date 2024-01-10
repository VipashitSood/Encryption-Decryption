using Tm.Core.Domain.Messages;
using Tm.Services.Caching;

namespace Tm.Services.Messages.Caching
{
    /// <summary>
    /// Represents an queued email cache event consumer
    /// </summary>
    public partial class QueuedEmailCacheEventConsumer : CacheEventConsumer<QueuedEmail>
    {
    }
}
