using Microsoft.EntityFrameworkCore;

namespace TibaRepoSearch;

public class FavoriteRepositoriesContext : DbContext
{
    public FavoriteRepositoriesContext(DbContextOptions<FavoriteRepositoriesContext> options) : base(options) { }

    public DbSet<FavoriteRepositoryData> FavoriteRepositories { get; set; }
    public DbSet<FavoriteRepositoryAnalysisData> FavoriteRepositoryAnalysis { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteRepositoryData>()
            .HasIndex(f => new { f.RepoId, f.UserId })
            .IsUnique();
        
        modelBuilder.Entity<FavoriteRepositoryData>()
            .HasIndex(f => f.UserId);
            
        modelBuilder.Entity<FavoriteRepositoryData>()
            .HasOne(f => f.Analysis)
            .WithOne()
            .HasForeignKey<FavoriteRepositoryAnalysisData>(a => a.FavoriteId)
            .IsRequired(false);
    }
}