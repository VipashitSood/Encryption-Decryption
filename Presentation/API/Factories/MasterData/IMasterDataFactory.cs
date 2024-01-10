using API.Models.MasterData;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ProjectResponse;

namespace API.Factories.MasterData
{
    public interface IMasterDataFactory
    {
        #region Project Name
        Task<(bool, string)> InsertUpdateProjectName(MasterDataInputModel model);
        Task<MasterDataResponseModel> GetProjectNameById(int id);
        Task<IList<MasterDataResponseModel>> GetAllProjectName();
        Task<(bool, string)> DeleteProjectName(int id);
        #endregion Project Name

        #region Project Status
        Task<(bool, string)> InsertProjectStatus(MasterDataInputModel model);
        Task<List<MasterDataResponseModel>> GetAllProjectStatus(int pageNumber, int pageSize);
        Task<MasterDataResponseModel> GetProjectStatusById(int id);
        Task<(bool, string)> DeleteProjectStatus(int id);

        #endregion Project Status

        #region Project Type
        Task<(bool, string)> InsertUpdateProjectType(MasterDataInputModel model);
        Task<MasterDataResponseModel> GetProjectTypeById(int id);
        Task<List<MasterDataResponseModel>> GetAllProjectType(int pageNumber, int pageSize);
        Task<(bool, string)> DeleteProjectType(int id);

        #endregion Project Type

        #region Currency
        Task<(bool, string)> InsertUpdateCurrency(MasterDataCurrencyModel model);
        Task<MasterDataResponseCurrencyModel> GetCurrencyById(int id);
        Task<List<MasterCurrencyDataResponseModel>> GetAllCurrency(int pageNumber, int pageSize);
        Task<(bool, string)> DeleteCurrency(int id);

        #endregion Currency

        #region Project Domain
        Task<(bool, string)> InsertUpdateProjectDomain(MasterDataInputModel model);
        Task<List<MasterProjectResponseModel>> GetAllProjectDomain(int pageNumber, int pageSize, string projectName = "", string HODName = "");
        Task<MasterDomainDataModel> GetProjectDomainById(int id);
        Task<(bool, string)> DeleteProjectDomain(int id);

        #endregion Project Domain

        #region Tech Department
        Task<(bool, string)> InsertUpdateTechDepartment(MasterDataInputModel model);
        Task<MasterDataResponseModel> GetTechDepartmentById(int id);
        Task<IList<MasterDataResponseModel>> GetAllTechDepartment();
        Task<(bool, string)> DeleteTechDepartment(int id);

        #endregion Tech Department

        #region Tech Stack

        Task<(bool, string)> InsertUpdateTechStack(MasterDataInputStackModel model);
        Task<MasterDataStackModel> GetTechStackById(int id);

        //MasterDataResponseModel GetTechStackById(int id);
        //IList<MasterDataResponseModel> GetAllTechStack();
        Task<List<MasterDataResponseModel>> GetAllTechStack(int pageNumber, int pageSize);

        // IList<SelectListItem> GetTechStackMappingByDeptId(int deptId);
        Task<MasterTeckStackResponseModel> GetTechStackMappingByDeptId();
        Task<(bool, string)> DeleteTechStack(int id);

        #endregion Tech Stack

        #region  Communication Mode
        Task<IList<MasterDataResponseModel>> GetAllCommunicationModeAsync();
        Task<MasterDataResponseModel> GetCommunicationModeByIdAsync(int id);
        Task<(bool, string)> InsertUpdateAsync(MasterDataInputModel model);
        Task<(bool, string)> DeleteCommunicationModeAsync(int id);
        #endregion  Communication Mode
        #region Resource Type
        Task<List<Tm.Core.Domain.Pms.MasterData.ResourceType>> GetAllResourceTypeListAsync();
        #endregion
    }
}
