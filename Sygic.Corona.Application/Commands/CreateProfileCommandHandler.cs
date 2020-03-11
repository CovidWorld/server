using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, CreateProfileResponse>
    {
        private readonly IRepository repository;

        public CreateProfileCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<CreateProfileResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            long lastId = await repository.GetLastId(cancellationToken);
            long nextId = lastId + 1;
            var location = new Location(request.Latitude, request.Longitude, request.Accuracy);
            var profile = new Profile(nextId, request.DeviceId, request.PushToken, request.Locale, location, request.AuthToken);

            if (await repository.AlreadyCreated(profile.DeviceId, cancellationToken))
            {
                throw new ArgumentException("Profile already created.");
            }

            await repository.CreateProfileAsync(profile, cancellationToken);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProfileResponse{ Id = nextId };

        }
    }
}
