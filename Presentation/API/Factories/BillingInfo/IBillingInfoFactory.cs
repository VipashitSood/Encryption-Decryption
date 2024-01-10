using API.Models.BillingInformation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Factories.BillingInformations
{
	public interface IBillingInfoFactory
    {
		/// <summary>
		/// Create Billing Information
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<(bool, string, int)> CreateBillingInformation(BillingInformationModel model);
		/// <summary>
		/// Get Billing Info Order
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IList<BillingInfoOrderModel>> GetBillingInfoOrder();

        /// <summary>
        /// Get Order POInfo List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IList<OrderPOInfoModel>> GetCustomerPOList(int customerId);

		/// <summary>
		///  Billing info
		/// </summary>
		/// <returns></returns>
		Task<IList<BillingInformationListModel>> GetAllBillingInfo(int customerId);

        /// <summary>
        ///  Billing Info Action Update
        /// </summary>
        /// <param name="billingInfoId"></param>
        /// <param name="managerAction"></param>
        /// <param name="dHAction"></param>
        /// <param name="aHAction"></param>
        /// <param name="raiseDate"></param>
        /// <param name="actualBilling"></param>
        /// <returns></returns>
        Task<(bool, string, int)> BillingInfoAction(int billingInfoId, int managerAction, int dHAction, int aHAction, DateTime? raiseDate, decimal actualBilling);

		/// <summary>
		/// Get billing Info By Id
		/// </summary>
		/// <param name="billingInfoId"></param>
		/// <returns></returns>

		Task<GetBillingInformationModel> GetBillingInfoById(int billingInfoId);

		/// <summary>
		/// Update Billing Information
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<(bool, string, int)> UpdateBillingInformation(UpdateBillingInformationModel model);

		/// <summary>
		///  Add PO BillingInfo
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		Task<(bool, string, int)> AddPOBillingInfo(IList<AddBillingInfoPOModel> model);
	}
}
