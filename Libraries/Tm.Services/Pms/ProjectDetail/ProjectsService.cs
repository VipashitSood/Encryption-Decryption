using API.Models.Attendance;
using LinqToDB;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using Tm.Core.Constants;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Localization;
using Tm.Core.Domain.Pms.Attendance;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Data;
using Tm.Services.Common;
using Tm.Services.Events;

namespace Tm.Services.Pms.ProjectDetail
{
    public partial class ProjectsService : IProjectsService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Projects> _projectsRepository;
        private readonly IRepository<BillingInfo> _billingInfoRepository;
        private readonly IRepository<ChangeRequest> _changeRequestRepository;
        private readonly IRepository<ClientDetail> _clientDetailRepository;
        private readonly IRepository<Meetings> _meetingsRepository;
        private readonly IRepository<TechStackMapping> _techStackMappingRepository;
        private readonly IRepository<Attachments> _attachmentRepository;
        private readonly IRepository<Meetings> _meetingRepository;
        private readonly IRepository<MonthlyPlannedHrs> _monthlyHrsRepository;
        private readonly IRepository<OtherInfo> _otherInfoRepository;
        private readonly IRepository<Resource> _projectResource;
        private readonly IRepository<ProjectManagers> _projectManagersRepository;
        private readonly IRepository<ProjectAttachment> _projectAttachmentRepository;
        private readonly IRepository<Projection> _projectionRepository;
        private readonly IRepository<DatabaseResponse> _databaseResponseRepository;
        private readonly IRepository<ProjectionListResponseModel> _projectionListResponseRepository;
        private readonly IRepository<ProjectionCalculationResponseModel> _projectionCalculationtResponseRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<AttendanceListResponseModel> _attendanceListRepository;
        private readonly IRepository<AttendenceUserList> _attendanceUserListRepository;
        private readonly IRepository<ADAttendance> _adAttendanceRepository;
        #endregion

        #region Ctor

        public ProjectsService(IEventPublisher eventPublisher,
            IRepository<Projects> projectsRepository,
            IRepository<BillingInfo> billingInfoRepository,
            IRepository<ChangeRequest> changeRequestRepository,
            IRepository<ClientDetail> clientDetailRepository,
            IRepository<Meetings> meetingsRepository,
            IRepository<TechStackMapping> techStackMappingRepository,
            IRepository<Attachments> attachmentRepository,
            IRepository<Meetings> meetingRepository,
            IRepository<MonthlyPlannedHrs> monthlyHrsRepository,
            IRepository<OtherInfo> otherInfoRepository,
            IRepository<Resource> projectResource,
            IRepository<ProjectManagers> projectManagersRepository,
            IRepository<Projection> projectionRepository,
            IRepository<ProjectAttachment> projectAttachmentRepository,
            IRepository<DatabaseResponse> databaseResponseRepository,
            IRepository<ProjectionListResponseModel> projectionListResponseRepository,
            IRepository<ProjectionCalculationResponseModel> projectionCalculationtResponseRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<AttendanceListResponseModel> attendanceListRepository,
            IRepository<AttendenceUserList> attendanceUserListRepository,
            IRepository<ADAttendance> adAttendanceRepository)
        {
            _eventPublisher = eventPublisher;
            _projectsRepository = projectsRepository;
            _billingInfoRepository = billingInfoRepository;
            _changeRequestRepository = changeRequestRepository;
            _clientDetailRepository = clientDetailRepository;
            _meetingsRepository = meetingsRepository;
            _techStackMappingRepository = techStackMappingRepository;
            _attachmentRepository = attachmentRepository;
            _meetingRepository = meetingRepository;
            _monthlyHrsRepository = monthlyHrsRepository;
            _otherInfoRepository = otherInfoRepository;
            _projectResource = projectResource;
            _projectManagersRepository = projectManagersRepository;
            _projectAttachmentRepository = projectAttachmentRepository;
            _projectionRepository = projectionRepository;
            _databaseResponseRepository = databaseResponseRepository;
            _projectionListResponseRepository = projectionListResponseRepository;
            _projectionCalculationtResponseRepository = projectionCalculationtResponseRepository;
            _attendanceRepository = attendanceRepository;
            _attendanceListRepository = attendanceListRepository;
            _attendanceUserListRepository = attendanceUserListRepository;
            _adAttendanceRepository = adAttendanceRepository;
        }

        #endregion

        #region Method

        #region Project Detail

        /// <summary>
        /// Gets all Projects
        /// </summary>
        /// <returns>Projects</returns>
        public virtual async Task<IList<Projects>> GetAllProjects()
        {
            var query = _projectsRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }
        /// <summary>
        /// Get all projects Active on Devops
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<Projects>> GetAllActiveDevopsProjectsForTimeLogs()
        {
            var query = _projectsRepository.Table.Where(x => !x.IsDeleted && x.IsAzure && !string.IsNullOrEmpty(x.Azure_ProjectId));

            var result = await query.ToListAsync();

            return result;
        }
        /// <summary>
        /// Get All Projects With Filter
        /// </summary>
        /// <param name="projectNameId"></param>
        /// <param name="projectTypeId"></param>
        /// <param name="managerId"></param>
        /// <param name="projectStatusId"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<int, List<Projects>>> GetAllProjectsWithFilter(int projectNameId = 0, int projectTypeId = 0, int managerId = 0, int projectStatusId = 0, int pageIndex = 1, int pageSize = 5)
        {
            var query = _projectsRepository.Table.Where(x => !x.IsDeleted);

            if (projectNameId > 0)
                query = query.Where(b => projectNameId == b.ProjectNameId);
            if (projectTypeId > 0)
                query = query.Where(b => projectTypeId == b.ProjectTypeId);
            if (managerId > 0)
                query = query.Where(b => managerId == b.ManagerId);
            if (projectStatusId > 0)
                query = query.Where(b => projectStatusId == b.ProjectStatusId);
            int count = query.ToList().Count();
            var result = await query.OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return Tuple.Create(count, result);
        }

        /// <summary>
        /// Gets a Projects 
        /// </summary>
        /// <param name="id">Projects identifier</param>
        /// <returns>Store</returns>
        public virtual async Task<Projects> GetProjectsById(int id)
        {
            if (id == 0)
                return null;

            var Projects = await _projectsRepository.GetByIdAsync(id);

            return Projects;
        }
        /// <summary>
        /// Get Project by Project Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<List<ProjectAttachment>> GetProjectsAttachmentsByProjectId(int id)
        {
            if (id == 0)
                return null;

            return await _projectAttachmentRepository.Table.Where(x => x.ProjectId == id && x.IsDeleted == false).ToListAsync();

        }
        /// <summary>
        /// Updates the Project Attachment
        /// </summary>
        /// <param name="projectAttachments">projectAttachments</param>
        public virtual async Task UpdateProjectAttachment(ProjectAttachment projectAttachments)
        {
            if (projectAttachments == null)
                throw new ArgumentNullException(nameof(projectAttachments));

            await _projectAttachmentRepository.UpdateAsync(projectAttachments);

            //event notification
            _eventPublisher.EntityUpdated(projectAttachments);
        }
        /// <summary>
        /// Inserts a Projects
        /// </summary>
        /// <param name="Projects">Projects</param>
        public virtual async Task InsertProjects(Projects projects)
        {
            if (projects == null)
                throw new ArgumentNullException(nameof(projects));

            await _projectsRepository.InsertAsync(projects);

            //event notification
            _eventPublisher.EntityInserted(projects);
        }

        /// <summary>
        /// Updates the Projects
        /// </summary>
        /// <param name="Projects">Projects</param>
        public virtual async Task UpdateProjects(Projects projects)
        {
            if (projects == null)
                throw new ArgumentNullException(nameof(projects));

            await _projectsRepository.UpdateAsync(projects);

            //event notification
            _eventPublisher.EntityUpdated(projects);
        }

        /// <summary>
        /// Get Total Hours
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<int> GetProjectTotalHoursAsync(int projectId)
        {
            int totalHrs = 0;
            var query = _projectsRepository.Table.Where(a => a.Id == projectId);

            // Assuming EstimatedEffortUnit and EffortsDuration are integer properties in the entity.

            // Get the first EstimatedEffortUnit value from the query
            int estHrs = await query.Select(x => x.EstimatedEffortUnit).FirstOrDefaultAsync();

            // Get the first EffortsDuration value from the query
            int effortsDuration = await query.Select(x => x.EffortsDuration).FirstOrDefaultAsync();

            // Calculate estimate total hours
            if (estHrs == (int)EstimatedEffortsEnum.Hours)
            {
                totalHrs = effortsDuration;
            }
            // Assuming a day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Days)
            {
                totalHrs = effortsDuration * 8;
            }
            // Assuming a week has 7 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Week)
            {
                totalHrs = effortsDuration * 7 * 8;
            }
            // Assuming a month has 30 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Months)
            {
                totalHrs = effortsDuration * 30 * 8;
            }
            // Assuming a year has 366 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Year)
            {
                totalHrs = effortsDuration * 366 * 8;
            }

            return totalHrs;
        }

        /// <summary>
        /// Project Name Exists
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public bool ProjectNameExists(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                return false;

            var query = _projectsRepository.Table.Where(p => p.Name.Contains(projectName));

            return query.Any();
        }

        #endregion

        #region Billing_Info

        /// <summary>
        /// Gets all BillingInfo asynchronously
        /// </summary>
        /// <returns>BillingInfo</returns>
        public virtual async Task<IList<BillingInfo>> GetAllBillingInfoAsync()
        {
            var query = _billingInfoRepository.Table;

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a BillingInfo asynchronously
        /// </summary>
        /// <param name="id">BillingInfo identifier</param>
        /// <returns>BillingInfo</returns>
        public virtual async Task<BillingInfo> GetBillingInfoByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var billingInfo = await _billingInfoRepository.GetByIdAsync(id);

            return billingInfo;
        }

        /// <summary>
        /// Get All BillingInfo By ProjectId asynchronously
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<BillingInfo>> GetAllBillingInfoByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _billingInfoRepository.Table.Where(x => !x.IsDeleted && x.ProjectId == projectId).ToListAsync();

            return await query;
        }

        /// <summary>
        /// Inserts a BillingInfo asynchronously
        /// </summary>
        /// <param name="billingInfo">billingInfo</param>
        public virtual async Task InsertBillingInfoAsync(BillingInfo billingInfo)
        {
            if (billingInfo == null)
                throw new ArgumentNullException(nameof(billingInfo));

            await _billingInfoRepository.InsertAsync(billingInfo);

            // event notification
            _eventPublisher.EntityInserted(billingInfo);
        }

        /// <summary>
        /// Updates the BillingInfo asynchronously
        /// </summary>
        /// <param name="billingInfo">billingInfo</param>
        public virtual async Task UpdateBillingInfoAsync(BillingInfo billingInfo)
        {
            if (billingInfo == null)
                throw new ArgumentNullException(nameof(billingInfo));

            await _billingInfoRepository.UpdateAsync(billingInfo);

            // event notification
            _eventPublisher.EntityUpdated(billingInfo);
        }

        #endregion

        #region Change_Request
        /// <summary>
        /// Gets all ChangeRequest asynchronously
        /// </summary>
        /// <returns>List of ChangeRequest</returns>
        public virtual async Task<IList<ChangeRequest>> GetAllChangeRequestAsync()
        {
            var query = _changeRequestRepository.Table;

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a ChangeRequest asynchronously
        /// </summary>
        /// <param name="id">ChangeRequest identifier</param>
        /// <returns>ChangeRequest</returns>
        public virtual async Task<ChangeRequest> GetChangeRequestByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var changeRequest = await _changeRequestRepository.GetByIdAsync(id);

            return changeRequest;
        }

        /// <summary>
        /// Inserts a ChangeRequest asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        public virtual async Task InsertChangeRequestAsync(ChangeRequest changeRequest)
        {
            if (changeRequest == null)
                throw new ArgumentNullException(nameof(changeRequest));

            await _changeRequestRepository.InsertAsync(changeRequest);

            // event notification
            _eventPublisher.EntityInserted(changeRequest);
        }

        /// <summary>
        /// Updates the ChangeRequest asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        public virtual async Task UpdateChangeRequestAsync(ChangeRequest changeRequest)
        {
            if (changeRequest == null)
                throw new ArgumentNullException(nameof(changeRequest));

            await _changeRequestRepository.UpdateAsync(changeRequest);

            // event notification
            _eventPublisher.EntityUpdated(changeRequest);
        }

        /// <summary>
        /// Get All Change Request By ProjectId asynchronously
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<ChangeRequest>> GetAllChangeRequestByProjectIdAsync(int ProjectId)
        {
            if (ProjectId == 0)
                return null;

            var query = _changeRequestRepository.Table.Where(b => !b.IsDeleted && b.ProjectId == ProjectId).ToListAsync();

            return await query;
        }

        /// <summary>
        /// Get All ChangeRequestId By ProjectId asynchronously
        /// </summary>
        /// <param name="crId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<ChangeRequest>> GetAllChangeRequestIdByProjectIdAsync(int crId, int orderId)
        {
            if (crId == 0 && orderId == 0)
                return null;

            var query = _changeRequestRepository.Table.Where(b => !b.IsDeleted && b.OrderId == orderId && b.Id == crId).ToListAsync();

            return await query;
        }

        /// <summary>
        /// Get ChangeRequest Total Hours asynchronously
        /// </summary>
        /// <param name="crId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<int> GetChangeRequestTotalHoursAsync(int crId, int projectId)
        {
            int totalHrs = 0;
            var query = _changeRequestRepository.Table.Where(a => a.OrderId == projectId && a.Id == crId);

            // Get the first EstimatedEffortUnit value from the query
            int estHrs = await query.Select(x => x.EstimatedEfforts).FirstOrDefaultAsync();

            // Get the first EffortsDuration value from the query
            int effortsDuration = await query.Select(x => x.EstimatedDuration).FirstOrDefaultAsync();

            // Calculate estimate total hours
            if (estHrs == (int)EstimatedEffortsEnum.Hours)
            {
                totalHrs = effortsDuration;
            }
            // Assuming a day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Days)
            {
                totalHrs = effortsDuration * 8;
            }
            // Assuming a week has 7 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Week)
            {
                totalHrs = effortsDuration * 7 * 8;
            }
            // Assuming a month has 30 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Months)
            {
                totalHrs = effortsDuration * 30 * 8;
            }
            // Assuming a year has 366 days and each day has 8 hours of work.
            else if (estHrs == (int)EstimatedEffortsEnum.Year)
            {
                totalHrs = effortsDuration * 366 * 8;
            }

            return totalHrs;
        }

        #endregion

        #region Client_Detail
        /// <summary>
        /// Gets all ClientDetail
        /// </summary>
        /// <returns>ClientDetail</returns>
        public virtual async Task<IList<ClientDetail>> GetAllClientDetail()
        {
            var query = _clientDetailRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a ClientDetail 
        /// </summary>
        /// <param name="id">ClientDetail identifier</param>
        /// <returns>ClientDetail</returns>
        public virtual async Task<ClientDetail> GetClientDetailById(int id)
        {
            if (id == 0)
                return null;

            var Projects = await _clientDetailRepository.GetByIdAsync(id);

            return Projects;
        }

        /// <summary>
        /// Get ClientDetailId By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<ClientDetail>> GetClientDetailsByProjectId(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = await _clientDetailRepository.Table.Where(b => b.ProjectId == projectId).ToListAsync();
            return query;
        }

        /// <summary>
        /// Get ClientDetails By ProjectId Async
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<ClientDetail> GetClientDetailByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _clientDetailRepository.Table.Where(b => b.ProjectId == projectId);
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a ClientDetail
        /// </summary>
        /// <param name="clientDetail">ClientDetail</param>
        public virtual async Task InsertClientDetail(ClientDetail clientDetail)
        {
            if (clientDetail == null)
                throw new ArgumentNullException(nameof(clientDetail));

            await _clientDetailRepository.InsertAsync(clientDetail);

            //event notification
            _eventPublisher.EntityInserted(clientDetail);
        }

        /// <summary>
        /// Updates the ClientDetail
        /// </summary>
        /// <param name="clientDetail">clientDetail</param>
        public virtual async Task UpdateClientDetail(ClientDetail clientDetail)
        {
            if (clientDetail == null)
                throw new ArgumentNullException(nameof(clientDetail));

            await _clientDetailRepository.UpdateAsync(clientDetail);

            //event notification
            _eventPublisher.EntityUpdated(clientDetail);
        }
        #endregion

        #region Project_Meetings
        /// <summary>
        /// Gets all Meetings
        /// </summary>
        /// <returns>Meetings</returns>
        public virtual async Task<IList<Meetings>> GetAllMeetings()
        {
            var query = _meetingsRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a Meetings 
        /// </summary>
        /// <param name="id">Meetings identifier</param>
        /// <returns>Meetings</returns>
        public virtual async Task<Meetings> GetMeetingsById(int id)
        {
            if (id == 0)
                return null;

            var Projects = await _meetingsRepository.GetByIdAsync(id);

            return Projects;
        }

        /// <summary>
        /// Inserts a Meetings
        /// </summary>
        /// <param name="Meetings">Meetings</param>
        public virtual async Task InsertMeetings(Meetings meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            await _meetingsRepository.InsertAsync(meeting);

            //event notification
            _eventPublisher.EntityInserted(meeting);
        }

        /// <summary>
        /// Updates the Meetings
        /// </summary>
        /// <param name="Meetings">Meetings</param>
        public virtual async Task UpdateMeetings(Meetings meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            await _meetingsRepository.UpdateAsync(meeting);

            //event notification
            _eventPublisher.EntityUpdated(meeting);
        }
        #endregion

        #region Tech Stack Mapping
        /// <summary>
        /// Gets all TechStackMapping
        /// </summary>
        /// <returns>TechStackMapping</returns>
        public virtual async Task<IList<TechStackMapping>> GetAllTechStackMapping()
        {
            var query = _techStackMappingRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a TechStackMapping 
        /// </summary>
        /// <param name="id">TechStackMapping identifier</param>
        /// <returns>TechStackMapping</returns>
        public virtual async Task<TechStackMapping> GetTechStackMappingByProjectId(int projectId)
        {
            if (projectId == 0)
                return null;

            var techStackMapping = await _techStackMappingRepository.Table.FirstOrDefaultAsync(b => b.ProjectId == projectId);

            return techStackMapping;
        }

        /// <summary>
        /// Gets a TechStackMapping 
        /// </summary>
        /// <param name="id">TechStackMapping identifier</param>
        /// <returns>TechStackMapping</returns>
        public virtual async Task<IList<TechStackMapping>> GetAllTechStackMappingByProjectId(int projectId)
        {
            if (projectId == 0)
                return null;

            var techStackMapping = await _techStackMappingRepository.Table.Where(x => x.ProjectId == projectId && x.IsDeleted != true).ToListAsync();

            return techStackMapping;
        }

        /// <summary>
        /// Inserts a TechStackMapping
        /// </summary>
        /// <param name="techStackMapping">TechStackMapping</param>
        public virtual async Task InsertTechStackMapping(TechStackMapping techStackMapping)
        {
            if (techStackMapping == null)
                throw new ArgumentNullException(nameof(techStackMapping));

            await _techStackMappingRepository.InsertAsync(techStackMapping);

            //event notification
            _eventPublisher.EntityInserted(techStackMapping);
        }

        /// <summary>
        /// Updates the TechStackMapping
        /// </summary>
        /// <param name="techStackMapping">TechStackMapping</param>
        public virtual async Task UpdateTechStackMapping(TechStackMapping techStackMapping)
        {
            if (techStackMapping == null)
                throw new ArgumentNullException(nameof(techStackMapping));

            await _techStackMappingRepository.UpdateAsync(techStackMapping);

            //event notification
            _eventPublisher.EntityUpdated(techStackMapping);
        }

        public virtual async Task<TechStackMapping> DeleteTechStackMapping(int id)
        {
            if (id == 0)
                return null;

            var Projects = _techStackMappingRepository.GetByIdAsync(id);

            return await Projects;
        }

        #endregion

        #region Attachment
        /// <summary>
        /// Gets all Attachment
        /// </summary>
        /// <returns>Attachment</returns>
        public virtual async Task<IList<Attachments>> GetAllAttachment()
        {
            var query = _attachmentRepository.Table.Where(x => x.IsDeleted == false);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a Attachment 
        /// </summary>
        /// <param name="id">Attachment identifier</param>
        /// <returns>Attachment</returns>
        public virtual async Task<Attachments> GetAttachmentById(int id)
        {
            if (id == 0)
                return null;

            var Projects = await _attachmentRepository.GetByIdAsync(id);

            return Projects;
        }
        /// <summary>
        /// Get AttachmentId By CRId
        /// </summary>
        /// <param name="crId"></param>
        /// <returns></returns>
        public virtual async Task<List<Attachments>> GetAttachmentIdByCRId(int crId)
        {
            if (crId == 0)
                return null;

            var query = _attachmentRepository.Table.Where(x => x.Id == crId);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Inserts a Attachment
        /// </summary>
        /// <param name="Attachment">Attachment</param>
        public virtual async Task InsertAttachment(Attachments attachments)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            await _attachmentRepository.InsertAsync(attachments);

            //event notification
            _eventPublisher.EntityInserted(attachments);
        }

        /// <summary>
        /// Updates the Attachment
        /// </summary>
        /// <param name="Attachment">Attachment</param>
        public virtual async Task UpdateAttachment(Attachments attachments)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            await _attachmentRepository.UpdateAsync(attachments);

            //event notification
            _eventPublisher.EntityUpdated(attachments);
        }

        public virtual async Task DeleteAttachment(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var attachment = await _attachmentRepository.GetByIdAsync(id);

            await _attachmentRepository.DeleteAsync(attachment);
        }

        #endregion

        #region Meetings 

        /// <summary>
        /// Get All Meeting
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<Meetings>> GetAllMeeting()
        {
            var query = _meetingRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Meeting By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Meetings> GetMeetingById(int id)
        {
            if (id == 0)
                return null;

            var meeting = await _meetingRepository.GetByIdAsync(id);

            return meeting;
        }

        /// <summary>
        /// Get Meeting By ProjectIdAsync
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<Meetings> GetMeetingByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _meetingRepository.Table.Where(x => !x.IsDeleted && x.ProjectId == projectId);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get All Meetings By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<Meetings>> GetAllMeetingByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _meetingRepository.Table.Where(x => !x.IsDeleted && x.ProjectId == projectId).ToListAsync();

            return await query;
        }

        /// <summary>
        /// Insert Meeting
        /// </summary>
        /// <param name="meeting"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertMeeting(Meetings meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            await _meetingRepository.InsertAsync(meeting);

            //event notification
            _eventPublisher.EntityInserted(meeting);
        }

        /// <summary>
        /// Update Meeting
        /// </summary>
        /// <param name="meeting"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateMeeting(Meetings meeting)
        {
            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            await _meetingRepository.UpdateAsync(meeting);

            //event notification
            _eventPublisher.EntityUpdated(meeting);
        }

        #endregion Meetings 

        #region Monthly Planned Hours 

        /// <summary>
        /// Gets all MonthlyPlannedHrs asynchronously
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<MonthlyPlannedHrs>> GetAllMonthlyHrsAsync()
        {
            var query = _monthlyHrsRepository.Table.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id);

            var result = await query.ToListAsync();

            return result;
        }
        /// <summary>
        /// Gets a MonthlyPlannedHrs asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<IList<MonthlyPlannedHrs>> GetMonthlyHrsByProjectIdAsync(int orderId)
        {
            var query = _monthlyHrsRepository.Table.Where(x => !x.IsDeleted && x.OrderId == orderId).OrderByDescending(x => x.Id);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a MonthlyPlannedHrs asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual async Task<MonthlyPlannedHrs> GetMonthlyHrsByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var billingInfo = await _monthlyHrsRepository.GetByIdAsync(id);
            if (billingInfo.IsDeleted != true)
            {
                return billingInfo;
            }
            else
            {
                throw new Exception(ConstantValues.RecordNotFound);
            }
        }

        /// <summary>
        /// Inserts a MonthlyPlannedHrs asynchronously
        /// </summary>
        /// <param name="monthlyPlanned"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertMonthlyHrsAsync(MonthlyPlannedHrs monthlyPlanned)
        {
            if (monthlyPlanned == null)
                throw new ArgumentNullException(nameof(monthlyPlanned));

            await _monthlyHrsRepository.InsertAsync(monthlyPlanned);

            // event notification
            _eventPublisher.EntityInserted(monthlyPlanned);
        }

        /// <summary>
        /// Updates the MonthlyPlannedHrs asynchronously
        /// </summary>
        /// <param name="monthlyPlanned"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateMonthlyHrsAsync(MonthlyPlannedHrs monthlyPlanned)
        {
            if (monthlyPlanned == null)
                throw new ArgumentNullException(nameof(monthlyPlanned));

            await _monthlyHrsRepository.UpdateAsync(monthlyPlanned);

            // event notification
            _eventPublisher.EntityUpdated(monthlyPlanned);
        }

        /// <summary>
        /// Get All MonthlyHrs By ProjectId asynchronously
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<IList<MonthlyPlannedHrs>> GetAllMonthlyHrsByProjectIdAsync(int orderId)
        {
            if (orderId == 0)
                return null;

            var query = _monthlyHrsRepository.Table.Where(b => b.OrderId == orderId).ToListAsync();

            return await query;
        }

        #endregion Monthly Planned Hours 

        #region Other Info
        /// <summary>
        /// Get All OtherInfo
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<OtherInfo>> GetAllOtherInfoAsync()
        {
            var query = _otherInfoRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get OtherInfo By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OtherInfo> GetOtherInfoByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var otherInfo = await _otherInfoRepository.GetByIdAsync(id);

            return otherInfo;
        }

        /// <summary>
        /// Get All OtherInfo By ProjectId Async
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<List<OtherInfo>> GetAllOtherInfoByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _otherInfoRepository.Table.Where(x => !x.IsDeleted).Where(x => !x.IsDeleted && x.ProjectId == projectId).ToListAsync();

            return await query;
        }
        /// <summary>
        /// Get OtherInfo By ProjectId Async
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public virtual async Task<OtherInfo> GetOtherInfoByProjectIdAsync(int projectId)
        {
            if (projectId == 0)
                return null;

            var query = _otherInfoRepository.Table.Where(x => !x.IsDeleted).Where(x => !x.IsDeleted && x.ProjectId == projectId);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert OtherInfo
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertOtherInfoAsync(OtherInfo otherInfo)
        {
            if (otherInfo == null)
                throw new ArgumentNullException(nameof(otherInfo));

            await _otherInfoRepository.InsertAsync(otherInfo);

            //event notification
            _eventPublisher.EntityInserted(otherInfo);
        }

        /// <summary>
        /// Update OtherInfo
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateOtherInfoAsync(OtherInfo otherInfo)
        {
            if (otherInfo == null)
                throw new ArgumentNullException(nameof(otherInfo));

            await _otherInfoRepository.UpdateAsync(otherInfo);

            //event notification
            _eventPublisher.EntityUpdated(otherInfo);
        }

        #endregion Other Info

        #region Project Managers

        public virtual async Task<IList<ProjectManagers>> GetAllProjectManagers()
        {
            return await _projectManagersRepository.Table.ToListAsync();
        }
        public virtual async Task<ProjectManagers> GetProjectManagersById(int id)
        {
            if (id == 0)
                throw new Exception(ConstantValues.InvalidManagerId);
            return await _projectManagersRepository.Table.FirstOrDefaultAsync(x => x.Id == id);
        }
        public virtual async Task SaveProjectManager(ProjectManagers manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            await _projectManagersRepository.InsertAsync(manager);
            _eventPublisher.EntityInserted(manager);
        }
        public async Task SaveResources(List<Resource> resources)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));
            foreach (var item in resources)
            {
                await _projectResource.InsertAsync(item);
                _eventPublisher.EntityInserted(item);
            }
        }

        #endregion

        #region Projecton

        /// <summary>
        /// Inserts a Projection asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        public virtual async Task InsertProjectionRequestAsync(Projection projection)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));

            await _projectionRepository.InsertAsync(projection);

            // event notification
            _eventPublisher.EntityInserted(projection);
        }


        /// <summary>
        /// Update a Projection asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        public virtual async Task UpdatetProjectionRequestAsync(Projection projection)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));

            await _projectionRepository.UpdateAsync(projection);

            // event notification
            _eventPublisher.EntityUpdated(projection);
        }

        /// <summary>
        /// Get Projection detail by ID asynchronously
        /// </summary>
        /// <param name="changeRequest">changeRequest</param>
        public virtual async Task<Projection> GetProjectionDetailByIdAsync(int id)
        {
            return await _projectionRepository.Table.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false && x.IsActive == true);
        }
        /// <summary>
        /// Get Projections By UserID , start & end Date
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<List<Projection>> GetProjectionDetailByDateRangeAsync(int projectId, string resourceId, DateTime startDate, DateTime endDate)
        {
            return await _projectionRepository.Table.Where(x => x.ResourceId == resourceId && x.ProjectId == projectId && (x.ProjectionDate >= startDate && x.ProjectionDate <= endDate) && x.IsActive == true && x.IsDeleted == false).ToListAsync();
        }


        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async virtual Task<List<ProjectionListResponseModel>> GetAllProjectionListByProjectIdAsync(
        int projectId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int languageId = 0)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pStartDate = SqlParameterHelper.GetDateTimeParameter("StartDate", startDate);
            var pEndDate = SqlParameterHelper.GetDateTimeParameter("EndDate", endDate);
            var pLanguageId = SqlParameterHelper.GetInt32Parameter("LanguageId", languageId);
            return await Task.Factory.StartNew<List<ProjectionListResponseModel>>(() =>
            {
                List<ProjectionListResponseModel> projectionList = _projectionListResponseRepository.EntityFromSql("SSP_GetProjectionDetailbyProjectId",
                    pProjectIdId,
                    pStartDate,
                    pEndDate,
                    pLanguageId).ToList();
                return projectionList;
            });
        }


        /// <summary>
        /// Get Projection calculation By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async virtual Task<List<ProjectionCalculationResponseModel>> GetProjectionCalculationByProjectIdAsync(
        int projectId,
        int languageId = 0)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pLanguageId = SqlParameterHelper.GetInt32Parameter("LanguageId", languageId);
            return await Task.Factory.StartNew<List<ProjectionCalculationResponseModel>>(() =>
            {
                List<ProjectionCalculationResponseModel> projectionList = _projectionCalculationtResponseRepository.EntityFromSql("SSP_GetProjectionCalculationbyProjectId",
                    pProjectIdId,
                    pLanguageId).ToList();
                return projectionList;
            });
        }
        #endregion

        #region Attendance
        /// <summary>
        /// Inserts a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        public virtual async Task InsertAttendanceRequestAsync(Attendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance));

            await _attendanceRepository.InsertAsync(attendance);

            // event notification
            _eventPublisher.EntityInserted(attendance);
        }


        /// <summary>
        /// Update a Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        public virtual async Task UpdatetAttendanceRequestAsync(Attendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance));

            await _attendanceRepository.UpdateAsync(attendance);

            // event notification
            _eventPublisher.EntityUpdated(attendance);
        }
        /// <summary>
        /// Get Attendance detail by ID asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        public virtual async Task<Attendance> GetAttendanceDetailByIdAsync(int id)
        {
            return await _attendanceRepository.Table.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false && x.IsActive == true);
        }
        /// <summary>
        /// Get User list for Attendance from Projection
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<AttendenceUserList>> GetProjectionResourcesForAttendanceIdAsync(int projectId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pStartDate = SqlParameterHelper.GetDateTimeParameter("StartDate", startDate);
            var pEndDate = SqlParameterHelper.GetDateTimeParameter("EndDate", endDate);
            return await Task.Factory.StartNew<List<AttendenceUserList>>(() =>
            {
                List<AttendenceUserList> projectionList = _attendanceUserListRepository.EntityFromSql("SSP_GetProjectionUsersDetailbyProjectId",
                    pProjectIdId,
                    pStartDate,
                    pEndDate).ToList();
                return projectionList;
            });
        }


        /// <summary>
        /// Get Projection list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async virtual Task<List<AttendanceListResponseModel>> GetAllAttendanceListByProjectIdAsync(
        int projectId,
        DateTime? startDate = null,
        DateTime? endDate = null)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pStartDate = SqlParameterHelper.GetDateTimeParameter("StartDate", startDate);
            var pEndDate = SqlParameterHelper.GetDateTimeParameter("EndDate", endDate);
            return await Task.Factory.StartNew<List<AttendanceListResponseModel>>(() =>
            {
                List<AttendanceListResponseModel> projectionList = _attendanceListRepository.EntityFromSql("SSP_GetADAttendanceDetailbyProjectId",
                    pProjectIdId,
                    pStartDate,
                    pEndDate).ToList();
                return projectionList;
            });
        }

        /// <summary>
        /// Get Attendance from Devops list By projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public async virtual Task<List<AttendanceListResponseModel>> GetAllADAttendanceListByProjectIdAsync(
        int projectId,
        DateTime? startDate = null,
        DateTime? endDate = null)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pStartDate = SqlParameterHelper.GetDateTimeParameter("StartDate", startDate);
            var pEndDate = SqlParameterHelper.GetDateTimeParameter("EndDate", endDate);
            return await Task.Factory.StartNew<List<AttendanceListResponseModel>>(() =>
            {
                List<AttendanceListResponseModel> projectionList = _attendanceListRepository.EntityFromSql("SSP_GetAttendanceDetailbyProjectId",
                    pProjectIdId,
                    pStartDate,
                    pEndDate).ToList();
                return projectionList;
            });
        }

        /// <summary>
        /// Get Projections By UserID , start & end Date
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public virtual async Task<List<Attendance>> GetAttendanceDetailByDateRangeAsync(int projectId, string resourceId, DateTime startDate, DateTime endDate)
        {
            return await _attendanceRepository.Table.Where(x => x.ResourceId == resourceId && x.ProjectId == projectId && (x.AttendanceDate >= startDate && x.AttendanceDate <= endDate) && x.IsActive == true && x.IsDeleted == false).ToListAsync();
        }

        /// <summary>
        /// Copy Data from Projection to Attendance
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<DatabaseResponse> CopyProjectionToAttendanceAsync(int projectId, string resourceIds, int requestedBy, DateTime? startDate = null, DateTime? endDate = null)
        {
            var pProjectIdId = SqlParameterHelper.GetInt32Parameter("ProjectId", projectId);
            var pResourceIds = SqlParameterHelper.GetStringParameter("ResourceIds", resourceIds);
            var pStartDate = SqlParameterHelper.GetDateTimeParameter("StartDate", startDate);
            var pEndDate = SqlParameterHelper.GetDateTimeParameter("EndDate", endDate);
            var pRequestedById = SqlParameterHelper.GetInt32Parameter("RequestedById", requestedBy);
            return await Task.Factory.StartNew<DatabaseResponse>(() =>
            {
                return _databaseResponseRepository.EntityFromSql("SSP_InsertUserAttendanceFormProjectionbyProjectId",
                    pProjectIdId,
                    pResourceIds,
                    pStartDate,
                    pEndDate,
                    pRequestedById).FirstOrDefault();
            });
        }
        #endregion

        #region Ad_Attendance
        /// <summary>
        /// Update a AD Attendance asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        public virtual async Task UpdateADAttendanceRequestAsync(ADAttendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance));

            await _adAttendanceRepository.UpdateAsync(attendance);

            // event notification
            _eventPublisher.EntityUpdated(attendance);
        }
        /// <summary>
        /// Get AD Attendance detail by ID asynchronously
        /// </summary>
        /// <param name="attendance">attendance</param>
        public virtual async Task<ADAttendance> GetADAttendanceDetailByIdAsync(int id)
        {
            return await _adAttendanceRepository.Table.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false && x.IsActive == true);
        }
        /// <summary>
        /// Insert & Updat AD Time log
        /// </summary>
        /// <param name="adDataList"></param>
        /// <returns></returns>
        public async Task<DatabaseResponse> UpsertADAttendanceDataAsync(List<ADTimeLog> adDataList)
        {
            //var test = CommonMethod.ToDataTable(adDataList);
            //var pTimeLogDataType = SqlParameterHelper.GetStructuredParameter("TimeLogDataType", CommonMethod.ToDataTable(adDataList));

            //IEnumerable To DataTable
            DataTable dtTimeLog;
            dtTimeLog = CommonMethod.IEnumerableToDataTable(adDataList);

            //Add paramter
            var pTimeLogDataType = SqlParameterHelper.GetTableTypeStructuredParameter("TimeLogData", dtTimeLog);
            return await Task.Factory.StartNew<DatabaseResponse>(() =>
            {
                return _databaseResponseRepository.EntityFromSql("SSP_UpsertADAttendance", pTimeLogDataType).FirstOrDefault();
            });
        }
        #endregion

        #endregion


    }
}