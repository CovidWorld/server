using System;
using System.Text;
using OtpNet;

namespace Sygic.Corona.Infrastructure.Services.TokenGenerating
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly string secretKey;

        public TokenGenerator(string secretKey)
        {
            this.secretKey = secretKey;
        }
        public string Generate()
        {
            var totp = new Totp(Encoding.ASCII.GetBytes(secretKey));

            return totp.ComputeTotp(DateTime.UtcNow);
        }
    }
}
