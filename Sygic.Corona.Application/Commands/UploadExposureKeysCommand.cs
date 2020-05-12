using System;
using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Commands
{
    public class UploadExposureKeysCommand : IRequest
    {
        private readonly List<ExposureKey> exposureKeys;
        public List<ExposureKey> ExposureKeys => exposureKeys;
        public DateTime DaySpecification { get; }
        public UploadExposureKeysCommand()
        {
            exposureKeys = new List<ExposureKey>();
        }
        public UploadExposureKeysCommand(IEnumerable<ExposureKey> exposureKeys, DateTime daySpecification) 
            : this()
        {
            this.exposureKeys.AddRange(exposureKeys);
            DaySpecification = daySpecification;
        }
    }
}
