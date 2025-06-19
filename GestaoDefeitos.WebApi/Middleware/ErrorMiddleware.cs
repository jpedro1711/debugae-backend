using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace GestaoDefeitos.WebApi.Middleware
{
    public class ErrorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger _log = loggerFactory.CreateLogger("Error handler");

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (e is InvalidOperationException)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var result = JsonSerializer.Serialize(new { message = e.Message });
            await context.Response.WriteAsync(result);
        }
    }
}
