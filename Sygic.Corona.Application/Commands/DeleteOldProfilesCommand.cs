using MediatR;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldProfilesCommand : IRequest
    {
        public DeleteOldProfilesCommand(TimeSpan interval)
        {
            Interval = interval;
        }

        public TimeSpan Interval { get; }
    }
}
