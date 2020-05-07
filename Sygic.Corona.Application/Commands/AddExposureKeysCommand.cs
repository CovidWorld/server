using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Requests;

namespace Sygic.Corona.Application.Commands
{
    public class AddExposureKeysCommand : IRequest
    {
        private readonly List<ExposureKeyRequest> exposureKeys;
        public List<ExposureKeyRequest> ExposureKeys => exposureKeys;

        public AddExposureKeysCommand()
        {
            exposureKeys = new List<ExposureKeyRequest>();
        }
        public AddExposureKeysCommand(IEnumerable<ExposureKeyRequest> exposureKeys) : this()
        {
            this.exposureKeys.AddRange(exposureKeys);
        }
    }
}
