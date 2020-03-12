using FluentValidation;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Application.Validations
{
    public class AddContactsCommandValidator : AbstractValidator<AddContactsCommand>
    {
        public AddContactsCommandValidator()
        {
            
        }
    }
}
