using API.Models.BaseModels;

namespace API.Models.StaticContent
{
    public partial class MessageTemplateResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}