using System;
using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetInactiveProfilesQuery : IRequest<IEnumerable<Profile>>
    {
        public DateTime CheckDate { get; }
        public TimeSpan CheckInterval { get; }

        public GetInactiveProfilesQuery(DateTime checkDate, TimeSpan checkInterval)
        {
            CheckDate = checkDate;
            CheckInterval = checkInterval;
        }
    }
}
