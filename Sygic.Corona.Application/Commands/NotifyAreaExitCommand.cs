using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class NotifyAreaExitCommand : IRequest
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public int Accuracy { get; }
        public DateTime RecordDateUtc { get; }

        public NotifyAreaExitCommand(uint profileId, string deviceId, double latitude, double longitude, int accuracy, DateTime recordDateUtc)
        {
            ProfileId = profileId;
            DeviceId = deviceId;
            Latitude = latitude;
            Longitude = longitude;
            Accuracy = accuracy;
            RecordDateUtc = recordDateUtc;
        }
    }
}
