using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace HR.LeaveManagement.Application.Behaviors;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handle Request {RequestName} {@request}",
            requestName, JsonSerializer.Serialize(request));

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();
            stopwatch.Stop();

            logger.LogInformation("Handle Request {RequestName} in {ElapsedInMilliseconds} ms",
                requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
              ex,
              "Request {RequestName} failed after {ElapsedMilliseconds} ms",
              requestName,
              stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
