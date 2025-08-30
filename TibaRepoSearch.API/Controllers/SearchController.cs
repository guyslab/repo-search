using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

[ApiController]
[Route("api/[controller]")]
[RequireUserId]
public class SearchController : ControllerBase
{
    private readonly IRepositorySearchUseCase _useCase;
    private readonly ILogger<SearchController> _logger;
    public SearchController(IRepositorySearchUseCase useCase, ILogger<SearchController> logger)
    {
        _useCase = useCase;
        _logger = logger;
        _logger.LogTrace("[SearchController..ctor] {useCase} OK", useCase);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repository>>> Get([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(q))
                return BadRequest("Query parameter 'q' is required");

            var results = await _useCase.SearchAsync(q, page, pageSize);
            _logger.LogTrace("[SearchController.Get] {q};{page};{pageSize} OK", q, page, pageSize);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[SearchController.Get] {q};{page};{pageSize} {Message}", q, page, pageSize, ex.Message);
            throw;
        }
    }
}
