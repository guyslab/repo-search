namespace TibaRepoSearch;

public interface IListFavoriteRepositoryWithAnalysisCommand
{
    Task<IEnumerable<IFavoriteRepositoryData>> ExecuteAsync();
}