using System.Text;

namespace EmployeesAPI.Models;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //TODO понять почему не работает если раскомментировать блок ниже
            // if (context.Request.Method == "GET")
            // {
            //     _logger.LogInformation("{query}", context.Request.QueryString);
            // }
            //
            // string[] sendRequestsMethods = { "POST", "PUT", "DELETE" };
            // if (sendRequestsMethods.Contains(context.Request.Method))
            // {
            //     using var bodyReader = new StreamReader(context.Request.Body);
            //     var bodyS = await bodyReader.ReadToEndAsync();
            //     var body = System.Text.Json.JsonSerializer.Deserialize<object>(bodyS);
            //     _logger.LogInformation("{body}", body);
            // }

            await _next(context);
        }
        finally
        {
            _logger.LogInformation(
                "Request {method} {url} => {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode
            );
        }
    }
}