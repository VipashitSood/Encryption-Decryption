using API.Models.BaseModels;

namespace API.Models.GeneralDetail
{
    public class ProjectManagerResponseModel : BaseRequestModel
    {
        public string ManagerName { get; set; }
        public string UserId { get; set; }
    }
}
