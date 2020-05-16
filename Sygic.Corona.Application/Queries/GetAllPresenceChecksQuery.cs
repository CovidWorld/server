using System;
using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetAllPresenceChecksQuery : IRequest<GetAllPresenceChecksResponse>
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public TimeSpan Offset { get; }

        public GetAllPresenceChecksQuery(DateTime @from, DateTime to, TimeSpan offset)
        {
            From = @from;
            To = to;
            Offset = offset;
        }
    }
}