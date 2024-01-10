using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Orders;

namespace Tm.Services.Pms.Orders
{
    public interface IOrderService
    {
        Task<IList<PmsOrders>> GetAllOrders();
        
        Task<IList<PmsOrders>> OrderFilter();

        Task<PmsOrders> GetOrdersById(int id);
        Task<PmsOrders> GetCustomerOrderById(int id);
        PmsOrders GetOrdersByIdWithoutAsync(int id);

        Task InsertOrder(PmsOrders orderRoles);

        Task UpdateOrder(PmsOrders orderRoles);

        Task<PmsOrders> DeleteOrder(int id);

        Task<IList<PmsOrders>> GetCustomerOrdersById(int customerId);
    }
}
