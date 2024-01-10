using API.Models.BaseModels;

namespace API.Models.GeneralDetail
{
    public class ProjectManagerDetailResponseModel : BaseRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
