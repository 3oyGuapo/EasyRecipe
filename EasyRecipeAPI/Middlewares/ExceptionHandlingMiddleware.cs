using System.Net;
using System.Text.Json;

namespace EasyRecipeAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Resource not found: {ex.Message}");

                await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, "Resource not found");
            }

            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Invalid argument: {ex.Message}");

                await HandleExceptionAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unhandled exception occured: {ex.Message}");

                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "Internal server error");
            }
        }


        private static async Task HandleExceptionAsync(HttpContext httpContext, HttpStatusCode httpStatusCode, string message)
        {
            httpContext.Response.StatusCode = (int)httpStatusCode;

            httpContext.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = httpContext.Response.StatusCode,
                message = message,
                timestamp = DateTime.UtcNow
            };

            await httpContext.Response.WriteAsJsonAsync(response);
        }

    }
}
