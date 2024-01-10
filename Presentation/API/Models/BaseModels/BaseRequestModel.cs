using Tm.Framework.Models;

namespace API.Models.BaseModels
{
    public partial class BaseRequestModel: BaseTmModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string PublicKey { get; set; }

    }
}