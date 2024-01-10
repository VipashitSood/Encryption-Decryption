using API.Models.Reports;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Data;
using Tm.Services.Customers;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.Orders;
using Tm.Services.Pms.PmsCustomers;
using Tm.Services.Pms.Reports;

namespace API.Factories.Reports
{
    public class ReportFactory : IReportFactory

    {
        #region Fields
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private readonly ICustomersService _customerService;
        private readonly IRepository<BillingInformation> _billingInformationRepository;
        private readonly IRepository<BillingInfoPOMapping> _billingInfoPOMappingRepository;
        private readonly IMasterDataService _masterDataService;
        private static IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public ReportFactory(IReportService reportService,
            IOrderService orderService,
            ICustomersService customerService,
             IRepository<BillingInformation> billingInformationRepository,
            IRepository<BillingInfoPOMapping> billingInfoPOMappingRepository,
            IMasterDataService masterDataService,
            IHttpContextAccessor httpContextAccessor)
        {
            _reportService = reportService;
            _orderService = orderService;
            _customerService = customerService;
            _billingInformationRepository = billingInformationRepository;
            _billingInfoPOMappingRepository = billingInfoPOMappingRepository;
            _masterDataService = masterDataService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        public async Task<FilterReportModel> ReportFilter()
        {
            // Initialize the FilteredOrderModel
            var filterreport = new FilterReportModel();

            // Create a static SelectListItem with value 0 for all items
            var orderName = new SelectListItem { Text = "Search By Order", Value = "0" };
            var Months = new SelectListItem { Text = "Search By Month", Value = "0" };
            var CompanyName = new SelectListItem { Text = "Search By Company", Value = "0" };
            var Year = new SelectListItem { Text = "Search By Year", Value = "0" };

            // Get a list of all order names
            var orderNames = await _orderService.GetAllOrders();
            filterreport.OrderName.Add(orderName);
            filterreport.OrderName.AddRange(orderNames.Select(order =>
                new SelectListItem { Text = order.OrderName ?? "", Value = order.Id.ToString() }));

            // Add a static SelectListItem for CompanyName
            var CompanyNames = await _customerService.GetAllCustomerWithoutPaging(string.Empty);
            filterreport.CompanyName.Add(CompanyName);
            filterreport.CompanyName.AddRange(CompanyNames.Select(userModule =>
                new SelectListItem { Text = (userModule.Company != null ? userModule.Company.ToString() : ""), Value = userModule.Id.ToString() }));

            // Create a static "Select" option for "Month"
            AddSelectOption(filterreport.Months, "Month");

            // Add months
            filterreport.Months.Add(new SelectListItem { Text = "January", Value = "1" });
            filterreport.Months.Add(new SelectListItem { Text = "February", Value = "2" });
            filterreport.Months.Add(new SelectListItem { Text = "March", Value = "3" });
            filterreport.Months.Add(new SelectListItem { Text = "April", Value = "4" });
            filterreport.Months.Add(new SelectListItem { Text = "May", Value = "5" });
            filterreport.Months.Add(new SelectListItem { Text = "June", Value = "6" });
            filterreport.Months.Add(new SelectListItem { Text = "July", Value = "7" });
            filterreport.Months.Add(new SelectListItem { Text = "August", Value = "8" });
            filterreport.Months.Add(new SelectListItem { Text = "September", Value = "9" });
            filterreport.Months.Add(new SelectListItem { Text = "October", Value = "10" });
            filterreport.Months.Add(new SelectListItem { Text = "November", Value = "11" });
            filterreport.Months.Add(new SelectListItem { Text = "December", Value = "12" });

            // Create a dynamic list of years
            AddSelectOption(filterreport.Year, "Year");
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 8; i <= currentYear + 1; i++)
            {
                filterreport.Year.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            // Create a static "Select" option for "Year"


            // Return the filteredOrderModel
            return filterreport;
        }

        // Helper function to add "Select" option
        private void AddSelectOption(List<SelectListItem> list, string labelText)
        {
            list.Add(new SelectListItem
            {
                Text = "Search By " + labelText,
                Value = "0"
            });
        }



        public async Task<List<MonthlyBillingInformationReport>> GetAllBillingInformationReportsMonthly(int? orderId, int? customerId, int? year, int? targetMonth)
        {
            var query = _billingInformationRepository.Table.Where(x => !x.IsDeleted);

            if (orderId.HasValue && orderId.Value != 0)
            {
                query = query.Where(x => x.OrderId == orderId.Value);
            }

            if (year.HasValue && year.Value != 0)
            {
                query = query.Where(x => x.CreatedOn.Year == year);
            }

            if (targetMonth.HasValue && targetMonth.Value != 0)
            {
                query = query.Where(x => x.CreatedOn.Month == targetMonth);
            }

            var result = query
                .GroupBy(x => new BillingGroupKey { OrderId = x.OrderId, Year = x.CreatedOn.Year })
                .Select(group => new MonthlyBillingInformationReport
                {
                    OrderId = group.Key.OrderId,
                    OrderNumber = 0,
                    CompanyName = String.Empty,
                    Year = group.Key.Year,
                    January = CalculateMonthStatus(group, 1),
                    February = CalculateMonthStatus(group, 2),
                    March = CalculateMonthStatus(group, 3),
                    April = CalculateMonthStatus(group, 4),
                    May = CalculateMonthStatus(group, 5),
                    June = CalculateMonthStatus(group, 6),
                    July = CalculateMonthStatus(group, 7),
                    August = CalculateMonthStatus(group, 8),
                    September = CalculateMonthStatus(group, 9),
                    October = CalculateMonthStatus(group, 10),
                    November = CalculateMonthStatus(group, 11),
                    December = CalculateMonthStatus(group, 12)
                })
                .ToList();

            foreach (var report in result)
            {
                var orderResult = _orderService.GetOrdersByIdWithoutAsync(report.OrderId);
                if (orderResult != null)
                {
                    var company = _reportService.GetOrderWithCustomerDetails(orderResult.Id);
               
                    report.OrderNumber = (int)(orderResult?.OrderNumber);
                    report.CompanyName = company?.Result?.Company;
                }
            }
            return result;
        }

        private MonthStatus CalculateMonthStatus(IGrouping<BillingGroupKey, BillingInformation> group, int month)
        {
            var currencyPath = string.Empty;
            bool anyRed = group.Any(x => x.ToBeRaised.HasValue && x.ToBeRaised.Value.Month == month && x.RaiseDate == null);
            bool anyGreen = group.Any(x => x.ToBeRaised.HasValue && x.ToBeRaised.Value.Month == month && x.RaiseDate != null);
            var currencyId = group.Select(x => x.CurrencyId).FirstOrDefault();

            Currency currencyDetail = _masterDataService.GetCurrencyById(Convert.ToInt32(currencyId));
            if (currencyDetail != null)
            {
                currencyPath = string.Concat(_httpContextAccessor.HttpContext.Request.Scheme, "://", _httpContextAccessor.HttpContext.Request.Host, "\\", currencyDetail.Icon).Replace("\\", "/");
            }

            if (anyRed)
            {
                return new MonthStatus
                {
                    Status = "#FFABAB",
                    ProjectCost = group
                        .Where(x => x.ToBeRaised.HasValue && x.ToBeRaised.Value.Month == month)
                        .Sum(x => x.ProjectCost),
                    CurrencyPath = currencyPath,
                };
            }
            else if (anyGreen)
            {
                return new MonthStatus
                {
                    Status = "#D2FFD1",
                    ProjectCost = group
                        .Where(x => x.ToBeRaised.HasValue && x.ToBeRaised.Value.Month == month)
                        .Sum(x => x.ProjectCost),
                    CurrencyPath = currencyPath,
                };
            }
            else
            {
                return new MonthStatus
                {
                    Status = " #FFFF9F",
                    ProjectCost = group
                        .Where(x => x.ToBeRaised.HasValue && x.ToBeRaised.Value.Month == month)
                        .Sum(x => x.ProjectCost),
                    CurrencyPath = currencyPath
                };
            }
        }


    }
}