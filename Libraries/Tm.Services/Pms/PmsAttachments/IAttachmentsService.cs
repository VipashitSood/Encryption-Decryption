using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.PmsAttachment;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Core.Domain.Pms.PmsOrders;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Core.Domain.Pms.ProjectDetail;

namespace Tm.Services.Pms.PmsAttachments
{
	public interface IAttachmentsService
	{
		#region Attachments
		Task<IList<Attachments>> GetAllAttachment();

		Task<Attachments> GetAttachmentById(int id);


		Task InsertAttachment(Attachments attachment);

		Task UpdateAttachment(Attachments attachment);

		Task<Attachments> DeleteAttachment(int id);
		#endregion

		#region Customer Attachment

		Task<IList<CustomerAttachment>> GetAllCustomerAttachment();

		Task<CustomerAttachment> GetCustomerAttachmentById(int id);

		Task InsertCustomerAttachment(CustomerAttachment customerAttachment);

		Task UpdateCustomerAttachment(CustomerAttachment customerAttachment);

		Task<IList<CustomerAttachment>> GetCustomerAttachmentByCustomerId(int customerId, bool msa, bool nda);

		Task<bool> DeleteCustomerAttachment(CustomerAttachment customerAttachment);

		#endregion

		#region Order Attachment
		Task<IList<OrderAttachment>> GetAllOrderAttachment();

		Task<OrderAttachment> GetOrderAttachmentById(int id);

		Task InsertOrderAttachment(OrderAttachment orderAttachment);

		Task UpdateOrderAttachment(OrderAttachment orderAttachment);

		Task<IList<OrderAttachment>> GetOrderAttachmentByOrderId(int orderId, bool isSwoDocument, bool isPoUpload);

		Task<bool> DeleteOrderAttachment(OrderAttachment orderAttachment);
		#endregion

		#region Project Attachment
		Task InsertProjectAttachment(ProjectAttachment projectAttachment);
		#endregion
		#region Change Request Attachment
		Task InsertChangeRequestAttachment(ChangeRequestAttachment changeRequestAttachment);
		Task<IList<ChangeRequestAttachment>> GetChangeRequestAttachmentsByCRId(int crId);

		#endregion

		#region PO Information Attachment

		/// <summary>
		/// Get POInfoAttachment By Id 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		Task<POInfoAttachment> GetPOInfoAttachmentByPoInfoId(int poId);

		/// <summary>
		/// Create POInfoAttachment
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		Task InsertPOInfoAttachment(POInfoAttachment pOInfoAttachment);

		/// <summary>
		/// Update POInfoAttachment
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		Task UpdatePOInfoAttachment(POInfoAttachment pOInfoAttachment);

		/// <summary>
		///  Get All PoInfo attachment by its PoInfo Id
		/// </summary>
		/// <param name="poId"></param>
		/// <returns></returns>
		Task<IList<POInfoAttachment>> GetPOInforAttachmentByPOInfoId(int poId);

		/// <summary>
		///  Delete PoInfo attachment
		/// </summary>
		/// <returns></returns>
		Task<bool> DeletePOInfoAttachment(POInfoAttachment poInfoAttachment);
		#endregion
	}
}
