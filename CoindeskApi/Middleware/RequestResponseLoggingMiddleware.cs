
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
            // 替换 Response.Body 为 MemoryStream
            var originalResponseBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            try
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    await LogRequestAsync(context);
                }
                //await LogRequestAsync(context);
                await _next(context);
                await LogResponseAsync(context);
            }
            finally
            {
                // 將響應資料寫回原始資料流  
                await responseBody.CopyToAsync(originalResponseBody);
                // 確保恢復原始流
                context.Response.Body = originalResponseBody;
            }
        }



        private async Task LogRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering(); // 允許重複讀取請求

            var request = context.Request;
            var requestBody = string.Empty;

            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0; // 重置流位置
            }

            var result = "Request:\n" +
                                     $"Method: {request.Method}\n" +
                                     $"Path: {request.Path}\n" +
                                     $"Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {h.Value}"))}\n" +
                                     $"Body: {requestBody}";
            _logger.LogInformation(result);
            Console.WriteLine(result);

        }

        private async Task LogResponseAsync(HttpContext context)
        {
            // 確保 Response.Body 是 MemoryStream
            if (context.Response.Body is MemoryStream responseBody)
            {
                // 讀取響應内容
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin); // 重置流位置以便寫回

                var result = "Response:\n" +
                                       $"StatusCode: {context.Response.StatusCode}\n" +
                                       $"Headers: {string.Join(", ", context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"))}\n" +
                                       $"Body: {responseText}";
                _logger.LogInformation(result);
                Console.WriteLine(result);
            }
            else
            {
                _logger.LogWarning("Unable to log response: Response.Body is not a MemoryStream.");
            }
        }
    }
}
