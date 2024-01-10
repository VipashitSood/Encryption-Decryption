using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Orders;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.Orders
{
    public partial class PmsOrderService : IOrderService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PmsOrders> _order;
        #endregion

        #region Ctor
        public PmsOrderService(IEventPublisher eventPublisher,
            IRepository<PmsOrders> order)
        {
            _eventPublisher = eventPublisher;
            _order = order;
        }

        #endregion
        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns></returns>
        public async Task<IList<PmsOrders>> GetAllOrders()
        {
            return await _order.Table.ToListAsync();
        }

        /// <summary>
        /// Filter Data in Order
        /// </summary>
        /// <returns></returns>
        public async Task<IList<PmsOrders>> OrderFilter()
        {
            return await _order.Table.ToListAsync();
        }

        /// <summary>
        /// Get Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PmsOrders> GetOrdersById(int id)
        {
            if (id == 0)
                return null;

            var query = _order.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = query.FirstOrDefault();

            return result;
        }

/// <summary>
/// 
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public async Task<PmsOrders> GetCustomerOrderById(int id)
        {
            if (id == 0)
                return null;

            var query = _order.Table.Where(x => x.CustomerId == id && x.IsDeleted == false);

            var result = query.FirstOrDefault();

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public PmsOrders GetOrdersByIdWithoutAsync(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var query = _order.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = query.FirstOrDefault();

            return result;
        }



        /// <summary>
        /// Insert Order
        /// </summary>
        /// <param name="orderRoles"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertOrder(PmsOrders orderRoles)
        {
            if (orderRoles == null)
                throw new ArgumentNullException(nameof(orderRoles));

            await _order.InsertAsync(orderRoles);

            //event notification
            _eventPublisher.EntityInserted(orderRoles);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderRoles"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateOrder(PmsOrders orderRoles)
        {
            if (orderRoles == null)
                throw new ArgumentNullException(nameof(orderRoles));

            await _order.UpdateAsync(orderRoles);

            //event notification
            _eventPublisher.EntityUpdated(orderRoles);
        }

        /// <summary>
        /// Delete Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PmsOrders> DeleteOrder(int id)
        {
            if (id == 0)
                return null;

            var order = _order.GetByIdAsync(id);

            return await order;
        }

        /// <summary>
        /// Get Order By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IList<PmsOrders>> GetCustomerOrdersById(int customerId)
        {
            if (customerId == 0)
                throw new ArgumentNullException(nameof(customerId));

            var query = _order.Table.Where(x => x.CustomerId == customerId && x.IsDeleted == false);

            var result = await query.ToListAsync();

            return result;
        }
    }
}
