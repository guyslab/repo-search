using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RequestContext> _logger;

    public RequestContext(IHttpContextAccessor httpContextAccessor, ILogger<RequestContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _logger.LogTrace("[{timestamp}] [RequestContext..ctor] {httpContextAccessor} OK", DateTime.UtcNow.ToString("O"), httpContextAccessor);
    }

    public string? GetUserId()
    {
        try
        {
            var result = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
            _logger.LogTrace("[{timestamp}] [RequestContext.GetUserId]  OK", DateTime.UtcNow.ToString("O"));
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [RequestContext.GetUserId]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
            throw;
        }
    }
}