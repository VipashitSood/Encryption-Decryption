using API.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Orders;

namespace API.Factories.Orders
{
    public interface IPmsOrdersFactory
    {
        Task<IList<OrderListModel>> GetAllOrders(int customerId,
         int orderId,
         int sowDocumentId,
         bool? isPoRequired,bool? inHouse);
        Task<FilteredOrderModel> OrderFilter();
        Task<(bool, string)> DeleteOrders(int id);
        Task<OrderModel> GetOrdersById(int id);
        Task<(bool, string, int)> CreateOrder(IList<OrderModel> orderModels);
        Task<(bool, string, int)> UpdateOrder(OrderModel model);
    }
}
