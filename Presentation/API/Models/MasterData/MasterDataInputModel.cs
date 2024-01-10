using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Tm.Core;
using Tm.Core.Domain.Pms.MasterData;

namespace API.Models.MasterData
{
    public class MasterDataInputModel : BaseRequestModel
    {
        public string Name { get; set; }
        public string HOD { get; set; }
        public string UserId { get; set; }
    }
  
    public class MasterDomainDataModel: MasterDataResponseModel
    {
        public string HOD { get; set; }
        public string HODName { get; set; }
    }
    public class MasterDataCurrencyModel : BaseRequestModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CurrencyIcon { get; set; }
        public List<IFormFile> AttachFiles { get; set; }
    }
    public class MasterDataInputStackModel : BaseRequestModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
    public class MasterDataStackModel 
    {      
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string DepartName { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
    public class MasterDataStackResponseModel
    {
        public int StackId { get; set; }
        public string StackName { get; set; }
        public string DepartName { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }

    }

    public class MasterTeckStackResponseModel
    {
        public MasterTeckStackResponseModel()
        {
            FETeckStackList = new List<SelectListItem>();
            BETeckStackList = new List<SelectListItem>();
        }
      public  List<SelectListItem> FETeckStackList { get; set; }
      public List<SelectListItem> BETeckStackList { get; set; }
    }


    public class MasterDataResponseCurrencyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }      
        public string CurrencyIcon { get; set; }    
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }

}
