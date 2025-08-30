using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TibaRepoSearch.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "favorite_repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    repo_id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Owner = table.Column<string>(type: "text", nullable: false),
                    Stars = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_repositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "favorite_repository_analysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    favorite_id = table.Column<Guid>(type: "uuid", nullable: false),
                    License = table.Column<string>(type: "text", nullable: true),
                    topics_json = table.Column<string>(type: "text", nullable: true),
                    primary_language = table.Column<string>(type: "text", nullable: true),
                    readme_length = table.Column<int>(type: "integer", nullable: true),
                    open_issues = table.Column<int>(type: "integer", nullable: true),
                    Forks = table.Column<int>(type: "integer", nullable: true),
                    stars_snapshot = table.Column<int>(type: "integer", nullable: true),
                    activity_days = table.Column<int>(type: "integer", nullable: true),
                    default_branch = table.Column<string>(type: "text", nullable: true),
                    health_score = table.Column<decimal>(type: "numeric", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_repository_analysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_favorite_repository_analysis_favorite_repositories_favorite~",
                        column: x => x.favorite_id,
                        principalTable: "favorite_repositories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorite_repositories_repo_id_user_id",
                table: "favorite_repositories",
                columns: new[] { "repo_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_favorite_repositories_user_id",
                table: "favorite_repositories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_repository_analysis_favorite_id",
                table: "favorite_repository_analysis",
                column: "favorite_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorite_repository_analysis");

            migrationBuilder.DropTable(
                name: "favorite_repositories");
        }
    }
}
