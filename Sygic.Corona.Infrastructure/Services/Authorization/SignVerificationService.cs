using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
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
            try
            {
                byte[] keyBytes = Convert.FromBase64String(publicKey);
                byte[] msgBytes = Encoding.UTF8.GetBytes(message);
                byte[] sigBytes = Convert.FromBase64String(signature);

                var curve = SecNamedCurves.GetByName("secp256r1");
                var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
                var q = curve.Curve.DecodePoint(keyBytes);
                var pubKeyParameters = new ECPublicKeyParameters(q, domain);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(false, pubKeyParameters);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool Verify(string message, ECPublicKeyParameters pubKey, string signature)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(message);
                byte[] sigBytes = Convert.FromBase64String(signature);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(false, pubKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string SignData(string msg, ECPrivateKeyParameters privKey)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(true, privKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                byte[] sigBytes = signer.GenerateSignature();

                return Convert.ToBase64String(sigBytes);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var curve = SecNamedCurves.GetByName("secp256r1");
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            var secureRandom = new SecureRandom();
            var keyParams = new ECKeyGenerationParameters(domainParams, secureRandom);

            var generator = new ECKeyPairGenerator("EC");
            generator.Init(keyParams);
            var keyPair = generator.GenerateKeyPair();

            var privateKey = keyPair.Private as ECPrivateKeyParameters;
            var publicKey = keyPair.Public as ECPublicKeyParameters;

            return keyPair;
        }
    }
}
