using API.Models.BaseModels;

namespace API.Models.StaticContent
{
    public partial class StaticContentRequestModel : BaseRequestModel
    {
        public string SystemName { get; set; }
    }
}