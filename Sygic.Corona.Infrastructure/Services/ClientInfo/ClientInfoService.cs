using System.Text.RegularExpressions;

namespace Sygic.Corona.Infrastructure.Services.ClientInfo
{
    public class ClientInfoService : IClientInfo
    {
        private readonly string regexExpression;

        public ClientInfoService(string regexExpression)
        {
            this.regexExpression = regexExpression;
        }

        public AppClientInfo Parse(string input)
        {
            var regex = new Regex(regexExpression);

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
