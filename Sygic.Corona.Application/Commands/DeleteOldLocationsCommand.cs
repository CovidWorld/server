using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteOldLocationsCommand : IRequest
    {
        public TimeSpan DeleteInterval { get; }

        public DeleteOldLocationsCommand(TimeSpan deleteInterval)
        {
            DeleteInterval = deleteInterval;
        }
    }
}
