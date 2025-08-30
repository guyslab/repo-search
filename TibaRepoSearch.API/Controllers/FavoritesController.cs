using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

[ApiController]
[Route("api/[controller]")]
[RequireUserId]
public class FavoritesController : ControllerBase
{
    private readonly IAddToFavoritesUseCase _addToFavoritesUseCase;
    private readonly IListUserFavoritesUseCase _listUserFavoritesUseCase;
    private readonly IRemoveUserFavoriteUseCase _removeUserFavoriteUseCase;
    private readonly IRequestContext _requestContext;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(IAddToFavoritesUseCase addToFavoritesUseCase, IListUserFavoritesUseCase listUserFavoritesUseCase, IRemoveUserFavoriteUseCase removeUserFavoriteUseCase, IRequestContext requestContext, ILogger<FavoritesController> logger)
    {
        _addToFavoritesUseCase = addToFavoritesUseCase;
        _listUserFavoritesUseCase = listUserFavoritesUseCase;
        _removeUserFavoriteUseCase = removeUserFavoriteUseCase;
        _requestContext = requestContext;
        _logger = logger;
        _logger.LogTrace("[FavoritesController..ctor] {addToFavoritesUseCase};{listUserFavoritesUseCase};{removeUserFavoriteUseCase};{requestContext} OK", addToFavoritesUseCase, listUserFavoritesUseCase, removeUserFavoriteUseCase, requestContext);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteRepository>>> Get()
    {
        try
        {
            var results = await _listUserFavoritesUseCase.ListAsync(_requestContext.GetUserId()!);
            _logger.LogTrace("[FavoritesController.Get]  OK");
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[FavoritesController.Get]  {Message}", ex.Message);
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AddFavoriteRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.RepoId))
                return BadRequest("Query parameter 'RepoId' is required");
            var favorite = new FavoriteRepository(request.Name, request.Owner, request.Stars, request.UpdatedAt, "", request.RepoId);

            await _addToFavoritesUseCase.AddAsync(request, _requestContext.GetUserId()!);
            _logger.LogTrace("[FavoritesController.Post] {request} OK", request);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[FavoritesController.Post] {request} {Message}", request, ex.Message);
            throw;
        }
    }

    [HttpDelete("{repoId}")]
    public async Task<ActionResult> Delete(string repoId)
    {
        try
        {
            await _removeUserFavoriteUseCase.RemoveAsync(repoId, _requestContext.GetUserId()!);
            _logger.LogTrace("[FavoritesController.Delete] {repoId} OK", repoId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[FavoritesController.Delete] {repoId} {Message}", repoId, ex.Message);
            throw;
        }
    }
}