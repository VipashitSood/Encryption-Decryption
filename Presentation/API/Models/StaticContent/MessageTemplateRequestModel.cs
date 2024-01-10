using API.Models.BaseModels;

namespace API.Models.StaticContent
{
    public partial class MessageTemplateRequestModel : BaseRequestModel
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
    }
}