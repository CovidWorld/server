using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class DeleteExposureKeysCommand : IRequest
    {
        private readonly List<ExposureKey> exposureKeys;
        public List<ExposureKey> ExposureKeys => exposureKeys;

        public DeleteExposureKeysCommand()
        {
            exposureKeys = new List<ExposureKey>();
        }
        public DeleteExposureKeysCommand(IEnumerable<ExposureKey> exposureKeys)
            : this()
        {
            this.exposureKeys.AddRange(exposureKeys);
        }
    }
}
