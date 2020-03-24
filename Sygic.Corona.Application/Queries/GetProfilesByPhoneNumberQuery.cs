using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetProfilesByPhoneNumberQuery : IRequest<IEnumerable<GetProfilesByPhoneNumberResponse>>
    {
        public string SearchTherm { get; }
        public int Limit { get; }

        public GetProfilesByPhoneNumberQuery(string searchTherm, int limit = 15)
        {
            SearchTherm = searchTherm;
            Limit = limit;
        }
    }
}
