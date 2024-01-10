using System;
using System.Collections.Generic;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.Attendance;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Core.Domain.Pms.PmsMenu;
using Tm.Core.Domain.Pms.PmsOrders;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Core.Domain.Pms.UserRole;
using Tm.Core.Domain.Security;
using Tm.Core.Domain.Users;

namespace Tm.Data.Mapping
{
	/// <summary>
	/// Base instance of backward compatibility of table naming
	/// </summary>
	public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            { typeof(CustomerAddressMapping), "CustomerAddresses" },
            { typeof(CustomerCustomerRoleMapping), "Customer_CustomerRole_Mapping" },
            { typeof(UserScheduleMapping), "User_Schedule_Mapping" },
            { typeof(PermissionRecordCustomerRoleMapping), "PermissionRecord_Role_Mapping" },
            { typeof(Currency), "PMS_Currency_Master" },
            { typeof(TechStack), "PMS_TechStack_Master" },
            { typeof(ProjectType), "PMS_ProjectType_Master" },
            { typeof(ProjectStatus), "PMS_ProjectStatus_Master" },
            { typeof(ProjectDomain), "PMS_ProjectDomain_Master" },
            { typeof(Projects), "PMS_Projects" },
            { typeof(BillingInfo), "PMS_Project_Billing_Info" },
            { typeof(ChangeRequest), "PMS_Project_Change_Request" },
            { typeof(ClientDetail), "PMS_Project_Client_Detail" },
            { typeof(Meetings), "PMS_Project_Meetings" },
            { typeof(TechStackMapping), "PMS_Project_Tech_Stack_Mapping" },
            { typeof(Attachments), "Attachments" },
            { typeof(TechDepartment), "PMS_TechDepart_Master" },
            { typeof(ProjectName), "PMS_ProjectName_Master" },
            { typeof(CommunicationMode), "PMS_CommunicationMode_Master" },
            { typeof(MonthlyPlannedHrs), "PMS_Monthly_Billable_Info" },
            { typeof(ProjectManagers), "Project_Managers" },
            { typeof(Resource), "PMS_Project_Resource_Mapping" },
            { typeof(OtherInfo), "PMS_Other_Info" },
            { typeof(UserRoles), "PMS_User_Roles" },
            { typeof(ADUser), "PMS_Ad_User" },
            { typeof(ADUserRoleMapping), "PMS_Ad_UserRoleMapping" },
            { typeof(UserModules), "PMS_UserModules" },
            { typeof(PermissionModuleRecord), "PMS_PermissionModuleRecord" },
            { typeof(RoleModulePermissionMapping), "PMS_RoleModulePermissionMapping" },
            { typeof(PmsCustomer), "PMS_Customer" },
            { typeof(PmsOrders), "PMS_Orders" },
            { typeof(CustomerAttachment), "PMS_CustomerAttachments" },
            { typeof(OrderAttachment), "PMS_OrderAttachments" },
            { typeof(ProjectAttachment), "PMS_ProjectAttachments" },
            { typeof(ChangeRequestAttachment), "PMS_ChangeRequestAttachments" },
            { typeof(Tm.Core.Domain.Pms.MasterData.ResourceType), "ResourceType" },
            { typeof(Projection), "Projection" },
            { typeof(Attendance), "Attendance" },
            { typeof(POInfo), "PMS_POInfo" },
            { typeof(POInfoOrderMapping), "PMS_POInfoOrderMapping" },
            { typeof(POInfoAttachment), "PMS_POInfoAttachment" },
            { typeof(RefreshToken), "PMS_RefreshToken" },
            { typeof(ADAttendance), "AD_Attendance" },
            { typeof(BillingInformation), "PMS_BillingInfo" },
            { typeof(BillingInfoPOMapping), "PMS_BillingInfoPOMapping" },
            { typeof(MainMenu), "PMS_Menu_Items" },
            { typeof(SubMenu), "PMS_Sub_Menu_Items" },
            { typeof(ChildMenu), "PMS_Child_Menu_Items" },
            { typeof(RoleMenuPermissionMapping), "PMS_RoleMenuPermissionMapping" },
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {
            { (typeof(Customer), "BillingAddressId"), "BillingAddress_Id" },
            { (typeof(Customer), "ShippingAddressId"), "ShippingAddress_Id" },
            { (typeof(CustomerCustomerRoleMapping), "CustomerId"), "Customer_Id" },
            { (typeof(CustomerCustomerRoleMapping), "CustomerRoleId"), "CustomerRole_Id" },
            { (typeof(PermissionRecordCustomerRoleMapping), "PermissionRecordId"), "PermissionRecord_Id" },
            { (typeof(PermissionRecordCustomerRoleMapping), "CustomerRoleId"), "CustomerRole_Id" },
            { (typeof(CustomerAddressMapping), "AddressId"), "Address_Id" },
            { (typeof(CustomerAddressMapping), "CustomerId"), "Customer_Id" },
        };
    }
}