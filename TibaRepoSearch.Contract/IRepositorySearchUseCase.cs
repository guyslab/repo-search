namespace TibaRepoSearch;

public interface IRepositorySearchUseCase
{
    Task<IEnumerable<Repository>> SearchAsync(string query, int page = 1, int pageSize = 10);
}