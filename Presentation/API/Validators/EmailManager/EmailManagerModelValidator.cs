using API.Models.EmailManager;
using FluentValidation;
using Tm.Framework.Validators;
using Tm.Services.Localization;

namespace API.Validators.EmailManager
{
    public partial class EmailManagerModelValidator : BaseTmValidator<EmailManagerModel>
    {
        public EmailManagerModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.MessageTemplateSystemName)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Tm.API.EmailManager.MessageTemplateSystemName.Required"));

            RuleFor(x => x.ToName)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Tm.API.EmailManager.ToName.Required"));

            RuleFor(x => x.ToEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
        }
    }
}