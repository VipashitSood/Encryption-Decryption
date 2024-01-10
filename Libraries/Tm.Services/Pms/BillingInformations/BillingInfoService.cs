using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.BillingInformations;
using Tm.Core.Domain.Pms.POInformation;
using Tm.Data;
using Tm.Services.Events;
using Tm.Services.Pms.PmsCustomers;

namespace Tm.Services.Pms.BillingInformations
{
	public partial class BillingInfoService : IBillingInfoService
	{
		#region Fields
		private readonly IEventPublisher _eventPublisher;
		private readonly IRepository<BillingInformation> _billingInformationRepository;
		private readonly IRepository<BillingInfoPOMapping> _billingInfoPOMappingRepository;
		private readonly IRepository<POInfo> _poInfoRepository;
		#endregion

		#region Ctor
		public BillingInfoService(IEventPublisher eventPublisher,
		IRepository<BillingInformation> billingInformationRepository,
			IRepository<BillingInfoPOMapping> billingInfoPOMappingRepository,
			IRepository<POInfo> poInfoRepository)
		{
			_eventPublisher = eventPublisher;
			_billingInformationRepository=billingInformationRepository;
			_billingInfoPOMappingRepository= billingInfoPOMappingRepository;
			_poInfoRepository=poInfoRepository;

		}
		#endregion

		#region Billing Information

		/// <summary>
		/// Gets All Billing Information 
		/// </summary>
		/// <returns>BillingInformation</returns>
		public virtual async Task<IList<BillingInformation>> GetAllBillingInformation(int customerId)
		{
			var query = _billingInformationRepository.Table;

			query = query.Where(x => x.CustomerId== customerId && x.IsDeleted == false);

			var result = await query.ToListAsync();

			return result;
		}

		/// <summary>
		/// Gets Billing Information mapping by Identifier
		/// </summary>
		/// <returns>BillingInformation</returns>
		public virtual async Task<BillingInformation> GetBillingInformationById(int id)
		{
			if (id <= 0)
				return null;

			var query = _billingInformationRepository.Table.Where(x => x.Id == id);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}

		/// <summary>
		/// Insert a Billing Information
		/// </summary>
		public virtual async Task InsertBillingInformation(BillingInformation billingInformation)
		{
			if (billingInformation == null)
				throw new ArgumentNullException(nameof(billingInformation));

			await _billingInformationRepository.InsertAsync(billingInformation);

			//event notification
			_eventPublisher.EntityInserted(billingInformation);
		}

		/// <summary>
		/// Update a Billing Information
		/// </summary>
		public virtual async Task UpdateBillingInformation(BillingInformation billingInformation)
		{
			if (billingInformation == null)
				throw new ArgumentNullException(nameof(billingInformation));

			await _billingInformationRepository.UpdateAsync(billingInformation);

			//event notification
			_eventPublisher.EntityUpdated(billingInformation);
		}

		/// <summary>
		/// Delete a Billing Information
		/// </summary>
		public virtual async Task DeleteBillingInformation(BillingInformation billingInformation)
		{
			if (billingInformation == null)
				throw new ArgumentNullException(nameof(billingInformation));

			await _billingInformationRepository.DeleteAsync(billingInformation);
			//event notification
			_eventPublisher.EntityDeleted(billingInformation);
		}

		#endregion

		#region BillingInfoPOMapping

		/// <summary>
		/// Gets BillingInfo PO Mapping by Identifier
		/// </summary>
		/// <returns>BillingInfoPOMapping</returns>
		public virtual async Task<BillingInfoPOMapping> GetBillingInfoPOMappingById(int id)
		{
			if (id <= 0)
				return null;

			var query = _billingInfoPOMappingRepository.Table.Where(x => x.Id == id && x.IsDeleted == false);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}

        public async Task<BillingInfoPOMapping> GetBillingInfoPOMappingByIds(int poId)
        {
            if (poId <= 0)
                return null;

            var query = _billingInfoPOMappingRepository.Table.Where(x => x.POId == poId && x.IsDeleted == false);

            var result = await query.FirstOrDefaultAsync();

            return result;
        }


        /// <summary>
        /// Gets BillingInfo PO Mapping mapping by Identifier
        /// </summary>
        /// <returns>BillingInfoPOMapping</returns>
        public virtual async Task<IList<BillingInfoPOMapping>> GetBillingInfoPOMappingByBillingInfoId(int billingInfoId)
		{
			if (billingInfoId <= 0)
				return null;

			var query = _billingInfoPOMappingRepository.Table.Where(x => x.BillingInfoId == billingInfoId && x.IsDeleted == false);

			var result = await query.ToListAsync();

			return result;
		}

		/// <summary>
		/// Insert a BillingInfo PO Mapping
		/// </summary>
		public virtual async Task InsertBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping)
		{
			if (billingInfoPOMapping == null)
				throw new ArgumentNullException(nameof(billingInfoPOMapping));

			await _billingInfoPOMappingRepository.InsertAsync(billingInfoPOMapping);

			//event notification
			_eventPublisher.EntityInserted(billingInfoPOMapping);
		}

		/// <summary>
		/// Update a POInfoOrderMapping
		/// </summary>
		public virtual async Task UpdateBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping)
		{
			if (billingInfoPOMapping == null)
				throw new ArgumentNullException(nameof(billingInfoPOMapping));

			await _billingInfoPOMappingRepository.UpdateAsync(billingInfoPOMapping);

			//event notification
			_eventPublisher.EntityUpdated(billingInfoPOMapping);
		}

		/// <summary>
		/// Delete a POInfoOrderMapping
		/// </summary>
		public virtual async Task DeleteBillingInfoPOMapping(BillingInfoPOMapping billingInfoPOMapping)
		{
			if (billingInfoPOMapping == null)
				throw new ArgumentNullException(nameof(billingInfoPOMapping));

			await _billingInfoPOMappingRepository.DeleteAsync(billingInfoPOMapping);
			//event notification
			_eventPublisher.EntityDeleted(billingInfoPOMapping);
		}


		/// <summary>
		/// Gets POInfo by Identifier
		/// </summary>
		/// <returns>POInfo</returns>
		public virtual async Task<POInfo> GetPONumberByPOValue(string poNumber)
		{
			if (string.IsNullOrEmpty(poNumber))
				return null;

			var query = _poInfoRepository.Table.Where(x => x.PONumber == poNumber);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}

		/// <summary>
		/// Gets BillingInfo PO Mapping by Identifier
		/// </summary>
		/// <returns>BillingInfoPOMapping</returns>
		public virtual async Task<BillingInfoPOMapping> GetBillingInfoPOMappingBybillingInfoandPoId(int billingPoInfoId , int poid)
		{
			if (billingPoInfoId <= 0)
				return null;

			var query = _billingInfoPOMappingRepository.Table.Where(x => x.BillingInfoId == billingPoInfoId && x.POId == poid && x.IsDeleted == false);

			var result = await query.FirstOrDefaultAsync();

			return result;
		}
		#endregion
	}
}
