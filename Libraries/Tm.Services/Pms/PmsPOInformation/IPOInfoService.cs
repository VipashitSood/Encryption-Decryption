using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.POInformation;

namespace Tm.Services.Pms.PmsPOInformation
{
	public interface IPOInfoService
    {

        #region PO Information
        /// <summary>
        /// Gets all POInformation 
        /// </summary>
        /// <returns>POInformation</returns>
        Task<IList<POInfo>> GetAllPOInfo(int poId = 0, int clientId = 0, string companyName = "");

        /// <summary>
        /// Gets POInformation mapping by Identifier
        /// </summary>
        /// <returns>POInformation</returns>
        Task<POInfo> GetPOInfoById(int id);

        /// <summary>
        /// Insert a POInformation
        /// </summary>
        Task InsertPOInfo(POInfo poInfo);

        /// <summary>
        /// Update a POInformation
        /// </summary>
        Task UpdatePOInfo(POInfo poInfo);

        /// <summary>
        /// Delete a POInformation
        /// </summary>
        Task DeletePOInfo(POInfo poInfo);
        #endregion

        #region POinfoOrderMapping
        Task<IList<POInfoOrderMapping>> GetPOInfoOrderMappingByPOInfoId(int poInfoId);

        Task InsertPOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping);

        Task UpdatePOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping);

        Task DeletePOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping);

        Task<POInfoOrderMapping> GetByPOInfoIdandOrderId(int poInfoId, int orderId);

        /// <summary>
        /// Gets POInfo OrderId by Identifier
        /// </summary>
        /// <returns>POInformation</returns>
        Task<IList<POInfoOrderMapping>> GetPOInfoByOrderId(int orderId);
        #endregion
    }
}
