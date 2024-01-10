using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.POInformation;

namespace Tm.Services.Pms.BillingInformations
{
	public interface IBillingInfoService
    {

        #region Billing Information
        /// <summary>
        /// Gets all BillingInformation 
        /// </summary>
        /// <returns>POInformation</returns>
        Task<IList<BillingInformation>> GetAllBillingInformation(int orderId);

        /// <summary>
        /// Gets BillingInformation mapping by Identifier
        /// </summary>
        /// <returns>POInformation</returns>
        Task<BillingInformation> GetBillingInformationById(int id);

        /// <summary>
        /// Insert a BillingInformation
        /// </summary>
        Task InsertBillingInformation(BillingInformation billingInformation);

        /// <summary>
        /// Update a BillingInformation
        /// </summary>
        Task UpdateBillingInformation(BillingInformation billingInformation);

        /// <summary>
        /// Delete a BillingInformation
        /// </summary>
        Task DeleteBillingInformation(BillingInformation billingInformation);
        Task<BillingInfoPOMapping> GetBillingInfoPOMappingByIds(int poId);
        #endregion

        #region BillingInfoPOMapping

        Task<BillingInfoPOMapping> GetBillingInfoPOMappingById(int id);

        Task<IList<BillingInfoPOMapping>> GetBillingInfoPOMappingByBillingInfoId(int billingInfoId);

        Task InsertBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping);

        Task UpdateBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping);

        Task DeleteBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping);

        Task<POInfo> GetPONumberByPOValue(string poNumber);


        Task<BillingInfoPOMapping> GetBillingInfoPOMappingBybillingInfoandPoId(int billingPoInfoId, int poid);

        #endregion
    }
}
