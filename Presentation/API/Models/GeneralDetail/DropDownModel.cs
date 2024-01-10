using API.Models.BaseModels;
using API.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Tm.Core.Domain.Pms.MasterData;

namespace API.Models.GeneralDetail
{
    public class DropDownModel : BaseRequestModel
    {
        public List<OrderResponseModel> OrderNameList { get; set; }
        public List<MasterDataResponseModel> ProjectStatusList { get; set; }
        public List<ProjectManagerResponseModel> ProjectManagersList { get; set; }
    }
}
