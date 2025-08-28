using Microsoft.AspNetCore.Mvc;
using TibaRepoSearch.ApiManagement;

namespace TibaRepoSearch;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IAddToFavoritesUseCase _addToFavoritesUseCase;
    private readonly IListUserFavoritesUseCase _listUserFavoritesUseCase;
    private readonly IRemoveUserFavoriteUseCase _removeUserFavoriteUseCase;
    private readonly IRequestContext _requestContext;

    public FavoritesController(IAddToFavoritesUseCase addToFavoritesUseCase, IListUserFavoritesUseCase listUserFavoritesUseCase, IRemoveUserFavoriteUseCase removeUserFavoriteUseCase, IRequestContext requestContext)
    {
        _addToFavoritesUseCase = addToFavoritesUseCase;
        _listUserFavoritesUseCase = listUserFavoritesUseCase;
        _removeUserFavoriteUseCase = removeUserFavoriteUseCase;
        _requestContext = requestContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteRepository>>> Get()
    {
        var results = await _listUserFavoritesUseCase.ListAsync(_requestContext.GetUserId());
        return Ok(results);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AddFavoriteRequest request)
    {
        if (string.IsNullOrEmpty(request.RepoId))
            return BadRequest("Query parameter 'RepoId' is required");
        var favorite = new FavoriteRepository(request.Name, request.Owner, request.Stars, request.UpdatedAt, "", request.RepoId);

        await _addToFavoritesUseCase.AddAsync(request, _requestContext.GetUserId());
        return Accepted();
    }

    [HttpDelete("{repoId}")]
    public async Task<ActionResult> Delete(string repoId)
    {
        await _removeUserFavoriteUseCase.RemoveAsync(repoId, _requestContext.GetUserId());
        return NoContent();
    }
}