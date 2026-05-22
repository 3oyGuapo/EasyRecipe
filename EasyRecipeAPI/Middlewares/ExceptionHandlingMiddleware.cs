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

                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    statusCode = 404,
                    message = "Resource not found"
                });
            }

            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Invalid argument: {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    statusCode = 400,
                    message = "Bad request"
                });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unhandled exception occured: {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    statusCode = 500,
                    message = "Internal server error"
                });
            }
        }

    }
}
