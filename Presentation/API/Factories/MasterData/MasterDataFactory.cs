using API.Models.MasterData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tm.Core;
using Tm.Core.Domain.Pms.MasterData;
using Tm.Core.Domain.Pms.ProjectDetail;
using Tm.Core.Domain.Pms.ProjectResponse;
using Tm.Data;
using Tm.Services.Customers;
using Tm.Services.Pms.MasterData;
using Tm.Services.Pms.UserRole;

namespace API.Factories.MasterData
{
	public class MasterDataFactory : IMasterDataFactory
	{
		#region Fields
		private IMasterDataService _masterDataService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerService _customerService;
		private readonly IUserRoleService _userRoleService;
		private readonly IRepository<MasterDataResponseModel> _iMasterDataResponseModelRepository;
		private readonly IRepository<MasterProjectResponseModel> _iprojectResponseModelRepository;

		#endregion

		#region Ctor
		public MasterDataFactory(IMasterDataService masterDataService,
			IWorkContext workContext,
			ICustomerService customerService, IUserRoleService userRoleService, IRepository<MasterDataResponseModel> iMasterDataResponseModelRepository, IRepository<MasterProjectResponseModel> iprojectResponseModelRepository)
		{
			_masterDataService = masterDataService;
			_workContext = workContext;
			_customerService = customerService;
			_userRoleService = userRoleService;
			_iMasterDataResponseModelRepository= iMasterDataResponseModelRepository;
			_iprojectResponseModelRepository = iprojectResponseModelRepository;
			_iMasterDataResponseModelRepository = iMasterDataResponseModelRepository;
		}

		#endregion

		#region Utilities
		private string GetCustomerUsername(int customerId)
		{
			try
			{
				var customer = _customerService.GetCustomerById(customerId);
				return customer?.Username ?? "Unknown User";
			}
			catch (Exception ex)
			{
				//get unknow user if user isn't register
				return "Unknown User";
			}
		}

		private string GetHODName(string adUserID)
		{
			try
			{
				if (string.IsNullOrEmpty(adUserID))
				{
					return "Unknown User";
				}
				var customer = _userRoleService.GetADUserByADUserId(adUserID).Result;
				return customer?.Name ?? "Unknown User";
			}
			catch (Exception ex)
			{
				//get unknow user if user isn't register
				return "Unknown User";
			}
		}
		#endregion Utilities

		#region Method

		#region Project Name

		/// <summary>
		/// Insert & Update Project Name
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateProjectName(MasterDataInputModel model)
		{
			int userId = 0;
			var user = await _userRoleService.GetADUserByADUserId(model.UserId);
			if (user != null)
			{
				userId = user.Id;
			}
			var projectNameList = await _masterDataService.GetAllProjectName();
			if (model.Id > 0)
			{
				if (projectNameList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Project already exists !");
				}
				var projectName = await _masterDataService.GetProjectNameById(model.Id);
				if (projectName != null)
				{
					projectName.Name = (model.Name != "string") ? model.Name : projectName.Name;
					projectName.ModifiedBy = userId;
					projectName.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateProjectName(projectName);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}

			}
			else
			{
				if (projectNameList.Any(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()))
				{
					return (false, "This Name of Project already exists !");
				}

				var projectName = new ProjectName();
				projectName.Name = model.Name;
				projectName.CreatedBy = userId;
				projectName.CreatedOn = DateTime.UtcNow;
				projectName.IsDeleted = false;
				await _masterDataService.InsertProjectName(projectName);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Project Name By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseModel> GetProjectNameById(int id)
		{
			var result = new MasterDataResponseModel();
			var projectName = await _masterDataService.GetProjectNameById(id);
			if (projectName != null)
			{
				result.Name = projectName.Name;
				result.Id = projectName.Id;
			}

			return result;
		}

		/// <summary>
		/// Get All Project Name
		/// </summary>
		/// <returns></returns>        
		public async Task<IList<MasterDataResponseModel>> GetAllProjectName()
		{
			var data = await _masterDataService.GetAllProjectName();

			List<MasterDataResponseModel> result = data.Select(x => new MasterDataResponseModel
			{
				Id = x.Id,
				Name = x.Name,
				CreationDate = x.CreatedOn.ToString("dd/MM/yyyy"),
				UpdatedDate = x.ModifiedOn?.ToString("dd/MM/yyyy") ?? "",
				CreatedBy = GetCustomerUsername(x.CreatedBy),
				UpdatedByName = GetCustomerUsername(x.ModifiedBy),
			}).ToList();

			return result.OrderByDescending(x => x.Id).ToList();
		}

		/// <summary>
		/// Delete Project Name
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteProjectName(int id)
		{
			if (id > 0)
			{
				var projectName = await _masterDataService.GetProjectNameById(id);
				if (projectName != null)
				{
					if (_workContext.CurrentCustomer == null)
					{
						//if user is super admin then only record can be removed
						projectName.ModifiedBy = 1;
					}
					//projectStatus.ModifiedBy = _workContext.CurrentCustomer.Id; // toodo 
					projectName.ModifiedOn = DateTime.UtcNow;
					projectName.IsDeleted = true;
					await _masterDataService.UpdateProjectName(projectName);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "Can't update record");
				}

			}
			else
			{
				return (false, "No record found for update");
			}
		}

		#endregion Project Name

		#region Project Status

		/// <summary>
		/// Insert Project Status
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertProjectStatus(MasterDataInputModel model)
		{
			int userId = 0;
			var user = await _userRoleService.GetADUserByADUserId(model.UserId);
			if (user != null)
			{
				userId = user.Id;
			}
			var projectStatusList = await _masterDataService.GetAllProjectStatusWithoutPaging();
			if (model.Id > 0)
			{
				if (projectStatusList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null
					&& projectStatusList.Where(x => x.Id == model.Id).FirstOrDefault() == null)
				{
					return (false, "This Name of status already exists !");
				}
				var projectStatus = await _masterDataService.GetProjectStatusById(model.Id);
				if (projectStatus != null)
				{
					projectStatus.Name = (model.Name != "string") ? model.Name : projectStatus.Name;
					projectStatus.ModifiedBy = model.UserId;
					projectStatus.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateProjectStatus(projectStatus);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}

			}
			else
			{
				if (projectStatusList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of status already exists !");
				}

				var projectStatus = new ProjectStatus();
				projectStatus.Name = model.Name;
				projectStatus.CreatedBy = model.UserId;
				projectStatus.CreatedOn = DateTime.UtcNow;
				projectStatus.IsDeleted = false;
				await _masterDataService.InsertProjectStatus(projectStatus);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Project Status By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseModel> GetProjectStatusById(int id)
		{
			var result = new MasterDataResponseModel();
			var projectStatus = await _masterDataService.GetProjectStatusById(id);
			if (projectStatus != null)
			{
				result.Name = projectStatus.Name;
				result.Id = projectStatus.Id;
			}

			return result;
		}

		/// <summary> 
		/// Ge tAll Project Status
		/// </summary>
		/// <returns></returns>
		public async Task<List<MasterDataResponseModel>> GetAllProjectStatus(int pageNumber, int pageSize)
		{
			return await _masterDataService.GetAllProjectStatus(pageNumber, pageSize);
		}

		/// <summary>
		/// Delete Project Status
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteProjectStatus(int id)
		{
			if (id > 0)
			{
				var projectStatus = await _masterDataService.GetProjectStatusById(id);
				if (projectStatus != null)
				{
					projectStatus.ModifiedOn = DateTime.UtcNow;
					projectStatus.IsDeleted = true;
					await _masterDataService.UpdateProjectStatus(projectStatus);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "Can't update record");
				}

			}
			else
			{
				return (false, "No record found for update");
			}
		}

		#endregion Project Status

		#region Project Type

		/// <summary>
		/// Insert & Update Project Type
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateProjectType(MasterDataInputModel model)
		{
			var projectTypeList = await _masterDataService.GetAllProjectTypeWithoutPaging();
			if (model.Id > 0)
			{
				if (projectTypeList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null
					&& projectTypeList.Where(x => x.Id == model.Id).FirstOrDefault() == null)
				{
					return (false, "This Name of Type already exists !");
				}
				var projectType = await _masterDataService.GetProjectTypeById(model.Id);
				if (projectType != null)
				{
					projectType.Name = (model.Name != "string") ? model.Name : projectType.Name;
					projectType.ModifiedBy = model.UserId;
					projectType.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateProjectType(projectType);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}

			}
			else
			{
				if (projectTypeList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Type already exists !");
				}

				var projectType = new ProjectType();
				projectType.Name = model.Name;
				projectType.CreatedBy = model.UserId;
				projectType.CreatedOn = DateTime.UtcNow;
				projectType.IsDeleted = false;
				await _masterDataService.InsertProjectType(projectType);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Project Type By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseModel> GetProjectTypeById(int id)
		{
			var result = new MasterDataResponseModel();
			var projectType = await _masterDataService.GetProjectTypeById(id);
			if (projectType != null)
			{
				result.Name = projectType.Name;
				result.Id = projectType.Id;
			}

			return result;
		}

		/// <summary>
		/// Get All Project Type
		/// </summary>
		/// <returns></returns>
		public async Task<List<MasterDataResponseModel>> GetAllProjectType(int pageNumber, int pageSize)
		{
			return await _masterDataService.GetAllProjectType(pageNumber, pageSize);
		}

		/// <summary>
		/// Delete Project Type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteProjectType(int id)
		{
			if (id > 0)
			{
				var projectType = await _masterDataService.GetProjectTypeById(id);
				if (projectType != null)
				{
					projectType.ModifiedOn = DateTime.UtcNow;
					projectType.IsDeleted = true;
					await _masterDataService.UpdateProjectType(projectType);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "Can't update record");
				}

			}
			else
			{
				return (false, "No record found for update");
			}
		}

		#endregion Project Type

		#region Currency

		/// <summary>
		/// Insert & Update Currency
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateCurrency(MasterDataCurrencyModel model)
		{
			var iCurrencyExist = await _masterDataService.GetCurrencyByName(model.Name.Trim().ToLower());
			if (model.Id > 0)
			{
				var isOldRecord=iCurrencyExist.Where(x => x.Id != model.Id).ToList();
				if (isOldRecord.Count()>0)
				{
					return (false, "This Name of Currency already exists !");
				}
				var currency = _masterDataService.GetCurrencyById(model.Id);
				if (currency != null)
				{
					currency.Name = (model.Name != "string") ? model.Name : currency.Name;
					if (model.AttachFiles == null)
					{
						currency.Icon = currency.Icon;
					}
					else
					{
						currency.Icon = model.CurrencyIcon;
					}
					currency.ModifiedBy = model.UserId;
					currency.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateCurrency(currency);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}
			}
			else
			{

				if (iCurrencyExist.Count()>0)
				{
					return (false, "This Name of Currency already exists !");
				}

				var currency = new Currency();
				currency.Name = model.Name;
				currency.Icon = model.CurrencyIcon;
				currency.CreatedBy = model.UserId;
				currency.CreatedOn = DateTime.UtcNow;
				currency.IsDeleted = false;
				await _masterDataService.InsertCurrency(currency);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Currency By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseCurrencyModel> GetCurrencyById(int id)
		{
			var result = new MasterDataResponseCurrencyModel();
			var projectDomain =_masterDataService.GetCurrencyById(id);
			if (projectDomain != null)
			{
				result.CurrencyIcon = projectDomain.Icon;
				result.Name = projectDomain.Name;
				result.Id = projectDomain.Id;
			}
			return result;
		}

		/// <summary>
		/// Get All Currency
		/// </summary>
		/// <returns></returns>
		public async Task<List<MasterCurrencyDataResponseModel>> GetAllCurrency(int pageNumber, int pageSize)
		{
			return await _masterDataService.GetAllCurrency(pageNumber, pageSize);
		}

		/// <summary>
		/// Delete Currency
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteCurrency(int id)
		{
			if (id > 0)
			{
				var currency = _masterDataService.GetCurrencyById(id);
				if (currency != null)
				{
					currency.ModifiedOn = DateTime.UtcNow;
					currency.IsDeleted = true;
					await _masterDataService.UpdateCurrency(currency);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "Can't update record");
				}

			}
			else
			{
				return (false, "No record found for update");
			}
		}

		#endregion Currency

		#region Project Domain

		/// <summary>
		/// Insert & Update Project Domain
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateProjectDomain(MasterDataInputModel model)
		{
			int userId = 0;
			var user = await _userRoleService.GetADUserByADUserId(model.UserId);
			if (user != null)
			{
				userId = user.Id;
			}
			var projectDomainList = await _masterDataService.GetAllProjectDomain(1,int.MaxValue);
			if (model.Id > 0)
			{
				if (projectDomainList.Where(x => x.Id != model.Id && x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Domain already exists !");
				}
				var projectDomain = await _masterDataService.GetProjectDomainById(model.Id);
				if (projectDomain != null)
				{
					var hodUser = await _userRoleService.GetADUserByADUserId(model.HOD);
					projectDomain.HODId = model.HOD;
					projectDomain.HOD = hodUser.DisplayName;
					projectDomain.Name = (model.Name != "string") ? model.Name : projectDomain.Name;
					projectDomain.ModifiedBy = model.UserId;
					projectDomain.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateProjectDomain(projectDomain);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}
			}
			else
			{
				if (projectDomainList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Domain already exists !");
				}

				var projectDomain = new ProjectDomain();
                var hodUser = await _userRoleService.GetADUserByADUserId(model.HOD);
				projectDomain.HODId = model.HOD;
				projectDomain.HOD = hodUser.DisplayName;
                projectDomain.Name = model.Name;
				projectDomain.CreatedBy = model.UserId;
				projectDomain.CreatedOn = DateTime.UtcNow;
				projectDomain.IsDeleted = false;
				await _masterDataService.InsertProjectDomain(projectDomain);
				return (true, "Successfully Done");
			}
		}



		/// <summary>
		/// Get Project Domain By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDomainDataModel> GetProjectDomainById(int id)
		{
			var result = new MasterDomainDataModel();
			var projectDomain = await _masterDataService.GetProjectDomainById(id);
			if (projectDomain != null)
			{
				result.Name = projectDomain.Name;
				result.Id = projectDomain.Id;
				result.HODName = GetHODName(projectDomain.HOD);
				result.HOD = projectDomain.HOD;
			}

			return result;
		}

		/// <summary>
		/// Get All Project Domain
		/// </summary>
		/// <returns></returns>

		public async Task<List<MasterProjectResponseModel>> GetAllProjectDomain(int pageNumber, int pageSize, string projectName = "", string HODName = "")
		{
			return await  _masterDataService.GetAllProjectDomain(pageNumber, pageSize, projectName, HODName);
		}

		/// <summary>
		/// Delete Project Domain
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteProjectDomain(int id)
		{
			if (id > 0)
			{
				var projectDomain = await _masterDataService.GetProjectDomainById(id);
				if (projectDomain != null)
				{
					projectDomain.ModifiedOn = DateTime.UtcNow;
					projectDomain.IsDeleted = true;
					await _masterDataService.UpdateProjectDomain(projectDomain);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "Can't update record");
				}

			}
			else
			{
				return (false, "No record found for update");
			}
		}

		#endregion Project Domain

		#region Tech Department

		/// <summary>
		/// Insert & Update Tech Department
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateTechDepartment(MasterDataInputModel model)
		{
			int userId = 0;
			var user = await _userRoleService.GetADUserByADUserId(model.UserId);
			if (user != null)
			{
				userId = user.Id;
			}
			var techDepartmentList = await _masterDataService.GetAllTechDepartment();
			if (model.Id > 0)
			{
				if (techDepartmentList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Tech Department already exists !");
				}
				var techDepartment = await _masterDataService.GetTechDepartmentById(model.Id);
				if (techDepartment != null)
				{
					techDepartment.Name = (model.Name != "string") ? model.Name : techDepartment.Name;
					techDepartment.ModifiedBy = userId;
					techDepartment.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateTechDepartment(techDepartment);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}

			}
			else
			{

				if (techDepartmentList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Tech Department already exists !");
				}

				var techDepartment = new TechDepartment();
				techDepartment.Name = model.Name;
				techDepartment.CreatedBy = userId;
				techDepartment.CreatedOn = DateTime.UtcNow;
				techDepartment.IsDeleted = false;
				await _masterDataService.InsertTechDepartment(techDepartment);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Tech Department By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseModel> GetTechDepartmentById(int id)
		{
			var result = new MasterDataResponseModel();
			var techDepartment = await _masterDataService.GetTechDepartmentById(id);
			if (techDepartment != null)
			{
				result.Name = techDepartment.Name;
				result.Id = techDepartment.Id;
			}

			return result;
		}

		/// <summary>
		/// Get All Tech Department
		/// </summary>
		/// <returns></returns>
		public async Task<IList<MasterDataResponseModel>> GetAllTechDepartment()
		{
			var data = await _masterDataService.GetAllTechDepartment();
			List<MasterDataResponseModel> result = data.Select(x => new MasterDataResponseModel
			{
				Id = x.Id,
				Name = x.Name,
				CreationDate = x.CreatedOn.ToString("dd/MM/yyyy"),
				UpdatedDate = x.ModifiedOn?.ToString("dd/MM/yyyy") ?? "",
				CreatedBy = GetCustomerUsername(x.CreatedBy),
				UpdatedByName = GetCustomerUsername(x.ModifiedBy),
			}).ToList();

			return result.OrderByDescending(x => x.Id).ToList();
		}

		/// <summary>
		/// Get TechStackMapping By DeptId
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<MasterTeckStackResponseModel> GetTechStackMappingByDeptId()
		{
			var teckStackList = await _masterDataService.GetAllTechStack();
			var filteredTechStackList = teckStackList.Where(x => x.DepartmentId == (int)ProjectEnum.BackendTechStack || x.DepartmentId == (int)ProjectEnum.FrontendTechStack).ToList();

			MasterTeckStackResponseModel model = new MasterTeckStackResponseModel();
			foreach (var techstack in filteredTechStackList)
			{
				if (techstack.DepartmentId == (int)ProjectEnum.BackendTechStack && techstack.IsDeleted == false)
					model.FETeckStackList.Add(new SelectListItem { Text = Convert.ToString(techstack.Name), Value = Convert.ToString(techstack.Id) });
				else
					model.BETeckStackList.Add(new SelectListItem { Text = Convert.ToString(techstack.Name), Value = Convert.ToString(techstack.Id) });
			}
			return model;
		}

		/// <summary>
		/// Delete Tech Department
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteTechDepartment(int id)
		{
			if (id <= 0)
			{
				return (false, "Invalid ID. The ID must be greater than zero.");
			}

			var techStacks = await _masterDataService.GetAllTechStack();
			var techStacksToDelete = techStacks.Where(ts => ts.DepartmentId == id).ToList();

			// Check if there are associated tech stacks with the department
			if (techStacksToDelete.Any())
			{
				// Mark associated tech stacks as deleted in a bulk update
				foreach (var techStack in techStacksToDelete)
				{
					techStack.ModifiedOn = DateTime.UtcNow;
					techStack.IsDeleted = true;
				}

				// Perform bulk update of tech stacks
				await _masterDataService.UpdateTechStacks(techStacksToDelete);
			}

			var techDepart = await _masterDataService.GetTechDepartmentById(id);

			if (techDepart == null)
			{
				return (false, "No record found for deletion.");
			}

			// Check if the current user has the privilege to delete the record
			if (_workContext.CurrentCustomer == null)
			{
				// If the user is super admin, allow record deletion
				techDepart.ModifiedBy = 1;
			}
			else
			{
				// If the user is not a super admin, you may want to handle this case differently
				return (false, "User does not have permission to delete the record.");
			}

			techDepart.ModifiedOn = DateTime.UtcNow;
			techDepart.IsDeleted = true;
			await _masterDataService.UpdateTechDepartment(techDepart);

			return (true, "Record deleted successfully.");
		}

		#endregion Tech Department

		#region Tech Stack

		/// <summary>
		/// Insert & Update Tech Stack
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateTechStack(MasterDataInputStackModel model)
		{
			var techStackList = await _masterDataService.GetAllTechStack();
			var user = _customerService.GetCustomerById(model.UserId);
			if (model.Id > 0)
			{
				if (techStackList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Tech Stack already exists !");
				}

				var techStack = await _masterDataService.GetTechStackById(model.Id);
				if (techStack != null)
				{
					techStack.Name = (model.Name != "string") ? model.Name : techStack.Name;
					techStack.ModifiedBy = _customerService.GetCustomerById(model.UserId).Id;
					//techStack.DepartmentId = model.DepartmentId;
					techStack.DepartmentId = (model.DepartmentId != 0) ? model.DepartmentId : techStack.DepartmentId;
					techStack.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateTechStack(techStack);
					return (true, "Successfully Done");
				}
				else
				{
					return (false, "No record found for update");
				}

			}
			else
			{
				if (techStackList.Where(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
				{
					return (false, "This Name of Tech Stack already exists !");
				}
				var techStack = new TechStack();
				techStack.Name = model.Name;
				techStack.DepartmentId = model.DepartmentId;
				techStack.CreatedBy = _customerService.GetCustomerById(model.UserId).Id;
				techStack.CreatedOn = DateTime.UtcNow;
				techStack.IsDeleted = false;
				await _masterDataService.InsertTechStack(techStack);
				return (true, "Successfully Done");
			}
		}

		/// <summary>
		/// Get Tech Stack By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataStackModel> GetTechStackById(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Invalid id. The id must be greater than zero.", nameof(id));
			}

			var result = new MasterDataStackModel();
			var techStack = await _masterDataService.GetTechStackById(id);

			if (techStack != null)
			{
				result.Id = techStack.Id;
				result.Name = techStack.Name;

				if (techStack.CreatedBy != null)
				{
					var createdByUser = _customerService.GetCustomerById(techStack.CreatedBy);
					result.CreatedBy = createdByUser?.Username ?? "Unknown User";
				}
				else
				{
					result.CreatedBy = "Unknown User";
				}

				if (techStack.DepartmentId != null)
				{
					var department = await _masterDataService.GetTechDepartmentById(techStack.DepartmentId);
					result.DepartName = department?.Name ?? "Unknown Department";
				}
				else
				{
					result.DepartName = "Unknown Department";
				}

				result.CreationDate = techStack.CreatedOn.ToString("dd/MM/yyyy");

				if (techStack.ModifiedBy != null)
				{
					var updatedByUser = _customerService.GetCustomerById(techStack.ModifiedBy);
					result.UpdatedBy = updatedByUser?.Username ?? "Unknown User";
				}
				else
				{
					result.UpdatedBy = "Unknown User";
				}

				result.UpdatedDate = techStack.ModifiedOn?.ToString("dd/MM/yyyy");
			}
			// If techStack is null
			else
			{
				throw new Exception($"Tech stack with ID {id} not found.");
			}

			return result;
		}

        /// <summary>
        /// Get All Tech Stack
        /// </summary>
        /// <returns></returns>              
        public async Task<List<MasterDataResponseModel>> GetAllTechStack(int pageNumber, int pageSize)
        {
            return await _masterDataService.GetAllTechStacks(pageNumber, pageSize);
        }

		/// <summary>
		/// Delete Tech Stack
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteTechStack(int id)
		{
			if (id <= 0)
			{
				return (false, "Invalid ID. The ID must be greater than zero.");
			}

			var techStack = await _masterDataService.GetTechStackById(id);

			if (techStack == null)
			{
				return (false, "No record found for deletion.");
			}

			// Check if the DepartmentId of the tech stack exists
			var allTechDepartments = await _masterDataService.GetAllTechDepartment();
			var departmentExists = allTechDepartments.Any(td => td.Id == techStack.DepartmentId);

			if (!departmentExists)
			{
				return (false, "Associated department not found. Cannot proceed with deletion.");
			}

			// Check if the current user has the privilege to delete the record
			if (_workContext.CurrentCustomer == null)
			{
				// If the user is super admin, allow record deletion
				techStack.ModifiedBy = 1;
			}
			else
			{
				// If the user is not a super admin, you may want to handle this case differently
				return (false, "User does not have permission to delete the record.");
			}

			techStack.ModifiedOn = DateTime.UtcNow;
			techStack.IsDeleted = true;
			await _masterDataService.UpdateTechStack(techStack);

			return (true, "Record deleted successfully.");
		}

		#endregion Tech Stack

		#region Communication Mode

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
		/// Get Communication Mode By Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MasterDataResponseModel> GetCommunicationModeByIdAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Invalid id. The id must be greater than zero.", nameof(id));
			}

			var result = new MasterDataResponseModel();
			var communicationMode = await _masterDataService.GetCommunicationModesByIdAsync(id);

			if (communicationMode != null)
			{
				result.Id = communicationMode.Id;
				result.Name = communicationMode.Name;

				if (communicationMode.CreatedBy != null)
				{
					var createdByUser = await _userRoleService.GetADUserByADUserId(communicationMode.CreatedBy);
					result.CreatedBy = createdByUser?.DisplayName ?? "NA";
				}
				else
				{
					result.CreatedBy = "NA";
				}

				result.CreationDate = communicationMode.CreatedOn.ToString("dd/MM/yyyy");

				if (communicationMode.ModifiedBy != null)
				{
					var modifiedByUser = await _userRoleService.GetADUserByADUserId(communicationMode.ModifiedBy);
					result.UpdatedByName = modifiedByUser?.DisplayName ?? "NA";
				}
				else
				{
					result.UpdatedByName = "NA";
				}

				result.UpdatedDate = communicationMode.ModifiedOn?.ToString("dd/MM/yyyy") ?? "";
			}
			// If communicationMode is null
			else
			{
				throw new Exception($"Communication mode with ID {id} not found.");
			}

			return result;
		}

		/// <summary>
		/// Insert & Update Communication Mode
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<(bool, string)> InsertUpdateAsync(MasterDataInputModel model)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model), "The model parameter cannot be null.");
			}

			if (string.IsNullOrWhiteSpace(model.Name))
			{
				return (false, "Name is required.");
			}
			var communicationModeList = await _masterDataService.GetAllCommunicationModeAsync();
			var existingMode = communicationModeList.FirstOrDefault(x => x.Name.Trim().ToLower() == model.Name.Trim().ToLower());

			if (model.Id > 0)
			{
				if (existingMode != null && existingMode.Id != model.Id)
				{
					return (false, "This Name of Communication Mode already exists.");
				}

				var communicationMode = await _masterDataService.GetCommunicationModesByIdAsync(model.Id);
				if (communicationMode != null)
				{
					communicationMode.Name = model.Name;
					communicationMode.ModifiedBy = model.UserId;
					communicationMode.ModifiedOn = DateTime.UtcNow;
					await _masterDataService.UpdateCommunicationModeAsync(communicationMode);
					return (true, "Successfully Updated.");
				}
				else
				{
					return (false, "No record found for update.");
				}
			}
			else
			{
				if (existingMode != null)
				{
					return (false, "This Name of Communication Mode already exists.");
				}
				var communicationMode = new CommunicationMode();
				communicationMode.Name = model.Name;
				communicationMode.CreatedBy = model.UserId;
				communicationMode.CreatedOn = DateTime.UtcNow;
				communicationMode.IsDeleted = false;
				await _masterDataService.InsertCommunicationModeAsync(communicationMode);
				return (true, "Successfully Inserted.");
			}
		}

		/// <summary>
		/// Delete Communication Mode
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<(bool, string)> DeleteCommunicationModeAsync(int id)
		{
			if (id <= 0)
			{
				return (false, "Invalid ID. The ID must be greater than zero.");
			}

			var communicationMode = await _masterDataService.GetCommunicationModesByIdAsync(id);

			if (communicationMode == null)
			{
				return (false, "No record found for deletion.");
			}

			// Check if the current user has the privilege to delete the record

			// If the user is super admin, allow record deletion
			communicationMode.ModifiedBy = communicationMode.ModifiedBy;
			communicationMode.ModifiedOn = DateTime.UtcNow;
			communicationMode.IsDeleted = true;
			await _masterDataService.UpdateCommunicationModeAsync(communicationMode);

			return (true, "Record deleted successfully.");
		}


		#endregion Communication Mode
		#region Resource Type
		/// <summary>
		/// Get All active list of resource type data
		/// </summary>
		/// <returns></returns>
		public async Task<List<Tm.Core.Domain.Pms.MasterData.ResourceType>> GetAllResourceTypeListAsync()
		{
			return await _masterDataService.GetAllResourceTypeListAsync();
		}
		#endregion

		#endregion


	}
}

