using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.Orders;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.Reports
{
	public partial class ReportService : IReportService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PmsOrders> _order;
        private readonly IRepository<BillingInformation> _billingInformationRepository;
        private readonly IRepository<BillingInfoPOMapping> _billingInfoPOMappingRepository;
        private readonly IRepository<PmsCustomer> _customerRepository;
        #endregion

        #region Ctor
        public ReportService(IEventPublisher eventPublisher,
            IRepository<PmsOrders> order,
            IRepository<BillingInformation> billingInformationRepository,
            IRepository<BillingInfoPOMapping> billingInfoPOMappingRepository,
            IRepository<PmsCustomer> customerRepository)
        {
            _eventPublisher = eventPublisher;
            _order = order;
            _billingInformationRepository = billingInformationRepository;
            _billingInfoPOMappingRepository = billingInfoPOMappingRepository;
            _customerRepository = customerRepository;
        }
        #endregion

        public async Task<IList<PmsOrders>> GetAllReports()
        {
            return await _order.Table.ToListAsync();
        }

        public async Task<PmsCustomer> GetOrderWithCustomerDetails(int orderId)
        {
            if (orderId != 0)
            {
                var query = _order.Table.Where(x => x.Id == orderId).First();

                if (query!=null)
                {
                    var customer = _customerRepository.Table.Where(x => x.Id == query.CustomerId);
                    return await customer.FirstOrDefaultAsync();
                }
            }

            // Handle the case where orderId is null or zero
            return null; // or handle it in a way that makes sense for your application
        }


    }
}

