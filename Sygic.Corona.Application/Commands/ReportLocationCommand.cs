using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class ReportLocationCommand : IRequest
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public int Accuracy { get; }

        public ReportLocationCommand(uint profileId, string deviceId, double latitude, double longitude, int accuracy)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
        }
    }
}
