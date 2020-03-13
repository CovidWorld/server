using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommand : IRequest<CreateProfileResponse>
    {
        public string DeviceId { get; }
        public string PushToken { get; }
        public string Locale { get; }
        public double? Latitude { get; }
        public double? Longitude { get; }
        public int? Accuracy { get; }

        public CreateProfileCommand(string deviceId, string pushToken, string locale, double? latitude, double? longitude, int? accuracy)
        {
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }
    }
}
