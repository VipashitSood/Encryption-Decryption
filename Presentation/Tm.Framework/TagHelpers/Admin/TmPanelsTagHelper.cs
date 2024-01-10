using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Tm.Framework.TagHelpers.Admin
{
    /// <summary>
    /// nop-panel tag helper
    /// </summary>
    [HtmlTargetElement("nop-panels", Attributes = ID_ATTRIBUTE_NAME)]
    public class TmPanelsTagHelper : TagHelper
    {
        private const string ID_ATTRIBUTE_NAME = "id";

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
    }
}