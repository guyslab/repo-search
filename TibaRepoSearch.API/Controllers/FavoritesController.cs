using Microsoft.AspNetCore.Mvc;

namespace TibaRepoSearch;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IAddToFavoritesUseCase _addToFavoritesUseCase;
    private readonly IListUserFavoritesUseCase _listUserFavoritesUseCase;
    private readonly IRemoveUserFavoriteUseCase _removeUserFavoriteUseCase;

    public FavoritesController(IAddToFavoritesUseCase addToFavoritesUseCase, IListUserFavoritesUseCase listUserFavoritesUseCase, IRemoveUserFavoriteUseCase removeUserFavoriteUseCase)
    {
        _addToFavoritesUseCase = addToFavoritesUseCase;
        _listUserFavoritesUseCase = listUserFavoritesUseCase;
        _removeUserFavoriteUseCase = removeUserFavoriteUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteRepository>>> Get()
    {
        var results = await _listUserFavoritesUseCase.ListAsync("dummy-user");
        return Ok(results);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AddFavoriteRequest request)
    {
        if (string.IsNullOrEmpty(request.RepoId))
            return BadRequest("Query parameter 'RepoId' is required");
        var favorite = new FavoriteRepository(request.Name, request.Owner, request.Stars, request.UpdatedAt, "", request.RepoId);

        await _addToFavoritesUseCase.AddAsync(request, "dummy-user");
        return Accepted();
    }

    [HttpDelete("{repoId}")]
    public async Task<ActionResult> Delete(string repoId)
    {
        await _removeUserFavoriteUseCase.RemoveAsync(repoId, "dummy-user");
        return NoContent();
    }
}