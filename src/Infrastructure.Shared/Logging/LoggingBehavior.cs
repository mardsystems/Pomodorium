using MediatR;
using Microsoft.Extensions.Logging;
using System.ApplicationModel;

namespace Pomodorium.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Request<TResponse>
    where TResponse: Response
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var correlationId = request.GetCorrelationId();

        _logger.LogInformation("Handling {RequestName} on {MachineName} with Correlation {CorrelationId}", typeof(TRequest).Name, Environment.MachineName, correlationId);

        var response = await next();

        System.Diagnostics.Debug.Assert(response.CorrelationId == correlationId);

        _logger.LogInformation("Handled {ResponseName} on {MachineName} with Correlation {CorrelationId}", typeof(TResponse).Name, Environment.MachineName, correlationId);

        return response;
    }
}
