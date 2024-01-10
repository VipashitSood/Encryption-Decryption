using System;
using System.Linq;
using Tm.Core.Domain.Schedules;
using Tm.Core.Domain.Users;
using Tm.Data;

namespace Tm.Services.Users
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial class UserService : IUserService
    {
        #region Fields

        private readonly IRepository<UserScheduleMapping> _userScheduleMappingRepository;

        #endregion

        #region Ctor

        public UserService(IRepository<UserScheduleMapping> userScheduleMappingRepository)
        {
            _userScheduleMappingRepository = userScheduleMappingRepository;
        }

        #endregion

        #region Methods

        #region User schedule mapping

        /// <summary>
        /// Add a user-schedule mapping
        /// </summary>
        /// <param name="scheduleMapping">User-schedule mapping</param>
        public void AddUserScheduleMapping(UserScheduleMapping scheduleMapping)
        {
            if (scheduleMapping is null)
                throw new ArgumentNullException(nameof(scheduleMapping));

            _userScheduleMappingRepository.Insert(scheduleMapping);
        }

        ///// <summary>
        ///// Remove a user-schedule mapping
        ///// </summary>
        ///// <param name="user">User</param>
        ///// <param name="schedule">Schedule</param>
        //public void RemoveUserScheduleMapping(User user, Schedule schedule)
        //{
        //    if (user is null)
        //        throw new ArgumentNullException(nameof(user));

        //    if (schedule is null)
        //        throw new ArgumentNullException(nameof(schedule));

        //    var mapping = _userScheduleMappingRepository.Table.SingleOrDefault(usm => usm.UserId == user.Id && usm.ScheduleId == schedule.Id);

        //    if (mapping != null)
        //    {
        //        _userScheduleMappingRepository.Delete(mapping);
        //    }
        //}

        #endregion

        #endregion
    }
}