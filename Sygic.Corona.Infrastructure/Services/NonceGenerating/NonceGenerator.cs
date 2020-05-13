using System;
using System.Security.Cryptography;

namespace Sygic.Corona.Infrastructure.Services.NonceGenerating
{
    public class NonceGenerator : INonceGenerator
    {
        public string Generate()
        {
            var rng = new RNGCryptoServiceProvider();
            var nonceBytes = new byte[32];
            rng.GetBytes(nonceBytes);
            return Convert.ToBase64String(nonceBytes);
        }
    }
}
