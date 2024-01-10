using API.Models.BaseModels;
using Microsoft.AspNetCore.Http;

namespace API.Models.StaticContent
{
    public partial class UploadEditorFileRequestModel : BaseRequestModel
    {
        public string FileName { get; set; }
        public IFormFile FileData { get; set; }
    }
}