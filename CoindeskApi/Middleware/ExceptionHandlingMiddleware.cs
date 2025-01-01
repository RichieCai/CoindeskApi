using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);// 將響應資料寫回原始資料流  
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); ;
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //清空響應資料流，避免之前的響應內容干擾
            context.Response.Clear();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorResponse = new
            {
                statusCode = context.Response.StatusCode,
                message = "An unexpected error occurred.",
                details = exception.Message,
                // details = environment == "Development" ? exception.Message : null,
                timestamp = DateTime.UtcNow
            };
            //return null;
            return context.Response.WriteAsJsonAsync(errorResponse);
        }

    }
}
