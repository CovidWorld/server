using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Application.Commands
{
    public class VerifyMfaTokenCommandHandler : AsyncRequestHandler<VerifyMfaTokenCommand>
    {
        private readonly IRepository repository;

        public VerifyMfaTokenCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(VerifyMfaTokenCommand request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsync(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile == null)
            {
                throw new DomainException("Profile not found.");
            }

            if (request.MfaToken != profile.AuthToken)
            {
                throw new DomainException("Wrong mfa token.");
            }
            else
            {
                profile.Verify();
                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
