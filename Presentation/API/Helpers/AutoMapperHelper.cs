using AutoMapper;
using System;
using Tm.Core.Constants;
using Amp = API.Models.ProjectDetail;
using API.Models.ProjectDetail;
using Tm.Core.Domain.Pms.ProjectDetail;
using TPsm = Tm.Core.Domain.Pms.ProjectDetail;
using API.Models.UserRole;
using Tm.Core.Domain.Pms.UserRole;
using API.Models.Attachments;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.Projection;
using Tm.Core.Domain.Pms.PmsMenu;
using API.Models.PmsMenu;

namespace API.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateGeneralDetailMaps();
            CreateClientDetailMaps();
            CreateProjectManagerMaps();
            CreateMeetingMaps();
            CreateBillingInfoMaps();
            CreateMonthlyPlannedMaps();
            CreateChangeRequestMaps();
            CreateAttachmentMaps();
            ProjectManager();
            Resources();
            UserRole();
            UserRole();
            CreateADUser();
            CreateADUserRoleMapping();
            CreateProjection();
            Menus();
        }

        #region Utilities
        protected virtual void CreateClientDetailMaps()
        {
            CreateMap<TPsm.ClientDetail, Amp.ClientDetailModel>()
               .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.ClientDetailModel, TPsm.ClientDetail>()
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());
        }
        protected virtual void CreateProjectManagerMaps()
        {
            CreateMap<TPsm.ProjectManagers, Amp.ProjectManagers>();
            CreateMap<Amp.ProjectManagers, TPsm.ProjectManagers>();
        }
        protected virtual void CreateMeetingMaps()
        {
            CreateMap<TPsm.Meetings, Amp.MeetingModel>()
                //.ForMember(x => x.ExternalKickOff, a => a.MapFrom(u => Convert.ToDateTime(u.ExternalKickOff).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.InternalKickOff, a => a.MapFrom(u => Convert.ToDateTime(u.InternalKickOff).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.PlannedUAT, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedUAT).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.PlannedLive, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedLive).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.ActualUAT, a => a.MapFrom(u => Convert.ToDateTime(u.ActualUAT).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.ActualLive, a => a.MapFrom(u => Convert.ToDateTime(u.ActualLive).ToString(ConstantValues.DDMMYYYY)))
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.MeetingModel, TPsm.Meetings>()
                //.ForMember(x => x.ExternalKickOff, a => a.MapFrom(u => Convert.ToDateTime(u.ExternalKickOff).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.InternalKickOff, a => a.MapFrom(u => Convert.ToDateTime(u.InternalKickOff).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.PlannedUAT, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedUAT).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.PlannedLive, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedLive).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.ActualUAT, a => a.MapFrom(u => Convert.ToDateTime(u.ActualUAT).ToString(ConstantValues.DDMMYYYY)))
                //.ForMember(x => x.ActualLive, a => a.MapFrom(u => Convert.ToDateTime(u.ActualLive).ToString(ConstantValues.DDMMYYYY)))
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());
        }
        protected virtual void CreateBillingInfoMaps()
        {
            CreateMap<TPsm.BillingInfo, Amp.BillingInfoModel>()
                 .ForMember(x => x.EndDate, a => a.MapFrom(u => Convert.ToDateTime(u.EndDate).ToString(ConstantValues.DDMMYYYY)))
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.BillingInfoModel, TPsm.BillingInfo>()
                .ForMember(x => x.EndDate, a => a.MapFrom(u => Convert.ToDateTime(u.EndDate).ToString(ConstantValues.DDMMYYYY)))
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());
        }
        protected virtual void CreateMonthlyPlannedMaps()
        {
            CreateMap<TPsm.MonthlyPlannedHrs, Amp.MonthlyPlannedModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.MonthlyPlannedModel, TPsm.MonthlyPlannedHrs>()
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());
        }
        protected virtual void CreateChangeRequestMaps()
        {
            CreateMap<TPsm.ChangeRequest, Amp.ChangeRequestModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.ChangeRequestModel, TPsm.ChangeRequest>()
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());

            CreateMap<TPsm.ChangeRequest, Amp.ChangeRequestResponseModel>()
                 .ForMember(x => x.PlannedStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.PlannedEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedEndDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualEndDate).ToString(ConstantValues.DDMMYYYY)));
            CreateMap<Amp.ChangeRequestResponseModel, TPsm.ChangeRequest>()
                 .ForMember(x => x.PlannedStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.PlannedEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedEndDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualEndDate).ToString(ConstantValues.DDMMYYYY)));
        }
        protected virtual void CreateAttachmentMaps()
        {
            CreateMap<Attachments, AttachmentModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<AttachmentModel, Attachments>()
                .ForMember(entity => entity.ModifiedOn, options => options.Ignore());
        }
        protected virtual void CreateGeneralDetailMaps()
        {
            CreateMap<TPsm.Projects, Amp.GeneralDetailModel>()
                 .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<Amp.GeneralDetailModel, TPsm.Projects>()
                 .ForMember(entity => entity.ModifiedOn, options => options.Ignore())
                 .ForMember(x => x.PlannedStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.PlannedEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.PlannedEndDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualStartDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualStartDate).ToString(ConstantValues.DDMMYYYY)))
                 .ForMember(x => x.ActualEndDate, a => a.MapFrom(u => Convert.ToDateTime(u.ActualEndDate).ToString(ConstantValues.DDMMYYYY)));

            CreateMap<TPsm.Projects, Amp.GeneralDetailResponseModel>();
            CreateMap<Amp.GeneralDetailResponseModel, TPsm.Projects>();
        }
        protected void ProjectManager()
        {
            CreateMap<TPsm.ProjectManagers, Amp.ProjectManagers>();
            CreateMap<Amp.ProjectManagers, TPsm.ProjectManagers>();
        }
        protected void Menus()
        {
            CreateMap<MainMenu, MainMenuModel>();
            CreateMap<MainMenuModel, MainMenu>();
        }
        protected void Resources()
        {
            CreateMap<ResourceDTO, Resource>();
            CreateMap<Resource, ResourceDTO>();
        }
        protected void UserRole()
        {
            CreateMap<UserRoles, UserRolesModel>();
            CreateMap<UserRolesModel, UserRoles>();
        }


        protected void CreateADUser()
        {
            CreateMap<ADUser, ADUserModel>();
            CreateMap<ADUserModel, ADUser>();
        }

        protected void CreateADUserRoleMapping()
        {
            CreateMap<ADUserRoleMapping, ADUserRoleMappingModel>();
            CreateMap<ADUserRoleMappingModel, ADUserRoleMapping>();
        }

        protected void CreateProjection()
        {
            CreateMap<Projection, ProjectionRequestModel>();
            CreateMap<ProjectionRequestModel, Projection>();
        }
         #endregion Utilities
        }
    }
