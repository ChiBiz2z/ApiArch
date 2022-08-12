using System.Diagnostics;
using System.Text.Json;
using EmployeesAPI.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesAPI.Errors;

public static class ErrorHandlerExtensions
{
    public static void CustomErrors(this IApplicationBuilder? app, IHostEnvironment environment)
    {
        if (app != null && environment.IsDevelopment())
        {
            app.Use(WriteDevelopmentResponse);
        }
        else
        {
            app?.Use(WriteProductionResponse);
        }
    }

    private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: true);

    private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: false);

    private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
    {
        var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
        var ex = exceptionDetails?.Error;
        
        var problemCode = ex switch
        {
            DomainException => 400,
            _ => 500
        };
        
        if (ex != null)
        {
            httpContext.Response.ContentType = "application/problem+json";

            var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = includeDetails ? ex.ToString() : null;

            var problem = new ProblemDetails
            {
                Status = problemCode,
                Title = title,
                Detail = details
            };

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problem.Extensions["traceId"] = traceId;
            }

            var stream = httpContext.Response.Body;
            await JsonSerializer.SerializeAsync(stream, problem);
        }
    }
}