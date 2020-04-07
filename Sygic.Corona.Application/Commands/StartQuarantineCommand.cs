using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class StartQuarantineCommand : IRequest
    {
        public string DeviceId { get; }
        public uint ProfileId { get; }
        public string CovidPass { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public StartQuarantineCommand(string deviceId, uint profileId, string covidPass, DateTime startDate, DateTime endDate)
        {
            DeviceId = deviceId;
            ProfileId = profileId;
            CovidPass = covidPass;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
