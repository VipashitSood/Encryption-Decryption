using API.Models.BaseModels;

namespace API.Models.EmailManager
{
    public partial class EmailManagerModel : BaseRequestModel
    {
        public string MessageTemplateSystemName { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
    }
}