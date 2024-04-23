using System.Text;

namespace CathaybkHW.Middleware;

/// <summary>
/// Middleware for logging request and response
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();
        var buffer = new byte[context.Request.ContentLength ?? 0];
        await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        string requestBody = Encoding.UTF8.GetString(buffer);
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation($"Request Body: {requestBody}");

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);  // Call the next middleware

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation($"Response Body: {responseBodyText}");

        // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
        await responseBody.CopyToAsync(originalResponseBodyStream);
        context.Response.Body = originalResponseBodyStream;
    }
}