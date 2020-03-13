using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, CreateProfileResponse>
    {
        private readonly IRepository repository;
        private readonly ITokenGenerator tokenGenerator;

        public CreateProfileCommandHandler(IRepository repository, ITokenGenerator tokenGenerator)
        {
            this.repository = repository;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<CreateProfileResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            uint lastId = await repository.GetLastIdAsync(cancellationToken);
            uint nextId = lastId + 1;

            string token = tokenGenerator.Generate();

            var location = new Location(request.Latitude, request.Longitude, request.Accuracy);
            var profile = new Profile(nextId, request.DeviceId, request.PushToken, request.Locale, location, token);

            if (await repository.AlreadyCreatedAsync(profile.DeviceId, cancellationToken))
            {
                throw new DomainException("Profile already created.");
            }

            await repository.CreateProfileAsync(profile, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProfileResponse{ Id = nextId };

        }
    }
}
