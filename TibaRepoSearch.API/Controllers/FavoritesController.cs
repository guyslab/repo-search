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
        _logger.LogTrace("[{timestamp}] [FavoritesController..ctor] {addToFavoritesUseCase};{listUserFavoritesUseCase};{removeUserFavoriteUseCase};{requestContext} OK", DateTime.UtcNow.ToString("O"), addToFavoritesUseCase, listUserFavoritesUseCase, removeUserFavoriteUseCase, requestContext);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteRepository>>> Get()
    {
        try
        {
            var results = await _listUserFavoritesUseCase.ListAsync(_requestContext.GetUserId()!);
            _logger.LogTrace("[{timestamp}] [FavoritesController.Get]  OK", DateTime.UtcNow.ToString("O"));
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [FavoritesController.Get]  {Message}", DateTime.UtcNow.ToString("O"), ex.Message);
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
            _logger.LogTrace("[{timestamp}] [FavoritesController.Post] {request} OK", DateTime.UtcNow.ToString("O"), request);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [FavoritesController.Post] {request} {Message}", DateTime.UtcNow.ToString("O"), request, ex.Message);
            throw;
        }
    }

    [HttpDelete("{repoId}")]
    public async Task<ActionResult> Delete(string repoId)
    {
        try
        {
            await _removeUserFavoriteUseCase.RemoveAsync(repoId, _requestContext.GetUserId()!);
            _logger.LogTrace("[{timestamp}] [FavoritesController.Delete] {repoId} OK", DateTime.UtcNow.ToString("O"), repoId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[{timestamp}] [FavoritesController.Delete] {repoId} {Message}", DateTime.UtcNow.ToString("O"), repoId, ex.Message);
            throw;
        }
    }
}