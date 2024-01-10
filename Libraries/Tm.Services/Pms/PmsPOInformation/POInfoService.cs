using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Data;
using Tm.Services.Events;
using Tm.Services.Pms.PmsCustomers;

namespace Tm.Services.Pms.PmsPOInformation
{
	public partial class POInfoService : IPOInfoService
	{
		#region Fields
		private readonly IEventPublisher _eventPublisher;
		private readonly IRepository<POInfo> _poInfoRepository;
		private readonly ICustomersService _customersService;
		private readonly IRepository<POInfoOrderMapping> _poInfoOrderMappingRepository;
		#endregion

		#region Ctor
		public POInfoService(IEventPublisher eventPublisher,
		   IRepository<POInfo> poInfoRepository,
			ICustomersService customersService,
			IRepository<POInfoOrderMapping> poInfoOrderMappingRepository)
		{
			_eventPublisher = eventPublisher;
			_poInfoRepository = poInfoRepository;
			_customersService = customersService;
			_poInfoOrderMappingRepository = poInfoOrderMappingRepository;

		}
		#endregion

		#region PO Information

		/// <summary>
		/// Gets all POInformation 
		/// </summary>
		/// <returns>POInformation</returns>
		public virtual async Task<IList<POInfo>> GetAllPOInfo(int poId = 0, int clientId = 0, string companyName = "")
		{
			var query = _poInfoRepository.Table;

			if (poId > 0)
				query = query.Where(x => x.Id == poId);


			if (clientId > 0)
				query = query.Where(x => x.CustomerId == clientId);

			if (!string.IsNullOrEmpty(companyName))
			{
				// Assuming you have a method to retrieve CustomerId by CompanyName in customerRepository
				var customer = await _customersService.GetCustomerCompanyName(companyName);

				if (customer != null)
				{
					// Filter by CustomerId obtained from the customerRepository
					query = query.Where(x => x.CustomerId == customer.Id);
				}
			}

			query = query.Where(x => x.IsDeleted == false);

			var result = await query.ToListAsync();

			return result;
		}

		/// <summary>
		/// Gets POInformation mapping by Identifier
		/// </summary>
		/// <returns>POInformation</returns>
		public virtual async Task<POInfo> GetPOInfoById(int id)
		{
			if (id <= 0)
				return null;

			var query = _poInfoRepository.Table.Where(x => x.CustomerId == id);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}

		/// <summary>
		/// Insert a POInformation
		/// </summary>
		public virtual async Task InsertPOInfo(POInfo poInfo)
		{
			if (poInfo == null)
				throw new ArgumentNullException(nameof(poInfo));

			await _poInfoRepository.InsertAsync(poInfo);

			//event notification
			_eventPublisher.EntityInserted(poInfo);
		}

		/// <summary>
		/// Update a POInformation
		/// </summary>
		public virtual async Task UpdatePOInfo(POInfo poInfo)
		{
			if (poInfo == null)
				throw new ArgumentNullException(nameof(poInfo));

			await _poInfoRepository.UpdateAsync(poInfo);

			//event notification
			_eventPublisher.EntityUpdated(poInfo);
		}

		/// <summary>
		/// Delete a POInformation
		/// </summary>
		public virtual async Task DeletePOInfo(POInfo poInfo)
		{
			if (poInfo == null)
				throw new ArgumentNullException(nameof(poInfo));

			await _poInfoRepository.DeleteAsync(poInfo);
			//event notification
			_eventPublisher.EntityDeleted(poInfo);
		}

		#endregion


		#region POinfoOrderMapping

		/// <summary>
		/// Gets POInfoOrderMapping mapping by Identifier
		/// </summary>
		/// <returns>POInformation</returns>
		public virtual async Task<IList<POInfoOrderMapping>> GetPOInfoOrderMappingByPOInfoId(int poInfoId)
		{
			if (poInfoId <= 0)
				return null;

			var query = _poInfoOrderMappingRepository.Table.Where(x => x.POInfoId == poInfoId && x.IsDeleted == false);

			var result = await query.ToListAsync();

			return result;
		}

		/// <summary>
		/// Insert a POInfoOrderMapping
		/// </summary>
		public virtual async Task InsertPOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping)
		{
			if (poInfoOrderMapping == null)
				throw new ArgumentNullException(nameof(poInfoOrderMapping));

			await _poInfoOrderMappingRepository.InsertAsync(poInfoOrderMapping);

			//event notification
			_eventPublisher.EntityInserted(poInfoOrderMapping);
		}

		/// <summary>
		/// Update a POInfoOrderMapping
		/// </summary>
		public virtual async Task UpdatePOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping)
		{
			if (poInfoOrderMapping == null)
				throw new ArgumentNullException(nameof(poInfoOrderMapping));

			await _poInfoOrderMappingRepository.UpdateAsync(poInfoOrderMapping);

			//event notification
			_eventPublisher.EntityUpdated(poInfoOrderMapping);
		}

		/// <summary>
		/// Delete a POInfoOrderMapping
		/// </summary>
		public virtual async Task DeletePOInfoOrderMapping(POInfoOrderMapping poInfoOrderMapping)
		{
			if (poInfoOrderMapping == null)
				throw new ArgumentNullException(nameof(poInfoOrderMapping));

			await _poInfoOrderMappingRepository.DeleteAsync(poInfoOrderMapping);
			//event notification
			_eventPublisher.EntityDeleted(poInfoOrderMapping);
		}

		/// <summary>
		/// Gets POInfoId and OrderId by Identifier
		/// </summary>
		/// <returns>POInformation</returns>
		public virtual async Task<POInfoOrderMapping> GetByPOInfoIdandOrderId(int poInfoId, int orderId)
		{
			if (poInfoId <= 0)
				return null;

			var query = _poInfoOrderMappingRepository.Table.Where(x => x.POInfoId == poInfoId && x.OrderId==orderId && x.IsDeleted == false);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}

		/// <summary>
		/// Gets POInfo OrderId by Identifier
		/// </summary>
		/// <returns>POInformation</returns>
		public virtual async Task<IList<POInfoOrderMapping>> GetPOInfoByOrderId(int customerId)
		{
			if (customerId <= 0)
				return null;

			var query = _poInfoOrderMappingRepository.Table.Where(x => x.CustomerId == customerId && x.IsDeleted == false);

			var result = await query.ToListAsync();

			return result;
		}


		#endregion
	}
}
