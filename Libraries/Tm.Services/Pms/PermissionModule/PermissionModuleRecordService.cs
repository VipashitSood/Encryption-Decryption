using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.ModulePermissionRole;
using Tm.Core.Domain.Pms.PermissionModuleRecordResponse;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.PermissionModule
{
	public partial class PermissionModuleRecordService : IPermissionModuleRecordService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PermissionModuleRecord> _permissionModuleRecord;
        private readonly IRepository<PermissionModuleRecordModel> _permissionModuleRecordResponse;

        #endregion

        #region Ctor

        public PermissionModuleRecordService(IEventPublisher eventPublisher,
            IRepository<PermissionModuleRecord> permissionModuleRecord,
            IRepository<PermissionModuleRecordModel> permissionModuleRecordResponse)
        {
            _eventPublisher = eventPublisher;
            _permissionModuleRecord = permissionModuleRecord;
            _permissionModuleRecordResponse= permissionModuleRecordResponse;
        }

        #endregion

        #region Method
        /// <summary>
        /// Get All Permission Module Record
        /// </summary>
        /// <returns></returns>



        //public virtual async Task<IList<PermissionModuleRecord>> GetAllPermissionModuleRecord()
        //{
        //    var query = _permissionModuleRecord.Table.Where(x => !x.IsDeleted);

        //    var result = await query.ToListAsync();

        //    return result;
        //}

        public virtual async Task<IList<PermissionModuleRecordModel>> GetAllPermissionModuleRecord(string name = "", int userModuleId = 0)
        {
            var pName = SqlParameterHelper.GetStringParameter("Name", name);
            var pUserModuleId = SqlParameterHelper.GetInt32Parameter("UserModuleId", userModuleId);

            return await Task.Factory.StartNew<List<PermissionModuleRecordModel>>(() =>
            {
                List<PermissionModuleRecordModel> permissionResponseModelList = _permissionModuleRecordResponse.EntityFromSql("SSP_GetPermissionModuleRecordWithoutPaging",
                    pName,
                    pUserModuleId).ToList();
                return permissionResponseModelList;
            });
        }

        /// <summary>
        /// Get Permission Module Record By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PermissionModuleRecord> GetPermissionModuleRecordById(int id)
        {
            try
            {
                if (id <= 0)
                    return null;

                var permissionModuleRecord = await _permissionModuleRecord.GetByIdAsync(id);

                if (permissionModuleRecord != null && !permissionModuleRecord.IsDeleted)
                {
                    return permissionModuleRecord;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // Return null or throw a more specific exception if appropriate
                return null;
            }
        }

        /// <summary>
        /// Insert Permission Module Record
        /// </summary>
        /// <param name="roleModulePermission"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertPermissionModuleRecord(PermissionModuleRecord permissionModuleRecord)
        {
            if (permissionModuleRecord == null)
                throw new ArgumentNullException(nameof(permissionModuleRecord));

            await _permissionModuleRecord.InsertAsync(permissionModuleRecord);

            //event notification
            _eventPublisher.EntityInserted(permissionModuleRecord);
        }

        /// <summary>
        /// Update Permission Module Record
        /// </summary>
        /// <param name="permissionModuleRecord"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdatePermissionModuleRecord(PermissionModuleRecord permissionModuleRecord)
        {
            if (permissionModuleRecord == null)
                throw new ArgumentNullException(nameof(permissionModuleRecord));

            await _permissionModuleRecord.UpdateAsync(permissionModuleRecord);

            //event notification
            _eventPublisher.EntityUpdated(permissionModuleRecord);
        }
        #endregion
    }
}
