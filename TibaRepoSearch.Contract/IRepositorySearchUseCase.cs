namespace TibaRepoSearch;

public interface IRepositorySearchUseCase
{
    Task<IEnumerable<Repository>> SearchAsync(string query);
}