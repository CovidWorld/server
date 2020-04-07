using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure.Services.AutoNumberGenerating;
using Sygic.Corona.Infrastructure.Services.HashIdGenerating;
using Sygic.Corona.Infrastructure.Services.TokenGenerating;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, CreateProfileResponse>
    {
        private readonly IRepository repository;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IHashIdGenerator hashGenerator;
        private readonly IAutoNumberGenerator idGenerator;

        public CreateProfileCommandHandler(IRepository repository, ITokenGenerator tokenGenerator, 
            IHashIdGenerator hashGenerator, IAutoNumberGenerator idGenerator)
        {
            this.repository = repository;
            this.tokenGenerator = tokenGenerator;
            this.hashGenerator = hashGenerator;
            this.idGenerator = idGenerator;
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

            uint nextId = idGenerator.Generate("profile");

            string token = tokenGenerator.Generate();

            var profile = new Profile(nextId, request.DeviceId, request.PushToken, request.Locale,token);

            await repository.CreateProfileAsync(profile, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            profile.ChangeMedicalId(hashGenerator.Generate(profile.Id));
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProfileResponse{ ProfileId = profile.Id, DeviceId = profile.DeviceId };

        }
    }
}
