using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Constants;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Core.Domain.Pms.Attendance;
using Tm.Data;
using API.Models.Attendance;
using Tm.Services.Common;

namespace Tm.Services.Pms.ProjectDetail
{
    public interface IProjectsService
    {
        #region Project Detail
        Task<IList<Projects>> GetAllProjects();
        Task<IList<Projects>> GetAllActiveDevopsProjectsForTimeLogs();
        Task<Tuple<int, List<Projects>>> GetAllProjectsWithFilter(int projectNameId = 0, int projectTypeId = 0, int managerId = 0, int projectStatusId = 0, int pageIndex = 1, int pageSize = 5);
        Task<Projects> GetProjectsById(int id);
        Task<List<ProjectAttachment>> GetProjectsAttachmentsByProjectId(int id);
        Task UpdateProjectAttachment(ProjectAttachment projectAttachments);
        Task InsertProjects(Projects projects);
        Task UpdateProjects(Projects projects);
        Task<int> GetProjectTotalHoursAsync(int projectId);
        bool ProjectNameExists(string projectName);
        #endregion

        #region Billing_Info
        Task<IList<BillingInfo>> GetAllBillingInfoAsync();
        Task<BillingInfo> GetBillingInfoByIdAsync(int id);
        Task<IList<BillingInfo>> GetAllBillingInfoByProjectIdAsync(int projectId);
        Task InsertBillingInfoAsync(BillingInfo billingInfo);
        Task UpdateBillingInfoAsync(BillingInfo billingInfo);
        #endregion

        #region Change_Request
        Task<IList<ChangeRequest>> GetAllChangeRequestAsync();
        Task<ChangeRequest> GetChangeRequestByIdAsync(int id);
        Task InsertChangeRequestAsync(ChangeRequest changeRequest);
        Task UpdateChangeRequestAsync(ChangeRequest changeRequest);
        Task<IList<ChangeRequest>> GetAllChangeRequestByProjectIdAsync(int projectId);
        Task<IList<ChangeRequest>> GetAllChangeRequestIdByProjectIdAsync(int crId, int projectId);
        Task<int> GetChangeRequestTotalHoursAsync(int crId, int projectId);
        #endregion

        #region Client_Detail
        Task<IList<ClientDetail>> GetAllClientDetail();
        Task<ClientDetail> GetClientDetailById(int id);
        Task<IList<ClientDetail>> GetClientDetailsByProjectId(int projectId);
        Task<ClientDetail> GetClientDetailByProjectIdAsync(int projectId);
        Task InsertClientDetail(ClientDetail clientDetail);
        Task UpdateClientDetail(ClientDetail clientDetail);
        #endregion

        #region Project_Meetings
        Task<IList<Meetings>> GetAllMeetings();
        Task<Meetings> GetMeetingsById(int id);
        Task InsertMeetings(Meetings meeting);
        Task UpdateMeetings(Meetings meeting);
        #endregion

        #region Tech Stack Mapping
        Task<IList<TechStackMapping>> GetAllTechStackMapping();
        Task<TechStackMapping> GetTechStackMappingByProjectId(int projectid);
        Task<IList<TechStackMapping>> GetAllTechStackMappingByProjectId(int projectId);
        Task InsertTechStackMapping(TechStackMapping techStackMapping);
        Task UpdateTechStackMapping(TechStackMapping techStack);
        Task<TechStackMapping> DeleteTechStackMapping(int id);
        #endregion

        #region Attachment
        Task<IList<Attachments>> GetAllAttachment();
        Task<Attachments> GetAttachmentById(int id);
        Task<List<Attachments>> GetAttachmentIdByCRId(int crId);
        Task InsertAttachment(Attachments attachment);
        Task UpdateAttachment(Attachments attachment);
        Task DeleteAttachment(int id);
        #endregion

        #region Meetings 
        Task<IList<Meetings>> GetAllMeeting();
        Task<Meetings> GetMeetingById(int id);
        Task InsertMeeting(Meetings meeting);
        Task<Meetings> GetMeetingByProjectIdAsync(int projectId);
        Task UpdateMeeting(Meetings meeting);
        #endregion Meetings 

        #region Monthly Planned Hours 
        Task<IList<MonthlyPlannedHrs>> GetAllMonthlyHrsAsync();
        Task<IList<MonthlyPlannedHrs>> GetMonthlyHrsByProjectIdAsync(int billingInfoId);
        Task<MonthlyPlannedHrs> GetMonthlyHrsByIdAsync(int id);
        Task InsertMonthlyHrsAsync(MonthlyPlannedHrs monthlyPlanned);
        Task UpdateMonthlyHrsAsync(MonthlyPlannedHrs monthlyPlanned);
        Task<IList<MonthlyPlannedHrs>> GetAllMonthlyHrsByProjectIdAsync(int billingInfoId);

        #endregion Monthly Planned Hours 

        #region Other Info
        Task<IList<OtherInfo>> GetAllOtherInfoAsync();
        Task<OtherInfo> GetOtherInfoByIdAsync(int id);
        Task<List<OtherInfo>> GetAllOtherInfoByProjectIdAsync(int projectId);
        Task<OtherInfo> GetOtherInfoByProjectIdAsync(int projectId);
        Task InsertOtherInfoAsync(OtherInfo otherInfo);
        Task UpdateOtherInfoAsync(OtherInfo otherInfo);
        #endregion Other Info

        #region Project Managers
        Task<IList<ProjectManagers>> GetAllProjectManagers();
        Task SaveProjectManager(ProjectManagers manager);
        Task SaveResources(List<Resource> resources);
        Task<ProjectManagers> GetProjectManagersById(int id);

        #endregion

        #region Projecton

        /// <summary>
        /// Inserts a Projection asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        Task InsertProjectionRequestAsync(Projection projection);

        /// <summary>
        /// Update a Projection asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        Task UpdatetProjectionRequestAsync(Projection projection);
        /// <summary>
        /// Get projection detail by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Projection> GetProjectionDetailByIdAsync(int Id);
        /// <summary>
        /// Get Projections By UserID , start & end Date
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="resourceId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>

        Task<List<Projection>> GetProjectionDetailByDateRangeAsync(int projectId, string resourceId, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<ProjectionListResponseModel>> GetAllProjectionListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0);
        /// <summary>
        /// Get Projection calculation By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<ProjectionCalculationResponseModel>> GetProjectionCalculationByProjectIdAsync(int projectId, int languageId = 0);
        #endregion

        #region Attendance
        /// <summary>
        /// Inserts a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task InsertAttendanceRequestAsync(Attendance attendance);

        /// <summary>
        /// Update a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task UpdatetAttendanceRequestAsync(Attendance attendance);
        /// <summary>
        /// Get Attendance detail by ID asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task<Attendance> GetAttendanceDetailByIdAsync(int id);

        /// <summary>
        /// Get User list for Attendance from Projection
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<AttendenceUserList>> GetProjectionResourcesForAttendanceIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<AttendanceListResponseModel>> GetAllAttendanceListByProjectIdAsync(
        int projectId,
        DateTime? startDate = null,
        DateTime? endDate = null);

        /// <summary>
        /// Get Attendance from Devops list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<AttendanceListResponseModel>> GetAllADAttendanceListByProjectIdAsync(
        int projectId,
        DateTime? startDate = null,
        DateTime? endDate = null);
        // <summary>
        /// Get Projections By UserID , start & end Date
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<Attendance>> GetAttendanceDetailByDateRangeAsync(int projectId, string resourceId, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Copy Data from Projection to Attendance
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<DatabaseResponse> CopyProjectionToAttendanceAsync(int projectId, string resourceIds, int requestedBy, DateTime? startDate = null, DateTime? endDate = null);

        #endregion
        #region Ad_Attendance
        /// <summary>
        /// Update a AD Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task UpdateADAttendanceRequestAsync(ADAttendance attendance);
        /// <summary>
        /// Get AD Attendance detail by ID asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task<ADAttendance> GetADAttendanceDetailByIdAsync(int id);
        Task<DatabaseResponse> UpsertADAttendanceDataAsync(List<ADTimeLog> adDataList);
       
        #endregion
    }
}
