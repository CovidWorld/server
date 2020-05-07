using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Queries
{
    public class GetRotatedExposureKeysQueryHandler : IRequestHandler<GetRotatedExposureKeysQuery, IEnumerable<ExposureKey>>
    {
        private readonly CoronaContext context;

        public GetRotatedExposureKeysQueryHandler(CoronaContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExposureKey>> Handle(GetRotatedExposureKeysQuery request, CancellationToken cancellationToken)
        {
            var keys = await context.ExposureKeys
                .Where(x => x.Expiration > request.ToDate)
                .ToListAsync(cancellationToken);

            return keys;
        }
    }
}
