using API.Models.Attendance;
using LinqToDB;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Common;
using Tm.Core.Domain.Localization;
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.PmsCustomers;
using Tm.Core.Domain.Pms.Projection;
using Tm.Data;
using Tm.Services.Common;
using Tm.Services.Events;

namespace Tm.Services.Pms.PmsCustomers
{
	public partial class CustomersService : ICustomersService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PmsCustomer> _customer;
        private readonly IRepository<CustomerResponseModel> _iCustomerModelRepository;
        
        #endregion

        #region Ctor
        public CustomersService(IEventPublisher eventPublisher,
            IRepository<PmsCustomer> customer, IRepository<CustomerResponseModel> iCustomerModelRepository)
        {
            _eventPublisher = eventPublisher;
            _customer = customer;
            _iCustomerModelRepository = iCustomerModelRepository;
        }
        #endregion

        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<CustomerResponseModel>> GetAllCustomer(int pageNumber,int pageSize,string companyName = "", string customerName = "")
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            var pCompanyName = SqlParameterHelper.GetStringParameter("CompanyName", companyName);
            var pCustomerName = SqlParameterHelper.GetStringParameter("CustomerName", customerName);
            return await Task.Factory.StartNew<List<CustomerResponseModel>>(() =>
            {
                List<CustomerResponseModel> customerResponseModelList = _iCustomerModelRepository.EntityFromSql("SSP_GetAllCustomers",
                    pPageNumber,
                    pPageSize,
                    pCompanyName,
                    pCustomerName).ToList();
                return customerResponseModelList;
            });
        }

        // <summary>
        /// Get All Customer
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<List<CustomerResponseModel>> GetAllCustomerWithoutPaging(string companyName = "", string customerName = "")
        {
            var pCompanyName = SqlParameterHelper.GetStringParameter("CompanyName", companyName);
            var pCustomerName = SqlParameterHelper.GetStringParameter("CustomerName", customerName);
            return await Task.Factory.StartNew<List<CustomerResponseModel>>(() =>
            {
                List<CustomerResponseModel> customerResponseModelList = _iCustomerModelRepository.EntityFromSql("SSP_GetAllCustomersWithOutPaging",
                    pCompanyName,
                    pCustomerName).ToList();
                return customerResponseModelList;
            });
        }

        /// <summary>
        /// Get Customer By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<PmsCustomer> GetCustomerById(int id)
        {
            if (id == 0)
                return null;

            var query = _customer.Table.Where(x => x.Id == id && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerRoles"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertCustomer(PmsCustomer customerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException(nameof(customerRoles));

            await _customer.InsertAsync(customerRoles);

            //event notification
            _eventPublisher.EntityInserted(customerRoles);
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerRoles"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateCustomer(PmsCustomer customerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException(nameof(customerRoles));

            await _customer.UpdateAsync(customerRoles);

            //event notification
            _eventPublisher.EntityUpdated(customerRoles);
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PmsCustomer> DeleteCustomer(int id)
        {
            if (id == 0)
                return null;

            var Projects = _customer.GetByIdAsync(id);

            return await Projects;
        }

        /// <summary>
        /// Get All Customer
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<PmsCustomer> GetCustomerCompanyName(string companyname)
        {
            // Query your database table
            if (!string.IsNullOrWhiteSpace(companyname))
            {
                var query = _customer.Table.Where(c => c.Company.ToLower().Contains(companyname) && c.IsDeleted == false);
                return query.FirstOrDefault();
            }
            // Execute the query and return the filtered results
            return null;
        }
    }
}
