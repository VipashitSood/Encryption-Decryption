using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.ProjectResponse;
using Tm.Core.Domain.Users;

namespace Tm.Services.Pms.MasterData
{
	public interface IMasterDataService
	{

		#region Customer
		Task<Tm.Core.Domain.Customers.Customer> GetCustomerById(int id);

		#endregion Customer

		#region Project Name
		Task<IList<ProjectName>> GetAllProjectName();
		Task<ProjectName> GetProjectNameById(int id);
		Task InsertProjectName(ProjectName projectName);
		Task UpdateProjectName(ProjectName projectName);
		bool ProjectNameExists(string projectName, int id);
		Task<int> GetProjectIdByNameAsync(string projectName);
        #endregion Project Name

        #region Project Status
        Task<List<MasterDataResponseModel>> GetAllProjectStatus(int pageNumber, int pageSize);
		Task<List<MasterDataResponseModel>> GetAllProjectStatusWithoutPaging();

        Task<ProjectStatus> GetProjectStatusById(int id);
		Task InsertProjectStatus(ProjectStatus projectStatus);
		Task UpdateProjectStatus(ProjectStatus projectStatus);
        #endregion

        #region Project Type
        Task<List<MasterDataResponseModel>> GetAllProjectType(int pageNumber, int pageSize);
		Task<List<MasterDataResponseModel>> GetAllProjectTypeWithoutPaging();

        Task<ProjectType> GetProjectTypeById(int id);
		Task InsertProjectType(ProjectType projectType);
		Task UpdateProjectType(ProjectType projectType);
        #endregion

        #region Currency
        Task<List<MasterCurrencyDataResponseModel>> GetAllCurrency(int pageNumber, int pageSize);
        Currency GetCurrencyById(int id);
		Task InsertCurrency(Currency currency);
		Task UpdateCurrency(Currency currency);
		Task<List<Currency>> GetCurrencyByName(string name);
        #endregion

		#region Project Domain
		Task<List<MasterProjectResponseModel>> GetAllProjectDomain(int pageNumber, int pageSize, string projectName = "", string HODName = "");
        Task<ProjectDomain> GetProjectDomainById(int id);
		Task InsertProjectDomain(ProjectDomain projectDomain);
		Task UpdateProjectDomain(ProjectDomain projectDomain);

		#endregion

		#region Tech Stack
		Task<IList<TechStack>> GetAllTechStack();
        Task<List<MasterDataResponseModel>> GetAllTechStacks(int pageNumber, int pageSize);
        Task<TechStack> GetTechStackById(int id);
		Task<IList<TechStack>> GetTechStackMappingByDeptId(int deptId);
		Task InsertTechStack(TechStack techStack);
		Task UpdateTechStack(TechStack techStack);
		Task UpdateTechStacks(List<TechStack> techStacks);
		#endregion

		#region Tech Department
		Task<IList<TechDepartment>> GetAllTechDepartment();
		Task<TechDepartment> GetTechDepartmentById(int id);
		Task InsertTechDepartment(TechDepartment techDepartment);
		Task UpdateTechDepartment(TechDepartment techDepartment);

		#endregion Tech Department

		#region Communication Mode
		Task<IList<MasterDataResponseModel>> GetAllCommunicationModeAsync();
		Task<CommunicationMode> GetCommunicationModesByIdAsync(int id);
		Task InsertCommunicationModeAsync(CommunicationMode communicationMode);
		Task UpdateCommunicationModeAsync(CommunicationMode communicationMode);
		#endregion Communication Mode

		#region Resource Type
		/// <summary>
		/// Get All active list of resource type data
		/// </summary>
		/// <returns></returns>
		Task<List<Core.Domain.Pms.MasterData.ResourceType>> GetAllResourceTypeListAsync();

		#endregion

		#region RefreshToken

		/// <summary>
		/// Gets a RefreshToken 
		/// </summary>
		/// <param name="id">RefreshToken identifier</param>
		/// <returns>RefreshToken</returns>
		Task<RefreshToken> GetRefreshTokenByUserId(string userId);

		/// <summary>
		/// Inserts a Refresh Token
		/// </summary>
		/// <param name="RefreshToken">RefreshToken</param>
		Task InsertRefreshToken(RefreshToken refreshToken);

		/// <summary>
		/// Updates the RefreshToken
		/// </summary>
		/// <param name="RefreshToken">RefreshToken</param>
		Task UpdateRefreshToken(RefreshToken refreshToken);
		#endregion
	}
}
