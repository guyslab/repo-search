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
        _logger.LogTrace("[RequestContext..ctor] {httpContextAccessor} OK", httpContextAccessor);
    }

    public string? GetUserId()
    {
        try
        {
            var result = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
            _logger.LogTrace("[RequestContext.GetUserId]  OK");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[RequestContext.GetUserId]  {Message}", ex.Message);
            throw;
        }
    }
}