namespace Lc.MicroService.ConwayGoL.Middlewares;

using Lc.MicroService.ConwayGoL.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Middleware to handle exceptions.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled error has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception and returns a JSON response.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="exception">Exception to handle.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorDto
        {
            StatusCode = context.Response.StatusCode,
            Message = "An internal server error has occurred.",
            Detail = context.RequestServices.GetService<IWebHostEnvironment>().IsDevelopment() ? exception.StackTrace : null
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

