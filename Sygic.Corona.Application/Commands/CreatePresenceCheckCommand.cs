using MediatR;

namespace Sygic.Corona.Application.Commands
{
    public class CreatePresenceCheckCommand : IRequest
    {
        public string CovidPass { get; }

        public CreatePresenceCheckCommand(string covidPass)
        {
            CovidPass = covidPass;
        }
    }
}