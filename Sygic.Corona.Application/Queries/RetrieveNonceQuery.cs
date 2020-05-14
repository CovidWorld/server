using System;
using MediatR;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Application.Queries
{
    public class RetrieveNonceQuery : IRequest<CovidPassNonceCacheEntry>
    {
        public RetrieveNonceQuery(string covidPass)
        {
            CovidPass = covidPass  ?? throw new ArgumentNullException(nameof(covidPass));
        }

        public string CovidPass { get; }
    }
}