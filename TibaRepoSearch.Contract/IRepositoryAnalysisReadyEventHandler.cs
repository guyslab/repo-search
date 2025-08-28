namespace TibaRepoSearch;

public interface IRepositoryAnalysisReadyEventHandler
{
    Task Handle(RepositoryAnalysisReadyMessage message);
}