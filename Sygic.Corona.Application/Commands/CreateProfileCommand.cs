using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommand : IRequest<CreateProfileResponse>
    {
        public string DeviceId { get; }
        public string PushToken { get; }
        public string Locale { get; }

        public CreateProfileCommand(string deviceId, string pushToken, string locale)
        {
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
        }
    }
}
