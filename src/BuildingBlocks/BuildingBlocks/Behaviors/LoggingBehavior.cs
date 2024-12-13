using BuildingBlocks.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeToken = timer.Elapsed;
        if (timeToken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeToken} seconds",
                typeof(TRequest), timeToken.Seconds);

        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest), typeof(TResponse).Name);
        return response;
    }
}
