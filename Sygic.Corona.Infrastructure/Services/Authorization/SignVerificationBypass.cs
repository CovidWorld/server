namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public class SignVerificationBypass : ISignVerification
    {
        public bool Verify(string message, string publicKey, string signature)
        {
            return true;
        }
    }
}
