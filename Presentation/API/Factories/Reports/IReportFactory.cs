using API.Models.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Factories.Reports
{
    public interface IReportFactory
    {
        Task<FilterReportModel> ReportFilter();

        Task<List<MonthlyBillingInformationReport>> GetAllBillingInformationReportsMonthly(int? orderId, int? customerId, int? year, int? targetMonth);
    }
}
