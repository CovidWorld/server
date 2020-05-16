using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public interface ISignVerification
    {
        bool Verify(string message, string publicKey, string signature);
        bool Verify(string message, ECPublicKeyParameters pubKey, string signature);
        string SignData(string msg, ECPrivateKeyParameters privKey);
        public AsymmetricCipherKeyPair GenerateKeyPair();
    }
}