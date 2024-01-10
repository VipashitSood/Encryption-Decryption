using API.Models.POInformation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Factories.POInformation
{
	public interface IPOInfoFactory
    {
        Task<(bool, string, int)> CreatePoInformation(PoInfoModel model);

        Task<(bool, string, int)> UpdatePoInformation(UpdatePoInfoModel model);

        Task<IList<PoInfoSearchModel>> GetAllPoInformation(int poId, int clientId, string companyName, int pageIndex, int pageSize);

        Task<PoInfoFilterModel> PoInformationFilters();

        Task<IList<POInfoCustomOrderModel>> GetCustomerOrder(int customerId);

        Task<IList<POInfoCustomerModel>> GetCustomer();

        Task<PoInfoResponseModel> GetPoInformationById(int id);
    }
}
