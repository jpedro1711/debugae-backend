using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace GestaoDefeitos.WebApi.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _log;

        public ErrorMiddleware(RequestDelegate next, ILoggerFactory log)
        {
            _next = next;
            _log = log.CreateLogger("Error handler");
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception e)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { message = e.Message });
            await context.Response.WriteAsync(result);
        }
    }
}
