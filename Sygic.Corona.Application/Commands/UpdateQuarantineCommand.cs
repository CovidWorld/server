using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class UpdateQuarantineCommand : IRequest
    {
        public string CovidPass { get; }
        public DateTime QuarantineStart { get; }
        public DateTime QuarantineEnd { get; }

        public UpdateQuarantineCommand(string covidPass, DateTime quarantineStart, DateTime quarantineEnd)
        {
            CovidPass = covidPass;
            QuarantineStart = quarantineStart;
            QuarantineEnd = quarantineEnd;
        }
    }
}