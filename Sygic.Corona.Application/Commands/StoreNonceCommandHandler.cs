using System;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Sygic.Corona.Domain.Common;
using Sygic.Corona.Infrastructure.Services.NonceGenerating;

namespace Sygic.Corona.Application.Commands
{
    public class StoreNonceCommandHandler : RequestHandler<StoreNonceCommand>
    {
        private readonly IMemoryCache memoryCache;
        private readonly INonceGenerator nonceGenerator;

        public StoreNonceCommandHandler(IMemoryCache memoryCache, INonceGenerator nonceGenerator)
        {
            this.memoryCache = memoryCache;
            this.nonceGenerator = nonceGenerator;
        }

        protected override void Handle(StoreNonceCommand request)
        {
            if (string.IsNullOrEmpty(request.CovidPass))
            {
                throw new DomainException($"{nameof(request.CovidPass)} cannot be null or empty");
            }
            
            var cacheEntry = new CovidPassNonceCacheEntry(request.CovidPass, nonceGenerator.Generate());
            memoryCache.Set(cacheEntry.Key, cacheEntry, TimeSpan.FromSeconds(15));
        }
    }

    public class CovidPassNonceCacheEntry
    {
        public string Key => GetKey(CovidPass);
        public string CovidPass { get; }
        public string Nonce { get; }

        public CovidPassNonceCacheEntry(string covidPass, string nonce)
        {
            CovidPass = covidPass;
            Nonce = nonce;
        }

        public static string GetKey(string covidPass)
        {
            return $"CovidPassNonceCacheEntry.{covidPass}";
        }
    }
}