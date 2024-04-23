using CathaybkHW.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CathaybkHW.Infrastructure.Extensions;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Request URI: {request.RequestUri} Method: {request.Method}");
        if (request.Content is not null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation($"Request Body: {requestBody}");
        }
        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Received successful response from {request.RequestUri}");
            if (response.Content is not null)
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation($"Response Body: {responseBody}");
            }
        }
        else
        {
            _logger.LogWarning($"Received unsuccessful response from {request.RequestUri}");
        }

        return response;
    }
}

public static class HttpClientExtensions
{
    public static IServiceCollection RegisterHttpClient(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddHttpClient(nameof(HttpClientNames.CoindeskAPI), client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("CoindeskAPI").GetValue<string>("BaseUrl")!);
        }).AddHttpMessageHandler(provider =>
        {
            return new LoggingHandler(provider.GetRequiredService<ILogger<LoggingHandler>>());
        });

        return service;
    }
}
