using FluentValidation;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Application.Validations
{
    public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
    {
        public CreateProfileCommandValidator()
        {
            RuleFor(x => x.Locale).NotEmpty().NotNull().WithMessage("Provide language for profile (locale).");
            RuleFor(x => x.DeviceId).NotEmpty().NotNull().WithMessage("Provide Device ID.");
        }
    }
}
