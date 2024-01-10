using API.Models.StaticContent;
using FluentValidation;
using Tm.Framework.Validators;
using Tm.Services.Localization;

namespace API.Validators.StaticContent
{
    public partial class StaticContentRequestValidator : BaseTmValidator<StaticContentRequestModel>
    {
        public StaticContentRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).Must((x, context) =>
            {
                if (x.Id<=0 && string.IsNullOrEmpty(x.SystemName))
                    return false;
                return true;
            }).WithMessage(localizationService.GetResource("Tm.API.StaticContent.Fields.IdOrSystemName.Required"));
        }
    }
}