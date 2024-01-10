using API.Models.BaseModels;
using System.Collections.Generic;

namespace API.Models.StaticContent
{
    public partial class TestMessageTemplateRequestModel : BaseRequestModel
    {
        public TestMessageTemplateRequestModel()
        {
            Tokens = new List<CustomTokenList>();
        }
        public string Name { get; set; }

        public int BrandId { get; set; }

        public List<CustomTokenList> Tokens { get; set; }

        public string SendTo { get; set; }
    }

    public class CustomTokenList
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}

