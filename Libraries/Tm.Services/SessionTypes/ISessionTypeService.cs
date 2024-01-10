using System.Collections.Generic;
using Tm.Core.Domain.SessionTypes;

namespace Tm.Services.SessionTypes
{
    /// <summary>
    /// Session type service interface
    /// </summary>
    public partial interface ISessionTypeService
    {
        /// <summary>
        /// Gets all session types
        /// </summary>
        /// <returns>SessionTypes</returns>
        IList<SessionType> GetAllSessionTypes();
    }
}