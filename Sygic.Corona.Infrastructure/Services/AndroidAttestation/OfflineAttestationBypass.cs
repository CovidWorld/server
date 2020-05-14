using System.Collections.Generic;

namespace Sygic.Corona.Infrastructure.Services.AndroidAttestation
{
    public class OfflineAttestationBypass : IAndroidAttestation
    {
        public AttestationStatement ParseAndVerify(string signedAttestationStatement)
        {
            var claims = new Dictionary<string, string>();
            return new AttestationStatement(claims);
        }
    }
}
