using Tm.Core.Domain.Security;
using Tm.Services.Caching;

namespace Tm.Services.Security.Caching
{
    /// <summary>
    /// Represents a permission record-customer role mapping cache event consumer
    /// </summary>
    public partial class PermissionRecordCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<PermissionRecordCustomerRoleMapping>
    {
    }
}