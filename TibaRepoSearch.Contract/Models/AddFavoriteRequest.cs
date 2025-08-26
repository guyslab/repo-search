namespace TibaRepoSearch;

public record AddFavoriteRequest(string RepoId, string Name, string Owner, int Stars, DateTime UpdatedAt);