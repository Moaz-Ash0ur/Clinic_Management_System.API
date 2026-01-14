using ClinicManagement.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
    {
        private readonly ILogger _logger = logger;
        private readonly IUser _user = user;
        private readonly IIdentityService _identityService = identityService;


        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation(
                "Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
        }
    } 


    //public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    //where TRequest : notnull
    //{
    //    private readonly Stopwatch _timer;
    //    private readonly ILogger<TRequest> _logger;
    //    private readonly IUser _user;
    //    private readonly IIdentityService _identityService;

    //    public PerformanceBehaviour(
    //        ILogger<TRequest> logger,
    //        IUser user,
    //        IIdentityService identityService)
    //    {
    //        _timer = new Stopwatch();

    //        _logger = logger;
    //        _user = user;
    //        _identityService = identityService;
    //    }

    //    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    //    {
    //        _timer.Start();

    //        var response = await next();

    //        _timer.Stop();

    //        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

    //        if (elapsedMilliseconds > 500)
    //        {
    //            var requestName = typeof(TRequest).Name;
    //            var userId = _user.Id ?? string.Empty;
    //            var userName = string.Empty;

    //            if (!string.IsNullOrEmpty(userId))
    //            {
    //                userName = await _identityService.GetUserNameAsync(userId);
    //            }

    //            _logger.LogWarning(
    //                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}", requestName, elapsedMilliseconds, userId, userName, request);
    //        }

    //        return response;
    //    }
    //}


    //public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    //: IPipelineBehavior<TRequest, TResponse>
    //where TRequest : notnull
    //{
    //    private readonly ILogger<TRequest> _logger = logger;

    //    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    //    {
    //        try
    //        {
    //            return await next(ct);
    //        }
    //        catch (Exception ex)
    //        {
    //            var requestName = typeof(TRequest).Name;

    //            _logger.LogError(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

    //            throw;
    //        }
    //    }
    //}
}
