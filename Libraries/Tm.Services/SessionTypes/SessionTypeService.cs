using System.Collections.Generic;
using System.Linq;
using Tm.Core.Domain.SessionTypes;
using Tm.Data;

namespace Tm.Services.SessionTypes
{
    /// <summary>
    /// Session type service
    /// </summary>
    public partial class SessionTypeService : ISessionTypeService
    {
        #region Fields

        private readonly IRepository<SessionType> _sessionTypeRepository;

        #endregion

        #region Ctor

        public SessionTypeService(IRepository<SessionType> sessionTypeRepository)
        {
            _sessionTypeRepository = sessionTypeRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all session types
        /// </summary>
        /// <returns>Session Types</returns>
        public virtual IList<SessionType> GetAllSessionTypes()
        {
            var query = _sessionTypeRepository.Table.Where(st => st.Published);

            query = query.OrderBy(st => st.DisplayOrder).ThenBy(st => st.Id);

            return query.ToList();
        }

        #endregion
    }
}