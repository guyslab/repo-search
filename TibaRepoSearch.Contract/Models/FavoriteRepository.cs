namespace TibaRepoSearch;

public record FavoriteRepository(string Name, string Owner, int Stars, DateTime UpdatedAt, string Description, string RepoId, Analysis? Analysis = null)
    : Repository(Name, Owner, Stars, UpdatedAt, Description, RepoId);