using Tm.Core.Domain.Schedules;

namespace Tm.Services.Schedules
{
    /// <summary>
    /// Schedule service interface
    /// </summary>
    public partial interface IScheduleService
    {
        /// <summary>
        /// Deletes a schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        void DeleteSchedule(Schedule schedule);

        /// <summary>
        /// Gets a schedule 
        /// </summary>
        /// <param name="scheduleId">Schedule identifier</param>
        /// <returns>Schedule</returns>
        Schedule GetScheduleById(int scheduleId);

        /// <summary>
        /// Inserts a schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        void InsertSchedule(Schedule schedule);

        /// <summary>
        /// Updates the schedule
        /// </summary>
        /// <param name="schedule">Schedule</param>
        void UpdateSchedule(Schedule schedule);
    }
}