using FluentValidation;
using HR.LeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(
    ILogger<GlobalExceptionHandlingMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        ProblemDetails problem;

        switch (exception)
        {
            case NotFoundException ex:
                logger.LogWarning(ex, "Resource not found");
                response.StatusCode = StatusCodes.Status404NotFound;
                problem = CreateProblem("Resource Not Found", response.StatusCode, ex.Message, context);
                break;

            case ForbiddenAccessException ex:
                logger.LogWarning(ex, "Forbidden access");
                response.StatusCode = StatusCodes.Status403Forbidden;
                problem = CreateProblem("Forbidden", response.StatusCode, ex.Message, context);
                break;

            case BadRequestException ex:
                logger.LogWarning(ex, "Bad request");
                response.StatusCode = StatusCodes.Status400BadRequest;
                problem = CreateProblem("Bad Request", response.StatusCode, ex.Message, context);
                break;

            case ValidationException ex:
                logger.LogWarning(ex, "Validation failed");
                response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var validationProblem = new ValidationProblemDetails(errors)
                {
                    Title = "Validation Failed",
                    Status = response.StatusCode,
                    Instance = context.Request.Path
                };

                await response.WriteAsJsonAsync(validationProblem);
                return;

            default:
                logger.LogError(exception, "Unhandled exception");
                response.StatusCode = StatusCodes.Status500InternalServerError;
                problem = CreateProblem(
                    "Internal Server Error",
                    response.StatusCode,
                    "An unexpected error occurred.",
                    context);
                break;
        }

        await response.WriteAsJsonAsync(problem);
    }

    private static ProblemDetails CreateProblem(
        string title,
        int statusCode,
        string detail,
        HttpContext context)
    {
        return new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };
    }
}