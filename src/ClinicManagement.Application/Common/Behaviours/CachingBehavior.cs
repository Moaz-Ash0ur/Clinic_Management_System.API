using ClinicManagement.Domain.Common.Results.Abstractions;
using ClinicManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ClinicManagement.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {

        private readonly HybridCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(HybridCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

      
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {

            //check if request coming not implement my contract ICahcedQuery to insure if suppot do cahcing
            //if not return the handler excute wihtout chacing behaviour
            if (request is not ICachedQuery cachedRequest)
                return await next(ct);


            _logger.LogInformation("Checking cache for {RequestName}", typeof(TRequest).Name);

            //now write code for cahcing

            var result = await _cache.GetOrCreateAsync<TResponse>
                (cachedRequest.CacheKey,
                _ => new ValueTask<TResponse>((TResponse)(object)null!),
                 new HybridCacheEntryOptions
                 {
                     Flags = HybridCacheEntryFlags.DisableUnderlyingData
                 },
                cancellationToken: ct);


            //if not have cahce go to excute handler then return result to save it in cahce
            if (result is null)
            {
                result = await next(ct);

                //after excute if return handler success result complete store

                if (result is IResult res && res.IsSuccess)
                {
                    _logger.LogInformation("Caching result for {RequestName}", typeof(TRequest).Name);

                    await _cache.SetAsync(cachedRequest.CacheKey,result,
                    new HybridCacheEntryOptions
                    {
                        Expiration = cachedRequest.Expiration
                    },cachedRequest.Tags,ct);
                }
            }

            return result;
        }




    }
}
