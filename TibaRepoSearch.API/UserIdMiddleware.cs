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
        _logger.LogTrace("[UserIdMiddleware..ctor] {next} OK", next);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            context.Items["UserId"] = "user123";
            await _next(context);
            _logger.LogTrace("[UserIdMiddleware.InvokeAsync] {context} OK", context);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[UserIdMiddleware.InvokeAsync] {context} {Message}", context, ex.Message);
            throw;
        }
    }
}