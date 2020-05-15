using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.HashIdGenerating;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, CreateProfileResponse>
    {
        private readonly IRepository repository;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IHashIdGenerator hashGenerator;

        public CreateProfileCommandHandler(IRepository repository, ITokenGenerator tokenGenerator,
            IHashIdGenerator hashGenerator)
        {
            this.repository = repository;
            this.tokenGenerator = tokenGenerator;
            this.hashGenerator = hashGenerator;
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

                await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return new CreateProfileResponse { ProfileId = existingProfile.Id, DeviceId = existingProfile.DeviceId };
            }

            string token = tokenGenerator.Generate();

            var profile = new Profile(request.DeviceId, request.PushToken, request.Locale, token);

            var os = request.AppOperationSystem switch
            {
                "anr" => Platform.Android,
                "ios" => Platform.Ios,
                _ => Platform.Undefined
            };

            profile.AddClientInfo(new ClientInfo(request.AppName, request.AppIntegrator, request.AppVersion, os));

            await repository.CreateProfileAsync(profile, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            profile.ChangeMedicalId(hashGenerator.Generate(profile.Id));
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProfileResponse { ProfileId = profile.Id, DeviceId = profile.DeviceId };
        }
    }
}
