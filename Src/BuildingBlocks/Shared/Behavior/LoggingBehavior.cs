using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contract.ResultPattern;
using System.Diagnostics;

namespace Shared.Behavior
{
    internal class LoggingBehavior<TRequest, TRespons>(ILogger<LoggingBehavior<TRequest, TRespons>> _logger)
        : IPipelineBehavior<TRequest, TRespons> where TRequest
        : IBaseRequest where TRespons : Result
    {
        public async Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] Handle Request{TRequest}",
                typeof(TRequest).Name, typeof(TRespons).Name, request);

            var timer = new Stopwatch();

            var result = await next();
            timer.Stop();

            var timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3)
                _logger.LogWarning("request tak mor than 3 seconds",
                    typeof(TRequest).Name, typeof(TRespons).Name, request);
            _logger.LogInformation("End Request", typeof(TRequest).Name, typeof(TRespons).Name, request);

            return result;
        }
    }
}
