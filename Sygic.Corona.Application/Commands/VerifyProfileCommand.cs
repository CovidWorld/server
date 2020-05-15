using MediatR;
using System;

namespace Sygic.Corona.Application.Commands
{
    public class VerifyProfileCommand : IRequest
    {
        public string DeviceId { get; }
        public long ProfileId { get; }
        public string CovidPass { get; }
        public string Nonce { get; }
        public string PublicKey { get; }
        public string SignedAttestationStatement { get; }

        public VerifyProfileCommand(string deviceId, long profileId, string covidPass, string nonce, string publicKey, string signedAttestationStatement)
        {
            DeviceId = deviceId ?? throw new ArgumentNullException(nameof(deviceId));
            ProfileId = profileId;
            CovidPass = covidPass ?? throw new ArgumentNullException(nameof(covidPass));
            Nonce = nonce ?? throw new ArgumentNullException(nameof(nonce));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            SignedAttestationStatement = signedAttestationStatement;
        }
    }
}
