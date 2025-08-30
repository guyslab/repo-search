using Xunit;

namespace TibaRepoSearch.Tests.UseCases;

public class FetchRepositoryAnalysisUseCaseTests
{
    [Theory]
    [InlineData(1000, 10, 5, 20, 94.18)] // High stars, recent activity, good ratio
    [InlineData(0, 365, 0, 0, 20)] // No stars, old activity, no issues
    [InlineData(2000, 0, 10, 10, 80)] // Max stars, very recent, equal issues/forks
    public void CalculateHealthScore_ReturnsExpectedScore(int stars, int activityDays, int openIssues, int forks, decimal expected)
    {
        var uut = new FetchRepositoryAnalysisUseCase(null, null);
        var result = uut.CalculateHealthScore(stars, activityDays, openIssues, forks);

        Assert.Equal(expected, result, 1);
    }

    [Fact]
    public void CalculateHealthScore_ReturnsMaximum100()
    {
        var uut = new FetchRepositoryAnalysisUseCase(null, null);
        var result = uut.CalculateHealthScore(10000, 0, 0, 1000);

        Assert.True(result <= 100);
    }

    [Fact]
    public void CalculateHealthScore_ReturnsMinimum0()
    {
        var uut = new FetchRepositoryAnalysisUseCase(null, null);
        var result = uut.CalculateHealthScore(0, 1000, 1000, 0);

        Assert.True(result >= 0);
    }
}