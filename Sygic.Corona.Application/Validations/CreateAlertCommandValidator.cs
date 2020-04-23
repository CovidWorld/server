using FluentValidation;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Application.Validations
{
    public class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
    {
        public CreateAlertCommandValidator()
        {
            RuleFor(x => x.PushBody).NotNull().NotEmpty()
                .When(x => x.WithPushNotification.HasValue && x.WithPushNotification.Value);
            RuleFor(x => x.PushSubject).NotNull().NotEmpty()
                .When(x => x.WithPushNotification.HasValue && x.WithPushNotification.Value);
        }
    }
}
