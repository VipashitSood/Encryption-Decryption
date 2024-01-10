using API.Models.ProjectDetail;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.ProjectDetail;

namespace API.Factories.ChangeRequestDetail
{
    public interface IChangeRequestFactory
    {
        Task<List<ChangeRequestResponseModel>> GetAllChangeRequestByProjectAsync(int projectId);
        Task<ChangeRequestResponseModel> GetAllChangeRequestByIdAsync(int id);
        Task<(bool, string, int)> InsertUpdateChangeRequest(ChangeRequestModel model);
    }
}
