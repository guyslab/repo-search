using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TibaRepoSearch;

[Table("favorite_repository_analysis")]
public class FavoriteRepositoryAnalysisData : IFavoriteRepositoryAnalysisData
{
    [Key]
    public Guid Id { get; set; }
    
    [Column("favorite_id")]
    public Guid FavoriteId { get; set; }
    
    public string? License { get; set; }
    
    [Column("topics_json")]
    public string? TopicsJson { get; set; }
    
    [Column("primary_language")]
    public string? PrimaryLanguage { get; set; }
    
    [Column("readme_length")]
    public int? ReadmeLength { get; set; }
    
    [Column("open_issues")]
    public int? OpenIssues { get; set; }
    
    public int? Forks { get; set; }
    
    [Column("stars_snapshot")]
    public int? StarsSnapshot { get; set; }
    
    [Column("activity_days")]
    public int? ActivityDays { get; set; }
    
    [Column("default_branch")]
    public string? DefaultBranch { get; set; }
    
    [Column("health_score")]
    public decimal? HealthScore { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}