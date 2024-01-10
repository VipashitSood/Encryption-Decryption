using System;
using System.Collections.Generic;
using System.Text;

namespace Tm.Core.Constants
{
    public static class ConstantValues
    {
        public const string Success = "Successfully Done";
        public const string ClientId = "client_id";
        public const string Scope = "scope";
        public const string RedirectUri = "redirect_uri";
        public const string GrantType = "grant_type";
        public const string ClientSecret = "client_secret";
        public const string Code = "code";
        public const string Bearer = "Bearer";
        public const string InvalidManagerId = "Invalid manager Id";
        public const string InvalidProjectId = "Invalid project Id";
        public const string InvalidId = "Invalid ID";
        public const string RecordNotFound = "Record not found.";
        public const string ProjectNotFound = "Project not found.";
        public const string NoRecordFoundForUpdate = "No record found for update";
        public const string CantUpdateRecord = "Can't update record";
        public const string ErrorFetchingChangeRequest = "Error while fetching change request";
        public const string DDMMYYYY = "dd/MM/yyyy";
        public const string ErrorFetchingBillingInfo = "Error while fetching billing information.";
        public const string ErrorFetchingMeetingInfo = "Error while fetching meeting details.";
        public const string ErrorFetchingClientInfo = "Error while fetching client details.";
        public const string ErrorFetchingMonthlyPlanned = "Error while fetching monthly planned hours.";
        public const string ErrorFetchingProjectData = "An error occurred while fetching project data.";
        public const string NoClientExist = "No Client Exist";
        public const string NoProjectNameExist = "No Project Name Exist";
        public const string NoProjectTypeExist = "No Project Type Exist";
        public const string UnknownUser = "";
        public const string AdCount = "$top";
        public const string NoAttachment = "No Attachment Found";
        public const string ErrorFetchingUserRole = "No User Role Found";
        public const string NoRecord = "No Record Found";
        public const string DuplicateUserRole = "User Role Already Added";
        public const string DuplicatePriority = "Priority already exist. please enter different priority.";
        public const string CustomerOrderExist = "Customer Contain Orders";
        public const string ProjectionError = "Error in projection while fetching";
        public const string ProjectionDateConflictError = "Projection already exist.";
        public const string ProjectionNotFoundError = "Projection not found.";
        public const string CustomerNameAlreadyExist = "Customer Name already exists";
        public const string CustomerPhoneAlreadyExist = "Customer Phone Number already exists";
        public const string AttendanceAlreadyExistError = "Attendance already exist.";
        public const string AttendanceCopyError = "Error occured in copy projection.";
        public const string ProjectCostError = "Project cost exceding estimated efforts.";
        public const string ProjectHoursError = "Project hours exceding estimated efforts.";
    }
}
