using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public class SignVerificationBypass : ISignVerification
    {
        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            throw new System.NotImplementedException();
        }

        public string SignData(string msg, ECPrivateKeyParameters privKey)
        {
            throw new System.NotImplementedException();
        }

        public bool Verify(string message, string publicKey, string signature)
        {
            return true;
        }

        public bool Verify(string message, ECPublicKeyParameters pubKey, string signature)
        {
            return true;
        }
    }
}
