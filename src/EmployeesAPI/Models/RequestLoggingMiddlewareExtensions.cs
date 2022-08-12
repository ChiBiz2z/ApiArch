namespace EmployeesAPI.Models;

public static class RequestLoggingMiddlewareExtensions
{
    public static void UseMiddlewareLogging(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}