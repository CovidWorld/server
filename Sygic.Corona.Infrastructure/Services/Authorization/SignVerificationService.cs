using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;

namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public class SignVerificationService : ISignVerification
    {
        public bool Verify(string message, string publicKey, string signature)
        {
            byte[] keyBytes = Convert.FromBase64String(publicKey);
            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            byte[] sigBytes = Convert.FromBase64String(signature);

            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var q = curve.Curve.DecodePoint(keyBytes);
            var pubKeyParameters = new ECPublicKeyParameters(q, domain);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(false, pubKeyParameters);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(sigBytes);
        }
    }
}
