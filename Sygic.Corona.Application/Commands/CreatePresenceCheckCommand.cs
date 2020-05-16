using MediatR;
using Sygic.Corona.Domain;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class CreatePresenceCheckCommand : IRequest
    {
        public string CovidPass { get; }
        public TimeSpan DeadLineTime { get; }
        public PresenceCheckStatus Status { get; }

        public CreatePresenceCheckCommand(string covidPass, TimeSpan deadLineTime, PresenceCheckStatus status)
        {
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
            DeadLineTime = deadLineTime;
            Status = status;
        }
    }
}