using System.Diagnostics;
using System.Text;

namespace QuizApp.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        _logger.LogInformation("Incoming Request: {Method} {Path}", request.Method, request.Path);

        string? requestBody = null;
        if (request.ContentLength > 0 && request.Body.CanSeek)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            _logger.LogInformation("Request Body: {Body}", requestBody);
        }

        foreach (var header in request.Headers)
        {
            _logger.LogInformation("{Header}: {Value}", header.Key, header.Value);
        }

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);
        stopwatch.Stop();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("Response: {StatusCode} ({Elapsed} ms)", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        _logger.LogInformation("Response Body: {Body}", responseBodyText);

        await responseBody.CopyToAsync(originalResponseBodyStream);
    }
}
