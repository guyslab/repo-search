using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TibaRepoSearch;

[Table("favorite_repositories")]
public class FavoriteRepositoryData : IFavoriteRepositoryData
{
    [Key]
    public Guid Id { get; set; }
    
    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;
    
    [Column("repo_id")]
    public string RepoId { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Owner { get; set; } = string.Empty;
    
    public int Stars { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}