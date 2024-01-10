using API.Models.Accounts;
using FluentValidation;
using Tm.Framework.Validators;
using Tm.Services.Localization;

namespace API.Validators.Accounts
{
    public partial class AccountModelValidator : BaseTmValidator<AccountModel>
    {
        public AccountModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AccountNo)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Account.Fields.AccountNo.Required"));
            RuleFor(x => x.AccountHolderName)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Address.Fields.AcountHolderName.Required"));
        }
    }
}