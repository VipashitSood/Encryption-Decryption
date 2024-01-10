using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ProjectDetail;

namespace Tm.Services.Pms.GeneralDetail
{
    public interface IGeneralDetailService
    {
        #region Order Name
        Task<ClientDetail> GetClientDetailByOrderId(int id);
        #endregion Order Name
    }
}
