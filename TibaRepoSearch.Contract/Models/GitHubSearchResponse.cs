using System.Text.Json.Serialization;

namespace TibaRepoSearch;

public record GitHubSearchResponse(
    [property: JsonPropertyName("total_count")] int TotalCount,
    [property: JsonPropertyName("incomplete_results")] bool IncompleteResults,
    [property: JsonPropertyName("items")] GitHubRepository[] Items);

public record GitHubRepository(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("node_id")] string NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("full_name")] string FullName,
    [property: JsonPropertyName("owner")] GitHubOwner Owner,
    [property: JsonPropertyName("private")] bool Private,
    [property: JsonPropertyName("html_url")] string HtmlUrl,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("fork")] bool Fork,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTime UpdatedAt,
    [property: JsonPropertyName("pushed_at")] DateTime PushedAt,
    [property: JsonPropertyName("git_url")] string GitUrl,
    [property: JsonPropertyName("ssh_url")] string SshUrl,
    [property: JsonPropertyName("clone_url")] string CloneUrl,
    [property: JsonPropertyName("svn_url")] string SvnUrl,
    [property: JsonPropertyName("homepage")] string Homepage,
    [property: JsonPropertyName("size")] int Size,
    [property: JsonPropertyName("stargazers_count")] int StargazersCount,
    [property: JsonPropertyName("watchers_count")] int WatchersCount,
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("has_issues")] bool HasIssues,
    [property: JsonPropertyName("has_projects")] bool HasProjects,
    [property: JsonPropertyName("has_wiki")] bool HasWiki,
    [property: JsonPropertyName("has_pages")] bool HasPages,
    [property: JsonPropertyName("has_downloads")] bool HasDownloads,
    [property: JsonPropertyName("archived")] bool Archived,
    [property: JsonPropertyName("disabled")] bool Disabled,
    [property: JsonPropertyName("open_issues_count")] int OpenIssuesCount,
    [property: JsonPropertyName("license")] GitHubLicense License,
    [property: JsonPropertyName("allow_forking")] bool AllowForking,
    [property: JsonPropertyName("is_template")] bool IsTemplate,
    [property: JsonPropertyName("topics")] string[] Topics,
    [property: JsonPropertyName("visibility")] string Visibility,
    [property: JsonPropertyName("forks")] int Forks,
    [property: JsonPropertyName("open_issues")] int OpenIssues,
    [property: JsonPropertyName("watchers")] int Watchers,
    [property: JsonPropertyName("default_branch")] string DefaultBranch,
    [property: JsonPropertyName("score")] double Score);

public record GitHubOwner(
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("node_id")] string NodeId,
    [property: JsonPropertyName("avatar_url")] string AvatarUrl,
    [property: JsonPropertyName("gravatar_id")] string GravatarId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("html_url")] string HtmlUrl,
    [property: JsonPropertyName("followers_url")] string FollowersUrl,
    [property: JsonPropertyName("following_url")] string FollowingUrl,
    [property: JsonPropertyName("gists_url")] string GistsUrl,
    [property: JsonPropertyName("starred_url")] string StarredUrl,
    [property: JsonPropertyName("subscriptions_url")] string SubscriptionsUrl,
    [property: JsonPropertyName("organizations_url")] string OrganizationsUrl,
    [property: JsonPropertyName("repos_url")] string ReposUrl,
    [property: JsonPropertyName("events_url")] string EventsUrl,
    [property: JsonPropertyName("received_events_url")] string ReceivedEventsUrl,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("site_admin")] bool SiteAdmin);

public record GitHubLicense(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("spdx_id")] string SpdxId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("node_id")] string NodeId);