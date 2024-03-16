using System.Net;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ObjectResult response = HandleException(ex);

            context.Response.StatusCode = response.StatusCode!.Value;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response.Value!.ToString()!);
        }
    }

    private ObjectResult HandleException(Exception ex)
    {
        if (ex is HttpRequestException httpEx && httpEx.StatusCode.HasValue)
        {
            Log.Error("Error occurred: {Message}", ex.Message);

            return httpEx.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundObjectResult(ex.Message),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult("Unauthorized or invalid apiKey"),
                _ => new ObjectResult("Unexpected error occurred")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                },
            };
        }

        return new ObjectResult("Unexpected error occurred")
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
