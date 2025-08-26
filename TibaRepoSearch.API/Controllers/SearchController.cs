using Microsoft.AspNetCore.Mvc;

namespace TibaRepoSearch;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IRepositorySearchUseCase _useCase;
    public SearchController(IRepositorySearchUseCase useCase)
    {
        _useCase = useCase;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repository>>> Get([FromQuery] string q)
    {
        if (string.IsNullOrEmpty(q))
            return BadRequest("Query parameter 'q' is required");

        var results = await _useCase.SearchAsync(q);
        return Ok(results);
    }
}
