using Tm.Core.Caching;

namespace Tm.Services.Gdpr
{
    /// <summary>
    /// Represents default values related to Gdpr services
    /// </summary>
    public static partial class TmGdprDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey ConsentsAllCacheKey => new CacheKey("Nop.consents.all");

        #endregion
    }
}