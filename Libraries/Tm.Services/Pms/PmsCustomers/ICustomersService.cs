using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.PmsCustomers;

namespace Tm.Services.Pms.PmsCustomers
{
    public interface ICustomersService
    {
        Task<List<CustomerResponseModel>> GetAllCustomer(int pageNumber, int pageSize, string companyName = "", string customerName = "");
        Task<List<CustomerResponseModel>> GetAllCustomerWithoutPaging(string companyName = "", string customerName = "");

        Task<PmsCustomer> GetCustomerById(int id);


        Task InsertCustomer(PmsCustomer customerRoles);

        Task UpdateCustomer(PmsCustomer customerRoles);

        Task<PmsCustomer> DeleteCustomer(int id);

        Task<PmsCustomer> GetCustomerCompanyName(string companyname);
    }
}
