using System;
using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class StoreNonceCommand : IRequest
    {
        public StoreNonceCommand(string covidPass)
        {
            CovidPass = covidPass  ?? throw new ArgumentNullException(nameof(covidPass));
        }

        public string CovidPass { get; }
    }
}