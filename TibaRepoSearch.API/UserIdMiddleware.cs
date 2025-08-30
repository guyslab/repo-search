using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserIdMiddleware> _logger;

    public UserIdMiddleware(RequestDelegate next, ILogger<UserIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [UserIdMiddleware..ctor] {next} OK", DateTime.UtcNow.ToString("O"), next);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            context.Items["UserId"] = "user123";
            await _next(context);
            _logger.LogTrace("[{timestamp}] [UserIdMiddleware.InvokeAsync] {context} OK", DateTime.UtcNow.ToString("O"), context);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [UserIdMiddleware.InvokeAsync] {context} {Message}", DateTime.UtcNow.ToString("O"), context, ex.Message);
            throw;
        }
    }
}