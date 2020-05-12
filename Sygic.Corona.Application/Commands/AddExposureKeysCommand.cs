using System;
using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class AddExposureKeysCommand : IRequest
    {
        private readonly List<ExposureKeyRequest> exposureKeys;
        public List<ExposureKeyRequest> ExposureKeys => exposureKeys;
        public TimeSpan ExpirationTime { get; }

        public AddExposureKeysCommand()
        {
            exposureKeys = new List<ExposureKeyRequest>();
        }
        public AddExposureKeysCommand(IEnumerable<ExposureKeyRequest> exposureKeys, TimeSpan expirationTime) : this()
        {
            ExpirationTime = expirationTime;
            this.exposureKeys.AddRange(exposureKeys);
        }
    }
}
