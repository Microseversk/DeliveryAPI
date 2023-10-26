using System.Net;
using DeliveryApi.Exceptions;
using DeliveryApi.Models;

namespace DeliveryApi.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        int statusCode = StatusCodes.Status500InternalServerError;
        switch (ex)
        {
            case BadRequestException _:
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case NotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                break;
        }
        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = ex.Message
        };
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(errorResponse.ToString());
    }
}

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}