using System.Net;
using System.Text.Json;
using ChatApp.DTO;

namespace ChatApp.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = _env.IsDevelopment()
                ? new ApiException((int) HttpStatusCode.InternalServerError, e.Message,
                    details: e.StackTrace)
                : new ApiException((int) HttpStatusCode.InternalServerError);


            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(response, options);


            await context.Response.WriteAsync(json);
        }
    }
}