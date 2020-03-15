using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;


namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public class FirebaseAuthService : IAuthService
    {
        private readonly HttpClient client;
        private readonly TokenValidationParameters validationParameters;

        public FirebaseAuthService(HttpClient client, TokenValidationParameters validationParameters)
        {
            this.client = client;
            this.validationParameters = validationParameters;
        }
        public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync("x509/securetoken@system.gserviceaccount.com", cancellationToken);
            var x509Data = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(await response.Content.ReadAsStreamAsync(),null, cancellationToken);

            var securityKeys = x509Data.Select(cert =>
                    new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(cert.Value)))
                    {
                        KeyId = cert.Key
                    }).ToArray();

            validationParameters.IssuerSigningKeys = securityKeys;

            var handler = new JsonWebTokenHandler();

            try
            {
                var tokenValidationResult = handler.ValidateToken(token, validationParameters);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
