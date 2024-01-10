using API.Models.Attachments;
using API.Models.Attendance;
using API.Models.GeneralDetail;
using API.Models.ProjectDetail;
using API.Models.UserRole;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Constants;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Pms.Attendance;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;

namespace API.Factories.ProjectDetail
{
    public interface IProjectsFactory
    {
        #region Project Listing 
        Task<Tuple<int, List<ProjectListModel>>> GetAllProjectAsync(int projectNameId, int projectTypeId, int managerId, int projectStatusId, int pageIndex, int pageSize);
        #endregion Project Listing 

        #region General Detail
        Task<ProjectResponseModel> GetProjectDetailByIdAsync(int projectId);
        Task<(bool, string, int)> InsertUpdateGenDetail(GenDetailModel model);

        Task<GeneralDetailResponseModel> GetGeneralDetailById(int id);
        Task<(bool, string, int)> InsertUpdateGeneralDetail(GeneralDetailModel model);
        Task<(bool, string)> DeleteGeneralDetail(int id);
        #endregion General Detail

        #region Attachment 
        Task<List<AttachmentModel>> GetAllAttachment();
        Task<(bool, string)> DeleteAttachment(int attachId);
        Task<List<ProjectAttachmentModel>> GetProjectAttachmentById(int id);
        Task<(bool, string)> DeleteAttachmentIdByCRIdAsync(int crId);
        #endregion Attachment

        #region Client Details
        Task<ClientDetailModel> GetClientDetailByIdAsync(int id);
        Task<ClientDetailModel> GetClientDetailByProjectIdAsync(int projectId);
        Task<(bool, string, int)> InsertUpdateClientDetailAsync(ClientDetailModel model);

        #endregion Client Details

        #region Meetings 
        Task<MeetingModel> GetMeetingByIdAsync(int id);
        Task<MeetingModel> GetMeetingByProjectIdAsync(int projectId);
        Task<(bool, string, int)> InsertUpdateMeetingAsync(MeetingModel model);
        #endregion Meetings 

        #region Billing Info
        Task<BillingInfoModel> GetBillingInfoByIdAsync(int id);
        Task<(bool, string, int)> InsertUpdateBillingInfoAsync(BillingInfoModel model);
        Task<(bool, string)> DeleteBillingInfoAsync(int id);
        Task<List<BillingInfoModel>> GetAllBillingInfoByProjectIdAsync(int projectId);

        #endregion Billing Info

        #region Monthly Planned Hours 
        Task<MonthlyPlannedHrsModel> GetAllMonthlyPlannedHrs(int orderId);
        Task<MonthlyPlannedModel> GetMonthlyPlannedHrsByIdAsync(int id);
        Task<(bool, string, int)> InsertUpdateMonthlyPlannedHrsAsync(MonthlyPlannedModel model);
        Task<(bool, string)> DeleteMonthlyPlannedHrsAsync(int id);
        Task<IList<MonthlyPlannedModel>> GetAllMonthlyPlannedByProjectIdAsync(int orderId);
        #endregion Monthly Planned Hours 

        #region Tech Stack
        Task<List<TechStackMapping>> GetAllTechStackMapping();
        Task<IList<TechStackMapping>> GetAllTechStackMappingByProjectId(int projectid);
        Task<(bool, string)> DeleteTechStack(int id);
        Task<(bool, string)> SaveTechStackMapping(techStackList model);
        Task DeleteTechStackMapping(int id);
        #endregion Tech Stack

        #region Change Request
        Task<List<AttachmentModel>> GetCRAttachmentIdByProjectId(int crId, int projectId);
        Task<ChangeRequestResponseModel> GetChangeRequestByIdAsync(int id);
        Task<IList<ChangeRequestResponseModel>> GetAllCRByProjectIdAsync(int projectId);
        Task<(bool, string, int, int)> InsertChangeRequestAsync(ChangeRequestModel model);
        Task<(bool, string, int, int)> UpdateChangeRequestAsync(ChangeRequestModel model);
        Task<(bool, string, int, int)> UpdateChangeRequestAsync(ChangeRequestResponseModel model);
        //Task<(bool, string, int, int)> InsertUpdateChangeRequestAsync(ChangeRequestModel model);
        Task<(bool, string)> DeleteCRIdByProjectIdAsync(int crId, int projectId);

        #endregion Change Request

        #region Project Managers

        Task<List<Models.ProjectDetail.ProjectManagers>> GetAllProjectManagersAsync();
        Task SaveProjectManager(API.Models.ProjectDetail.ProjectManagers manager);
        Task SaveResources(List<ResourceDTO> resources);

        #endregion

        #region Other Info 
        Task<OtherInfoModel> GetOtherInfoByProjectId(int projectId);
        #endregion OtherInfo
        #region Projection
        /// <summary>
        /// Insert projection request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> InsertProjectionRequestAsync(ProjectionRequestModel model);
        /// <summary>
        /// Insert projection request 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> UpdateProjectionRequestAsync(UpdateProjectionRequestModel model);
        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<ProjectionResponseModel> GetAllProjectionListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0);
        /// <summary>
        /// Get projection By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Projection> GetProjectionDetailByIdAsync(int id);

        /// <summary>
        /// Get Project calculation by project Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<List<ProjectionCalculationResponseModel>> GetProjectionCalculationByProjectIdAsync(int projectId, int languageId = 0);


        #endregion

        #region Attandance
        /// <summary>
        /// Inserts a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task<(bool, string)> InsertAttendanceRequestAsync(CopyProjectionToAttendanceRequestModel model);

        /// <summary>
        /// Update a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        Task<(bool, string)> UpdateAttendanceRequestAsync(UpdateAttendanceRequestModel model);
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
        /// <param name="startDate"></paramGetAllAttendanceListByProjectIdAsync
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        Task<AttendanceResponseListModel> GetAllAttendanceListByProjectIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null, int languageId = 0);
        #endregion

        #region
        /// <summary>
        /// Insert & Update General Detail
        /// </summary>
        /// <returns></returns>
        Task<(bool, string)> UpsertAllADTimeLogs();
        #endregion
    }
}
