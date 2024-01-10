using System.Collections.Generic;

namespace API.Models.StaticContent
{
    public partial class TestMessageTemplateResponseModel
    {
        public TestMessageTemplateResponseModel()
        {
            Tokens = new List<string>();
        }

        public int LanguageId { get; set; }

        public List<string> Tokens { get; set; }

        public string SendTo { get; set; }
    }
}