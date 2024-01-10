using Tm.Core.Domain.Users;

namespace Tm.Services.Users
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IUserService
    {
        #region User schedule mapping

        /// <summary>
        /// Add a user-schedule mapping
        /// </summary>
        /// <param name="scheduleMapping">User-schedule mapping</param>
        void AddUserScheduleMapping(UserScheduleMapping scheduleMapping);

        ///// <summary>
        ///// Remove a user-schedule mapping
        ///// </summary>
        ///// <param name="user">User</param>
        ///// <param name="schedule">Schedule</param>
        //void RemoveUserScheduleMapping(User user, Schedule schedule);

        #endregion
    }
}