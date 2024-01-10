using API.Models.BaseModels;

namespace API.Models.StaticContent
{
    public partial class InsertUpdateMessageTemplateModel : BaseRequestModel
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public int BrandId { get; set; }
    }
}