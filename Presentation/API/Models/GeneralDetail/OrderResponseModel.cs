using API.Models.BaseModels;

namespace API.Models.GeneralDetail
{
    public class OrderResponseModel : BaseRequestModel
    {
        public string Value  { get; set; }
        public string Label { get; set; }
    }
}
