using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class ReportLocationCommand : IRequest
    {
        public uint ProfileId { get; }
        public string DeviceId { get; }
        private readonly List<LocationRequest> locations;
        public List<LocationRequest> Locations => locations;

        protected ReportLocationCommand()
        {
            locations = new List<LocationRequest>();
        }
        public ReportLocationCommand(uint profileId, string deviceId, IEnumerable<LocationRequest> locations) : this()
        {
            ProfileId = profileId;
            DeviceId = deviceId;
            Locations.AddRange(locations);
        }
    }
}
