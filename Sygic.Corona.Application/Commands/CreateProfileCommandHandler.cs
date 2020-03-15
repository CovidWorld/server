using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
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
            var existingProfile = await repository.GetProfileAsync(request.DeviceId, cancellationToken);
            if (existingProfile != null)
            {
                if (existingProfile.PushToken != request.PushToken)
                {
                    existingProfile.UpdatePushToken(request.PushToken);
                }

                if (existingProfile.PhoneNumber != request.PhoneNumber)
                {
                    existingProfile.UpdatePhoneNumber(request.PhoneNumber);
                }
                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return new CreateProfileResponse { ProfileId = existingProfile.Id, DeviceId = existingProfile.DeviceId };
            }

            uint lastId = await repository.GetLastIdAsync(cancellationToken);
            uint nextId = lastId + 1;

            string token = tokenGenerator.Generate();

            var location = new Location(request.Latitude, request.Longitude, request.Accuracy);
            var profile = new Profile(nextId, request.DeviceId, request.PushToken, request.Locale, location, token, request.PhoneNumber);

            await repository.CreateProfileAsync(profile, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProfileResponse{ ProfileId = profile.Id, DeviceId = profile.DeviceId };

        }
    }
}
