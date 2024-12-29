
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CoindeskApi.Middleware
{
    public class RequestResponseLoggingMiddleware : Controller
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }
            // 只處理 API 請求（排除靜態資源和 Swagger）
            //if (context.Request.Path.StartsWithSegments("/swagger") ||
            //    context.Request.Path.StartsWithSegments("/favicon.ico"))
            //{
            //    await _next(context);
            //    return;
            //}


            // 記錄請求
            var request = context.Request;
            request.EnableBuffering();

            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            _logger.LogInformation($"[API Request] Method: {request.Method}, Path: {request.Path}, Body: {requestBody}");

            // 攔截回應
            var originalResponseBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"[API Response] StatusCode: {context.Response.StatusCode}, Body: {responseBody}");

            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }
}
