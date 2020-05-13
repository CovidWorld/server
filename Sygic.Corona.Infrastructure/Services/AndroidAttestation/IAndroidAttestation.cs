namespace Sygic.Corona.Infrastructure.Services.AndroidAttestation
{
    public interface IAndroidAttestation
    {
        AttestationStatement ParseAndVerify(string signedAttestationStatement);
    }
}