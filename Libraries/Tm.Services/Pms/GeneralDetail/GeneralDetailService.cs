using LinqToDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Data;

namespace Tm.Services.Pms.GeneralDetail
{
    public partial class GeneralDetailService : IGeneralDetailService
    {
        private readonly IRepository<ClientDetail> _clientDetailRepository;

        public GeneralDetailService(IRepository<ClientDetail> clientDetailRepository)
        {
            _clientDetailRepository = clientDetailRepository;
        }

        public virtual async Task<ClientDetail> GetClientDetailByOrderId(int id)
        {
            var query = _clientDetailRepository.Table.FirstOrDefaultAsync(x => x.OrderId == id);

            return await query;
        }
    }
}
