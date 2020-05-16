using System.Text.RegularExpressions;

namespace Sygic.Corona.Infrastructure.Services.ClientInfo
{
    public class ClientInfoService : IClientInfo
    {
        public AppClientInfo Parse(string input)
        {
            var regex = new Regex("(?<app>[A-Za-z]+)\\(?(?<integrator>[A-Za-z0-9.]+)?\\)?\\/(?<version>[0-9\\.]+)\\s*,?\\s*\\((?<os>[A-Za-z0-9]+)");

            var match = regex.Match(input);
            return new AppClientInfo
            {
                Name = match.Groups["app"].Value,
                Integrator = match.Groups["integrator"].Value,
                Version = match.Groups["version"].Value,
                OperationSystem = match.Groups["os"].Value
            };
        }
    }
}
