using System;
using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetRotatedExposureKeysQuery : IRequest<IEnumerable<ExposureKey>>
    {
        public DateTime ToDate { get; }

        public GetRotatedExposureKeysQuery(DateTime date)
        {
            ToDate = date;
        }
    }
}
