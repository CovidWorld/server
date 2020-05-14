namespace Sygic.Corona.Infrastructure.Services.Authorization
{
    public interface ISignVerification
    {
        bool Verify(string message, string publicKey, string signature);
    }
}