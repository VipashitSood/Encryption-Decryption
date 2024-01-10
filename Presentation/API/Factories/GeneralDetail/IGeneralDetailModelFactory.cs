using API.Models.GeneralDetail;
using System.Threading.Tasks;

namespace API.Factories.GeneralDetail
{
    public interface IGeneralDetailModelFactory
    {
        /// <summary>
        /// Get Order Detail By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderDetailResponseModel> GetOrderDetailById(int id);

        /// <summary>
        /// Get Project ManagerDetail By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ProjectManagerDetailResponseModel> GetProjectManagerDetailById(string userId);

        /// <summary>
        /// Get Dropdown list of Orders, Managers, Project Status
        /// </summary>
        /// <returns></returns>
        Task<DropDownModel> GetAllDropDownLists();
        /// <summary>
        /// Get manager detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectManagerDetailResponseModel> GetProjectManagerDetailById(int id);
    }
}
