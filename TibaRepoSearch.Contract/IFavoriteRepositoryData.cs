namespace TibaRepoSearch;

public interface IFavoriteRepositoryData
{
    Guid Id { get; set; }
    string UserId { get; set; }
    string RepoId { get; set; }
    string Name { get; set; }
    string Owner { get; set; }
    int Stars { get; set; }
    DateTime UpdatedAt { get; set; }
    DateTime CreatedAt { get; set; }
    IFavoriteRepositoryAnalysisData? Analysis { get; set; }
}