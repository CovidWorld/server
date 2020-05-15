using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Commands
{
    public class CreateProfileCommand : IRequest<CreateProfileResponse>
    {
        public string DeviceId { get; }
        public string PushToken { get; }
        public string Locale { get; }
        public string AppName { get; }
        public string AppIntegrator { get; }
        public string AppVersion { get; }
        public string AppOperationSystem { get; }

        public CreateProfileCommand(string deviceId, string pushToken, string locale, string appName, string appIntegrator, string appVersion, string appOperationSystem)
        {
            DeviceId = deviceId;
            PushToken = pushToken;
            Locale = locale;
            AppName = appName;
            AppIntegrator = appIntegrator;
            AppVersion = appVersion;
            AppOperationSystem = appOperationSystem;
        }
    }
}
