using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsCustomers;
using static Tm.Services.Pms.Reports.ReportService;

namespace Tm.Services.Pms.Reports
{
	public  interface IReportService
   
    {
        Task<IList<PmsOrders>> GetAllReports();

        Task<PmsCustomer> GetOrderWithCustomerDetails(int orderId);
    }
}
