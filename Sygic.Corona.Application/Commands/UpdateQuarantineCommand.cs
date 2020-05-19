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
        public string NotificationTitle { get; }
        public string NotificationBody { get; }

        public UpdateQuarantineCommand(string covidPass, DateTime quarantineStart, DateTime quarantineEnd, DateTime borderCrossedAt, Address quarantineAddress, string notificationTitle, string notificationBody)
        {
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
            QuarantineStart = quarantineStart;
            QuarantineEnd = quarantineEnd;
            BorderCrossedAt = borderCrossedAt;
            QuarantineAddress = quarantineAddress ?? throw new ArgumentNullException(nameof(quarantineAddress));
            NotificationTitle = notificationTitle;
            NotificationBody = notificationBody;
        }
    }
}