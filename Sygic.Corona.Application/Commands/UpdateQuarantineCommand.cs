using System;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class UpdateQuarantineCommand : IRequest
    {
        public string CovidPass { get; }
        public DateTime QuarantineStart { get; }
        public DateTime QuarantineEnd { get; }
        public DateTime BorderCrossedAt { get; }
        public Address QuarantineAddress { get; }

        public UpdateQuarantineCommand(string covidPass, DateTime quarantineStart, DateTime quarantineEnd, DateTime borderCrossedAt, Address quarantineAddress)
        {
            CovidPass = covidPass;
            QuarantineStart = quarantineStart;
            QuarantineEnd = quarantineEnd;
            BorderCrossedAt = borderCrossedAt;
            QuarantineAddress = quarantineAddress;
        }
    }
}