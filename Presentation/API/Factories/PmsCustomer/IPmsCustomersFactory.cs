using API.Models.Customer;
using API.Models.PmsCustomer;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.PmsCustomers;

namespace API.Factories.Customer
{
	public interface IPmsCustomersFactory
    {
        #region Customers
        Task<List<ProjectName>> GetProjectDetailNames();
        Task<FilterCustomerModel> CustomerFilter();
        Task<List<CustomerResponseModel>> GetAllCustomer(int pageNumber, int pageSize, string companyName = "", string customerName = "");
        Task<(bool, string)> DeleteCustomer(int id);
        Task<UpdatePmsCustomerModel> GetCustomerById(int id);
        Task<(bool, string,int)> CreateCustomer(PmsCustomerModel customer);
        Task<(bool, string,int)> UpdateCustomer(UpdatePmsCustomerModel model);

        Task<IList<CustomerAttachmentModel>> GetCustomerAttachment(int id);

        #endregion Customers
    }
}
