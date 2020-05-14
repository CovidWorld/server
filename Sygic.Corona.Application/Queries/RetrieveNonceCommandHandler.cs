using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Sygic.Corona.Application.Commands;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.NonceGenerating;

namespace Sygic.Corona.Application.Queries
{
    public class RetrieveNonceCommandHandler : RequestHandler<RetrieveNonceQuery, CovidPassNonceCacheEntry>
    {
        private readonly IMemoryCache memoryCache;
        private readonly INonceGenerator nonceGenerator;

        public RetrieveNonceCommandHandler(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        protected override CovidPassNonceCacheEntry Handle(RetrieveNonceQuery request)
        {
            if (string.IsNullOrEmpty(request.CovidPass))
            {
                throw new DomainException($"{nameof(request.CovidPass)} cannot be null or empty");
            }

            if (!memoryCache.TryGetValue<CovidPassNonceCacheEntry>(CovidPassNonceCacheEntry.GetKey(request.CovidPass), out var cacheEntry))
            {
                throw new DomainException($"{typeof(CovidPassNonceCacheEntry)} could not be found for {nameof(request.CovidPass)}: '{request.CovidPass}'");
            }

            return cacheEntry;
        }
    }
}