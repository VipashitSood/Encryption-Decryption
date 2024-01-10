using iTextSharp.text;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core.Domain.Customers;
using Tm.Core.Domain.Pms.Customer;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.Projection;
using Tm.Core.Domain.Pms.ProjectResponse;
using Tm.Core.Domain.Pms.UserRole;
using Tm.Data;
using Tm.Services.Events;

namespace Tm.Services.Pms.MasterData
{
    public partial class MasterDataService : IMasterDataService
    {
        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<ProjectStatus> _projectStatusRepository;
        private readonly IRepository<ProjectType> _projectTypeRepository;
        private readonly IRepository<TechStack> _techStackRepository;
        private readonly IRepository<ProjectDomain> _projectDomainRepository;
        private readonly IRepository<TechDepartment> _techDepartmentRepository;
        private readonly IRepository<ProjectName> _projectNameRepository;
        private readonly IRepository<Tm.Core.Domain.Customers.Customer> _customerRepository;
        private readonly IRepository<Projects> _projectsRepository;
        private readonly IRepository<CommunicationMode> _communicationModeRepository;
        private readonly IRepository<ADUser> _adUserRepository;
        private readonly IRepository<Tm.Core.Domain.Pms.MasterData.ResourceType> _resourceTypeRepository;
        private readonly IRepository<RefreshToken> _refreshTokenrepository;
        private readonly IRepository<MasterDataResponseModel> _iMasterDataResponseModelRepository;
        private readonly IRepository<MasterCurrencyDataResponseModel> _iMasterCurrencyDataResponseModelRepository;
        private readonly IRepository<MasterProjectResponseModel> _iProjectModelRepository;
       #endregion

        #region Ctor

        public MasterDataService(IEventPublisher eventPublisher,
            IRepository<Currency> currencyRepository,
            IRepository<ProjectStatus> projectStatusRepository,
            IRepository<ProjectType> projectTypeRepository,
            IRepository<TechStack> techStackRepository,
            IRepository<ProjectDomain> projectDomainRepository,
            IRepository<TechDepartment> techDepartmentRepository,
            IRepository<ProjectName> projectNameRepository,
            IRepository<Tm.Core.Domain.Customers.Customer> customerRepository,
            IRepository<Projects> projectsRepository,
            IRepository<CommunicationMode> communicationModeRepository, 
            IRepository<ADUser> adUserRepository,
            IRepository<Tm.Core.Domain.Pms.MasterData.ResourceType> resourceTypeRepository,
            IRepository<RefreshToken> refreshTokenrepository,
            IRepository<MasterDataResponseModel> iMasterDataResponseModelRepository,
            IRepository<MasterProjectResponseModel> iProjectModelRepository,
            IRepository<MasterCurrencyDataResponseModel> iMasterCurrencyDataResponseModelRepository)
        {
            _eventPublisher = eventPublisher;
            _currencyRepository = currencyRepository;
            _projectStatusRepository = projectStatusRepository;
            _projectTypeRepository = projectTypeRepository;
            _techStackRepository = techStackRepository;
            _projectDomainRepository = projectDomainRepository;
            _techDepartmentRepository = techDepartmentRepository;
            _projectNameRepository = projectNameRepository;
            _customerRepository = customerRepository;
            _projectsRepository = projectsRepository;
            _communicationModeRepository = communicationModeRepository;
            _adUserRepository = adUserRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _refreshTokenrepository = refreshTokenrepository;
            _iMasterDataResponseModelRepository = iMasterDataResponseModelRepository;
            _iMasterCurrencyDataResponseModelRepository = iMasterCurrencyDataResponseModelRepository;
            _iProjectModelRepository = iProjectModelRepository;
        }

        #endregion

        #region Method


        #region Customer

        /// <summary>
        /// Gets a User
        /// </summary>
        /// <param name="id">Customer identifier</param>
        /// <returns>A Customer</returns>
        public virtual async Task<Tm.Core.Domain.Customers.Customer> GetCustomerById(int id)
        {
            if (id == 0)
                return null;

            var customer = await _customerRepository.GetByIdAsync(id);

            return customer;
        }
        #endregion Customer

        #region Project Name

        /// <summary>
        /// Get All Project Name
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<ProjectName>> GetAllProjectName()
        {
            var query = _projectNameRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Project Name By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProjectName> GetProjectNameById(int id)
        {
            if (id <= 0)
                return null;

            var projectName = await _projectNameRepository.GetByIdAsync(id);

            if (projectName != null && !projectName.IsDeleted)
            {
                return projectName;
            }
            return null;
        }

        /// <summary>
        /// Insert Project Name
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertProjectName(ProjectName projectName)
        {
            if (projectName == null)
                throw new ArgumentNullException(nameof(projectName));

            await _projectNameRepository.InsertAsync(projectName);

            //event notification
            _eventPublisher.EntityInserted(projectName);
        }

        /// <summary>
        /// Update Project Name
        /// </summary>
        /// <param name="projectName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateProjectName(ProjectName projectName)
        {
            if (projectName == null)
                throw new ArgumentNullException(nameof(projectName));

            await _projectNameRepository.UpdateAsync(projectName);

            //event notification
            _eventPublisher.EntityUpdated(projectName);
        }

        /// <summary>
        /// Project Name Exists
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public bool ProjectNameExists(string projectName, int id )
        {
            if (string.IsNullOrEmpty(projectName))
                return false;

            var query = _projectNameRepository.Table.Where(p => p.Id!=id && p.Name == projectName);
            return query.Any();
        }

        /// <summary>
        /// Get ProjectId ByName Async
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public virtual async Task<int> GetProjectIdByNameAsync(string projectName)
        {
            var query = _projectNameRepository.Table
                                              .Where(pn => pn.Name == projectName)
                                              .Select(p => p.Id);

            return await query.FirstOrDefaultAsync();
        }


        #endregion Project Name

        #region Project Status

        /// <summary>
        /// Gets all projectStatus
        /// </summary>
        /// <returns>projectStatus</returns>
        public virtual async Task<List<MasterDataResponseModel>> GetAllProjectStatus(int pageNumber, int pageSize)
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                return _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllProjectStatus",
                    pPageNumber,
                    pPageSize).ToList();
            });
        }
        /// <summary>
        /// Get All project Status without paging
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<MasterDataResponseModel>> GetAllProjectStatusWithoutPaging()
        {
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                return _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllProjectStatusWithoutPaging").ToList();
            });
        }
        /// <summary>
        /// Gets a projectStatus 
        /// </summary>
        /// <param name="id">projectStatus identifier</param>
        /// <returns>Store</returns>
        public virtual async Task<ProjectStatus> GetProjectStatusById(int id)
        {
            if (id == 0)
                return null;

            var projectStatus = await _projectStatusRepository.GetByIdAsync(id);
            if (projectStatus != null && projectStatus.IsDeleted != true)
            {
                return projectStatus;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a projectStatus
        /// </summary>
        /// <param name="projectStatus">projectStatus</param>
        public virtual async Task InsertProjectStatus(ProjectStatus projectStatus)
        {
            if (projectStatus == null)
                throw new ArgumentNullException(nameof(projectStatus));

            await _projectStatusRepository.InsertAsync(projectStatus);

            //event notification
            _eventPublisher.EntityInserted(projectStatus);
        }

        /// <summary>
        /// Updates the projectStatus
        /// </summary>
        /// <param name="projectStatus">projectStatus</param>
        public virtual async Task UpdateProjectStatus(ProjectStatus projectStatus)
        {
            if (projectStatus == null)
                throw new ArgumentNullException(nameof(projectStatus));

            await _projectStatusRepository.UpdateAsync(projectStatus);

            //event notification
            _eventPublisher.EntityUpdated(projectStatus);
        }
        #endregion

        #region Project Type

        /// <summary>
        /// Gets all projectType
        /// </summary>
        /// <returns>projectType</returns>
        public virtual async Task<List<MasterDataResponseModel>> GetAllProjectType(int pageNumber, int pageSize)
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                return _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllProjectTypes",
                    pPageNumber,
                    pPageSize).ToList();
            });

            //return result;
        }
        /// <summary>
        /// Gets all projectType
        /// </summary>
        /// <returns>projectType</returns>
        public virtual async Task<List<MasterDataResponseModel>> GetAllProjectTypeWithoutPaging()
        {
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                return _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllProjectTypesWithoutPaging").ToList();
            });

            //return result;
        }

        /// <summary>
        /// Gets a projectType 
        /// </summary>
        /// <param name="id">projectType identifier</param>
        /// <returns>projectType</returns>
        public virtual async Task<ProjectType> GetProjectTypeById(int id)
        {
            if (id == 0)
                return null;

            var projectType = await _projectTypeRepository.GetByIdAsync(id);

            if (projectType != null && projectType.IsDeleted != true)
            {
                return projectType;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a projectType
        /// </summary>
        /// <param name="projectType">projectType</param>
        public virtual async Task InsertProjectType(ProjectType projectType)
        {
            if (projectType == null)
                throw new ArgumentNullException(nameof(projectType));

            await _projectTypeRepository.InsertAsync(projectType);

            //event notification
            _eventPublisher.EntityInserted(projectType);
        }

        /// <summary>
        /// Updates the projectType
        /// </summary>
        /// <param name="projectType">projectType</param>
        public virtual async Task UpdateProjectType(ProjectType projectType)
        {
            if (projectType == null)
                throw new ArgumentNullException(nameof(projectType));

            await _projectTypeRepository.UpdateAsync(projectType);

            //event notification
            _eventPublisher.EntityUpdated(projectType);
        }

        #endregion

        #region Currency

        /// <summary>
        /// Gets all Currency
        /// </summary>
        /// <returns>Currency</returns>
        public virtual async Task<List<MasterCurrencyDataResponseModel>> GetAllCurrency(int pageNumber, int pageSize)
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            var data= await Task.Factory.StartNew<List<MasterCurrencyDataResponseModel>>(() =>
            {
                return _iMasterCurrencyDataResponseModelRepository.EntityFromSql("SSP_GetAllCurrency",
                    pPageNumber,
                    pPageSize).ToList();
            });
            return data;
        }

        /// <summary>
        /// Gets a Currency 
        /// </summary>
        /// <param name="id">Currency identifier</param>
        /// <returns>Store</returns>
        public virtual Currency GetCurrencyById(int id)
        {
            if (id == 0)
                return null;

            Currency currency = _currencyRepository.GetByIdAsync(id).Result;

            if (currency != null && currency.IsDeleted != true)
            {
                return currency;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a Currency
        /// </summary>
        /// <param name="currency">Currency</param>
        public virtual async Task InsertCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            await _currencyRepository.InsertAsync(currency);

            //event notification
            _eventPublisher.EntityInserted(currency);
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currency">currency</param>
        public virtual async Task UpdateCurrency(Currency currency)
        {
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            await _currencyRepository.UpdateAsync(currency);

            //event notification
            _eventPublisher.EntityUpdated(currency);
        }
        /// <summary>
        /// Get Currency by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<List<Currency>> GetCurrencyByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return await _currencyRepository.Table.Where(x=>x.Name.Contains(name) && !x.IsDeleted).ToListAsync();
        }
        #endregion

        #region Project Domain

        /// <summary>
        /// Gets all ProjectDomain
        /// </summary>
        /// <returns>ProjectDomain</returns>
        public async Task<List<MasterProjectResponseModel>> GetAllProjectDomain(int pageNumber, int pageSize, string projectName = "", string HODName = "")
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            var pProjectName = SqlParameterHelper.GetStringParameter("ProjectName", projectName);
            var pHODName = SqlParameterHelper.GetStringParameter("HODName", HODName);
            return await Task.Factory.StartNew<List<MasterProjectResponseModel>>(() =>
            {
                List<MasterProjectResponseModel> projectResponseModelList = _iProjectModelRepository.EntityFromSql("SSP_GetAllProjectDomain",
                    pPageNumber,
                    pPageSize,
                    pProjectName,
                    pHODName).ToList();
                return projectResponseModelList;
            });
        }

        /// <summary>
        /// Gets a ProjectDomain 
        /// </summary>
        /// <param name="id">ProjectDomain identifier</param>
        /// <returns>ProjectDomain</returns>
        public virtual async Task<ProjectDomain> GetProjectDomainById(int id)
        {
            if (id == 0)
                return null;

            var projectDomain = await _projectDomainRepository.GetByIdAsync(id);
            if (projectDomain != null && projectDomain.IsDeleted != true)
            {
                return projectDomain;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets BillingInfo asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProjectDomain>> GetAllProjectDomainAsync(object id)
        {
            return _projectDomainRepository.Table.Join(_adUserRepository.Table, PDomain => PDomain.HOD, AdUser => AdUser.UserId, (PDomain, AdUserHOD) => new { PDomain = PDomain, Aduser = AdUserHOD })
                .Where(x => x.PDomain.IsDeleted == false).Select(post => new ProjectDomain()
                {
                    Name = post.PDomain.Name,
                    Id = post.PDomain.Id,
                    CreatedBy = post.PDomain.CreatedBy

                }).ToList();
        }
        /// <summary>
        /// Inserts a ProjectDomain
        /// </summary>
        /// <param name="ProjectDomain">ProjectDomain</param>
        public virtual async Task InsertProjectDomain(ProjectDomain projectDomain)
        {
            if (projectDomain == null)
                throw new ArgumentNullException(nameof(projectDomain));

            await _projectDomainRepository.InsertAsync(projectDomain);

            //event notification
            _eventPublisher.EntityInserted(projectDomain);
        }

        /// <summary>
        /// Updates the ProjectDomain
        /// </summary>
        /// <param name="ProjectDomain">ProjectDomain</param>
        public virtual async Task UpdateProjectDomain(ProjectDomain projectDomain)
        {
            if (projectDomain == null)
                throw new ArgumentNullException(nameof(projectDomain));

            await _projectDomainRepository.UpdateAsync(projectDomain);

            //event notification
            _eventPublisher.EntityUpdated(projectDomain);
        }
        #endregion

        #region Tech Department

        /// <summary>
        /// Get All Tech Department
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<TechDepartment>> GetAllTechDepartment()
        {
            var query = _techDepartmentRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get Tech Department By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TechDepartment> GetTechDepartmentById(int id)
        {
            if (id == 0)
                return null;

            var techDepartment = await _techDepartmentRepository.GetByIdAsync(id);
            if (techDepartment != null && techDepartment.IsDeleted != true)
            {
                return techDepartment;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Insert Tech Department
        /// </summary>
        /// <param name="techDepartment"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task InsertTechDepartment(TechDepartment techDepartment)
        {
            if (techDepartment == null)
                throw new ArgumentNullException(nameof(techDepartment));

            await _techDepartmentRepository.InsertAsync(techDepartment);

            //event notification
            _eventPublisher.EntityInserted(techDepartment);
        }

        /// <summary>
        /// Update Tech Department
        /// </summary>
        /// <param name="techDepartment"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual async Task UpdateTechDepartment(TechDepartment techDepartment)
        {
            if (techDepartment == null)
                throw new ArgumentNullException(nameof(techDepartment));

            await _techDepartmentRepository.UpdateAsync(techDepartment);

            //event notification
            _eventPublisher.EntityUpdated(techDepartment);
        }

        #endregion Tech Department

        #region Tech Stack

        /// <summary>
        /// Gets all TechStack
        /// </summary>
        /// <returns>TechStack</returns>
        public virtual async Task<IList<TechStack>> GetAllTechStack()
        {
            var query = _techStackRepository.Table.Where(x => !x.IsDeleted);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Get All TechStacks
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<List<MasterDataResponseModel>> GetAllTechStacks(int pageNumber, int pageSize)
        {
            var pPageNumber = SqlParameterHelper.GetInt32Parameter("PageNumber", pageNumber);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                return _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllTechStack",
                    pPageNumber,
                    pPageSize).ToList();
            });

            //return result;
        }

        public virtual async Task<IList<TechStack>> GetTechStackMappingByDeptId(int deptId)
        {
            var query = _techStackRepository.Table.Where(x => !x.IsDeleted && x.DepartmentId == deptId);

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets a TechStack 
        /// </summary>
        /// <param name="id">TechStack identifier</param>
        /// <returns>TechStack</returns>
        public virtual async Task<TechStack> GetTechStackById(int id)
        {
            if (id == 0)
                return null;

            var techStack = await _techStackRepository.GetByIdAsync(id);
            if (techStack != null && techStack.IsDeleted != true)
            {
                return techStack;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a TechStack
        /// </summary>
        /// <param name="TechStack">TechStack</param>
        public virtual async Task InsertTechStack(TechStack techStack)
        {
            if (techStack == null)
                throw new ArgumentNullException(nameof(techStack));

            await _techStackRepository.InsertAsync(techStack);

            //event notification
            _eventPublisher.EntityInserted(techStack);
        }

        /// <summary>
        /// Updates the TechStack
        /// </summary>
        /// <param name="TechStack">TechStack</param>
        public virtual async Task UpdateTechStack(TechStack techStack)
        {
            if (techStack == null)
                throw new ArgumentNullException(nameof(techStack));

            await _techStackRepository.UpdateAsync(techStack);

            //event notification
            _eventPublisher.EntityUpdated(techStack);
        }
        /// <summary>
        /// Update Tech Stacks
        /// </summary>
        /// <param name="techStacks"></param>
        public virtual async Task UpdateTechStacks(List<TechStack> techStacks)
        {
            if (techStacks == null)
                throw new ArgumentNullException(nameof(techStacks));

            foreach (var techStack in techStacks)
            {
                await _techStackRepository.UpdateAsync(techStack);
            }
        }

        #endregion Tech Stack

        #region  Communication Mode

        /// <summary>
        /// Get All Communication Mode
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MasterDataResponseModel>> GetAllCommunicationModeAsync()
        {
            return await Task.Factory.StartNew<List<MasterDataResponseModel>>(() =>
            {
                List<MasterDataResponseModel> masterDataResponseModelList = _iMasterDataResponseModelRepository.EntityFromSql("SSP_GetAllCommunicationModes").ToList();
                return masterDataResponseModelList;
            });
        }

        /// <summary>
        /// Get Communication Modes By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommunicationMode> GetCommunicationModesByIdAsync(int id)
        {
            if (id == 0)
                return null;

            var communicationMode = await _communicationModeRepository.GetByIdAsync(id);
            if (communicationMode != null && communicationMode.IsDeleted != true)
            {
                return communicationMode;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Insert Communication Mode
        /// </summary>
        /// <param name="communicationMode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task InsertCommunicationModeAsync(CommunicationMode communicationMode)
        {
            if (communicationMode == null)
                throw new ArgumentNullException(nameof(communicationMode));

            await _communicationModeRepository.InsertAsync(communicationMode);

            //event notification
            _eventPublisher.EntityInserted(communicationMode);
        }
        /// <summary>
        /// Update Communication Mode
        /// </summary>
        /// <param name="communicationMode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateCommunicationModeAsync(CommunicationMode communicationMode)
        {
            if (communicationMode == null)
                throw new ArgumentNullException(nameof(communicationMode));

            await _communicationModeRepository.UpdateAsync(communicationMode);

            //event notification
            _eventPublisher.EntityUpdated(communicationMode);
        }


        #endregion  Communication Mode

        #region Refresh Token

        /// <summary>
        /// Gets a RefreshToken 
        /// </summary>
        /// <param name="id">RefreshToken identifier</param>
        /// <returns>RefreshToken</returns>
        public virtual async Task<RefreshToken> GetRefreshTokenByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var refreshToken = await _refreshTokenrepository.Table.FirstOrDefaultAsync(x => x.UserId == userId);
            if (refreshToken != null)
            {
                return refreshToken;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a Refresh Token
        /// </summary>
        /// <param name="RefreshToken">RefreshToken</param>
        public virtual async Task InsertRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            await _refreshTokenrepository.InsertAsync(refreshToken);

            //event notification
            _eventPublisher.EntityInserted(refreshToken);
        }

        /// <summary>
        /// Updates the RefreshToken
        /// </summary>
        /// <param name="RefreshToken">RefreshToken</param>
        public virtual async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            await _refreshTokenrepository.UpdateAsync(refreshToken);

            //event notification
            _eventPublisher.EntityUpdated(refreshToken);
        }
        #endregion


        #endregion


        #region Resource Type
        /// <summary>
        /// Get All active list of resource type data
        /// </summary>
        /// <returns></returns>
        public async Task<List<Core.Domain.Pms.MasterData.ResourceType>> GetAllResourceTypeListAsync()
        {
            return await _resourceTypeRepository.Table.Where(x=>x.IsDeleted==false && x.IsActive== true).ToListAsync();
        }
        
        #endregion
    }
}
