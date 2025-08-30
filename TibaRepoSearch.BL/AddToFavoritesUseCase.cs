using Microsoft.Extensions.Logging;

namespace TibaRepoSearch;

public class AddToFavoritesUseCase : IAddToFavoritesUseCase
{
    private readonly IAddOrUpdateFavoriteRepositoryCommandFactory _commandFactory;
    private readonly ILogger<AddToFavoritesUseCase> _logger;

    public AddToFavoritesUseCase(IAddOrUpdateFavoriteRepositoryCommandFactory commandFactory, ILogger<AddToFavoritesUseCase> logger)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _logger.LogTrace("[AddToFavoritesUseCase..ctor] {commandFactory} OK", commandFactory);
    }

    public async Task AddAsync(AddFavoriteRequest request, string userId)
    {
        try
        {
            var repository = new Repository(request.Name, request.Owner, request.Stars, request.UpdatedAt, string.Empty, request.RepoId);
            var command = _commandFactory.Create(userId, repository);
            await command.ExecuteAsync();
            _logger.LogTrace("[AddToFavoritesUseCase.AddAsync] {request};{userId} OK", request, userId);
        }
        catch (Exception ex)
        {
            _logger.LogTrace("[AddToFavoritesUseCase.AddAsync] {request};{userId} {Message}", request, userId, ex.Message);
            throw;
        }
    }
}