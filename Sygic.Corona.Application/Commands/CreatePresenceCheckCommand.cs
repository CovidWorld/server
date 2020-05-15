using MediatR;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class CreatePresenceCheckCommand : IRequest
    {
        public string CovidPass { get; }
        public TimeSpan DeadLineTime { get; }

        public CreatePresenceCheckCommand(string covidPass, TimeSpan deadLineTime)
        {
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
            DeadLineTime = deadLineTime;
        }
    }
}