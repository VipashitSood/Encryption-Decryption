using API.Controllers;
using API.Factories;
using API.Factories.BillingInformations;
using API.Factories.ChangeRequestDetail;
using API.Factories.Customer;
using API.Factories.GeneralDetail;
using API.Factories.MasterData;
using API.Factories.Menus;
using API.Factories.Orders;
using API.Factories.POInformation;
using API.Factories.ProjectDetail;
using API.Factories.Reports;
using API.Factories.RolePermissions;
using API.Factories.UserRole;
using Autofac;
using Tm.Core.Configuration;
using Tm.Core.Infrastructure;
using Tm.Core.Infrastructure.DependencyManagement;
using Tm.Services.Accounts;
using Tm.Services.Pms.GeneralDetail;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Menus;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.PmsMenuPerimission;
using Tm.Services.Pms.ProjectDetail;
using Tm.Services.Pms.Reports;
using Tm.Services.Pms.UserRole;

namespace API.Infrastructure
{
	/// <summary>
	/// Dependency registrar
	/// </summary>
	public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, TmConfig config)
        {
            //register service
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<MasterDataService>().As<IMasterDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectsService>().As<IProjectsService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralDetailService>().As<IGeneralDetailService>().InstancePerLifetimeScope();
            builder.RegisterType<PmsOrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomersService>().As<ICustomersService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleMenuPermissionService>().As<IRoleMenuPermissionService>().InstancePerLifetimeScope();

            //register factories
            builder.RegisterType<AccountModelFactory>().As<IAccountModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MasterDataFactory>().As<IMasterDataFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectsFactory>().As<IProjectsFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UserRoleFactory>().As<IUserRoleFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RolePermissionModelFactory>().As<IRolePermissionModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralDetailModelFactory>().As<IGeneralDetailModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<PmsCustomersFactory>().As<IPmsCustomersFactory>().InstancePerLifetimeScope();
            builder.RegisterType<PmsOrdersFactory>().As<IPmsOrdersFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeRequestFactory>().As<IChangeRequestFactory>().InstancePerLifetimeScope();
            builder.RegisterType<POInfoFactory>().As<IPOInfoFactory>().InstancePerLifetimeScope();
            builder.RegisterType<BillingInfoFactory>().As<IBillingInfoFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ReportFactory>().As<IReportFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MenuFactory>().As<IMenuFactory>().InstancePerLifetimeScope();
            builder.RegisterType<BillingInformationController>().InstancePerLifetimeScope();
            builder.RegisterType<BillingInfoFactory>().InstancePerLifetimeScope();

        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}