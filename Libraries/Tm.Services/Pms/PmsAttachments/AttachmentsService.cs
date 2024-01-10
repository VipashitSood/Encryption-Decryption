using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Core.Domain.Pms.PmsOrders;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.PmsAttachments
{
    public partial class AttachmentsService : IAttachmentsService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Attachments> _attachment;
        private readonly IRepository<CustomerAttachment> _customerAttachment;
        private readonly IRepository<OrderAttachment> _orderAttachment;
        private readonly IRepository<ProjectAttachment> _projectAttachment;
        private readonly IRepository<ChangeRequestAttachment> _changeRequestAttachment;
        private readonly IRepository<POInfoAttachment> _pOInfoAttachment;
        #endregion

        #region Ctor
        public AttachmentsService(IEventPublisher eventPublisher,
            IRepository<Attachments> attachment,
            IRepository<CustomerAttachment> customerAttachment,
            IRepository<OrderAttachment> orderAttachment,
             IRepository<ProjectAttachment> projectAttachment,
             IRepository<ChangeRequestAttachment> changeRequestAttachment,
             IRepository<POInfoAttachment> pOInfoAttachment)
        {
            _eventPublisher = eventPublisher;
            _attachment = attachment;
            _customerAttachment = customerAttachment;
            _orderAttachment = orderAttachment;
            _projectAttachment = projectAttachment;
            _changeRequestAttachment = changeRequestAttachment;
            _pOInfoAttachment = pOInfoAttachment;
        }
        #endregion

        #region Attachment

        /// <summary>
        /// Get All Attachment
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Attachments>> GetAllAttachment()
        {
            // Query your database table
            var query = _attachment.Table;

            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get Attachment By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Attachments> GetAttachmentById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var query = _attachment.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertAttachment(Attachments attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            await _attachment.InsertAsync(attachment);

            //event notification
            _eventPublisher.EntityInserted(attachment);
        }

        /// <summary>
        /// Update Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateAttachment(Attachments attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            await _attachment.UpdateAsync(attachment);

            //event notification
            _eventPublisher.EntityUpdated(attachment);
        }

        /// <summary>
        /// Delete Attachment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Attachments> DeleteAttachment(int id)
        {
            if (id == 0)
                return null;

            var attachment = _attachment.GetByIdAsync(id);

            return await attachment;
        }

        #endregion

        #region Customer Attachment 
        /// <summary>
        /// Get All Customer Attachment
        /// </summary>
        /// <returns></returns>
        public async Task<IList<CustomerAttachment>> GetAllCustomerAttachment()
        {
            // Query your database table
            var query = _customerAttachment.Table;

            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get Customer Attachment By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<CustomerAttachment> GetCustomerAttachmentById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var query = _customerAttachment.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertCustomerAttachment(CustomerAttachment customerAttachment)
        {
            if (customerAttachment == null)
                throw new ArgumentNullException(nameof(customerAttachment));

            await _customerAttachment.InsertAsync(customerAttachment);

            //event notification
            _eventPublisher.EntityInserted(customerAttachment);
        }


        /// <summary>
        /// Update Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateCustomerAttachment(CustomerAttachment customerAttachment)
        {
            if (customerAttachment == null)
                throw new ArgumentNullException(nameof(customerAttachment));

            await _customerAttachment.UpdateAsync(customerAttachment);

            //event notification
            _eventPublisher.EntityInserted(customerAttachment);
        }

        /// <summary>
        ///  Get customer attachment by its customer Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="msa"></param>
        /// <param name="nda"></param>
        /// <returns></returns>
        public async Task<IList<CustomerAttachment>> GetCustomerAttachmentByCustomerId(int customerId, bool msa, bool nda)
        {
            // Query your database table
            var query = _customerAttachment.Table;
            query = query.Where(x => x.IsDeleted == false && x.CustomerId == customerId);
            if (msa)
            {
                query = query.Where(x => x.MSA == msa);
            }
            if (nda)
            {
                query = query.Where(x => x.NDA == nda);
            }
            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        ///  Delete Customer Attachment
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="msa"></param>
        /// <param name="nda"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCustomerAttachment(CustomerAttachment customerAttachment)
        {
            if (customerAttachment == null)
                throw new ArgumentNullException(nameof(customerAttachment));

            await _customerAttachment.DeleteAsync(customerAttachment);

            //event notification
            _eventPublisher.EntityInserted(customerAttachment);

            return true;
        }
        #endregion

        #region Order Attachment 
        /// <summary>
        /// Get All Order Attachment
        /// </summary>
        /// <returns></returns>
        public async Task<IList<OrderAttachment>> GetAllOrderAttachment()
        {
            // Query your database table
            var query = _orderAttachment.Table;

            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        /// Get Order Attachment By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<OrderAttachment> GetOrderAttachmentById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var query = _orderAttachment.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertOrderAttachment(OrderAttachment orderAttachment)
        {
            if (orderAttachment == null)
                throw new ArgumentNullException(nameof(orderAttachment));

            await _orderAttachment.InsertAsync(orderAttachment);

            //event notification
            _eventPublisher.EntityInserted(orderAttachment);
        }

        /// <summary>
        /// Update Attachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateOrderAttachment(OrderAttachment orderAttachment)
        {
            if (orderAttachment == null)
                throw new ArgumentNullException(nameof(orderAttachment));

            await _orderAttachment.UpdateAsync(orderAttachment);

            //event notification
            _eventPublisher.EntityInserted(orderAttachment);
        }

        /// <summary>
        ///  Get order attachment by its order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="isSwoDocument"></param>
        /// <param name="isPoUpload"></param>
        /// <returns></returns>
        public async Task<IList<OrderAttachment>> GetOrderAttachmentByOrderId(int orderId, bool isSwoDocument, bool isPoUpload)
        {
            // Query your database table
            var query = _orderAttachment.Table;
            query = query.Where(x => x.IsDeleted == false && x.OrderId == orderId);
            if (isSwoDocument)
            {
                query = query.Where(x => x.IsSOWDocument == isSwoDocument);
            }
            if (isPoUpload)
            {
                query = query.Where(x => x.IsPOUpload == isPoUpload);
            }

            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        ///  Delete Customer Attachment
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="msa"></param>
        /// <param name="nda"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrderAttachment(OrderAttachment orderAttachment)
        {
            if (orderAttachment == null)
                throw new ArgumentNullException(nameof(orderAttachment));

            await _orderAttachment.DeleteAsync(orderAttachment);

            //event notification
            _eventPublisher.EntityInserted(orderAttachment);

            return true;
        }
        #endregion

        #region Project Attachment
        /// <summary>
        /// Insert data to  project Attachments
        /// </summary>
        /// <param name="projectAttachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertProjectAttachment(ProjectAttachment projectAttachment)
        {
            if (projectAttachment == null)
                throw new ArgumentNullException(nameof(projectAttachment));

            await _projectAttachment.InsertAsync(projectAttachment);

            //event notification
            _eventPublisher.EntityInserted(projectAttachment);
        }

        #endregion

        #region Change Request Attachment
        /// <summary>
        /// Insert data to  project Attachments
        /// </summary>
        /// <param name="projectAttachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertChangeRequestAttachment(ChangeRequestAttachment changeRequestAttachment)
        {
            if (changeRequestAttachment == null)
                throw new ArgumentNullException(nameof(changeRequestAttachment));

            await _changeRequestAttachment.InsertAsync(changeRequestAttachment);

            //event notification
            _eventPublisher.EntityInserted(changeRequestAttachment);
        }
        /// <summary>
        /// Get CR attachments by CRID
        /// </summary>
        /// <param name="crId"></param>
        /// <returns></returns>
        public async Task<IList<ChangeRequestAttachment>> GetChangeRequestAttachmentsByCRId(int crId)
        {
            // Query your database table
            var query = _changeRequestAttachment.Table;

            query=query.Where(x => x.IsDeleted == false && x.CRId == crId);

            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }
        #endregion

        #region PO Information Attachment


        /// <summary>
        /// Get POInfoAttachment By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<POInfoAttachment> GetPOInfoAttachmentByPoInfoId(int poId)
        {
            if (poId == 0)
                throw new ArgumentNullException(nameof(poId));

            var query = _pOInfoAttachment.Table.Where(x => x.POInfoId == poId && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create POInfoAttachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertPOInfoAttachment(POInfoAttachment pOInfoAttachment)
        {
            if (pOInfoAttachment == null)
                throw new ArgumentNullException(nameof(pOInfoAttachment));

            await _pOInfoAttachment.InsertAsync(pOInfoAttachment);

            //event notification
            _eventPublisher.EntityInserted(pOInfoAttachment);
        }

        /// <summary>
        /// Update POInfoAttachment
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdatePOInfoAttachment(POInfoAttachment pOInfoAttachment)
        {
            if (pOInfoAttachment == null)
                throw new ArgumentNullException(nameof(pOInfoAttachment));

            await _pOInfoAttachment.UpdateAsync(pOInfoAttachment);

            //event notification
            _eventPublisher.EntityInserted(pOInfoAttachment);
        }

        /// <summary>
        ///  Get PoInfo attachment by its PoInfo Id
        /// </summary>
        /// <param name="poId"></param>
        /// <returns></returns>
        public async Task<IList<POInfoAttachment>> GetPOInforAttachmentByPOInfoId(int poId)
        {
            // Query your database table
            var query = _pOInfoAttachment.Table;
            query = query.Where(x => x.IsDeleted == false && x.POInfoId == poId);
            // Execute the query and return the filtered results
            return await query.ToListAsync();
        }

        /// <summary>
        ///  Delete PoInfo attachment
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeletePOInfoAttachment(POInfoAttachment poInfoAttachment)
        {
            if (poInfoAttachment == null)
                throw new ArgumentNullException(nameof(poInfoAttachment));

            await _pOInfoAttachment.DeleteAsync(poInfoAttachment);

            //event notification
            _eventPublisher.EntityInserted(poInfoAttachment);

            return true;
        }
        #endregion
    }
}
