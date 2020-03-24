using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldContactsCommand : IRequest
    {
        public TimeSpan DeleteInterval { get; }

        public DeleteOldContactsCommand(TimeSpan deleteInterval)
        {
            DeleteInterval = deleteInterval;
        }
    }
}
