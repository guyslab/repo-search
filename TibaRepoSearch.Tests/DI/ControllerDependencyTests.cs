using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace TibaRepoSearch.Tests.DI;

public class ControllerDependencyTests
{
    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:Host"] = "localhost",
                ["Database:Name"] = "test",
                ["Database:User"] = "test",
                ["Database:Password"] = "test",
                ["Redis:ConnectionString"] = "localhost:6379"
            })
            .Build();

        services.AddApplicationLayer(configuration);
        services.AddInfrastructureLayer(configuration);
        services.AddHttpContextAccessor();

        return services.BuildServiceProvider();
    }

    [Fact]
    public void SearchController_CanResolveDependencies()
    {
        using var serviceProvider = CreateServiceProvider();
        
        var useCase = serviceProvider.GetRequiredService<IRepositorySearchUseCase>();
        var controller = new SearchController(useCase);
        
        Assert.NotNull(controller);
    }

    [Fact]
    public void FavoritesController_CanResolveDependencies()
    {
        using var serviceProvider = CreateServiceProvider();
        
        var addToFavoritesUseCase = serviceProvider.GetRequiredService<IAddToFavoritesUseCase>();
        var listUserFavoritesUseCase = serviceProvider.GetRequiredService<IListUserFavoritesUseCase>();
        var removeUserFavoriteUseCase = serviceProvider.GetRequiredService<IRemoveUserFavoriteUseCase>();
        var requestContext = serviceProvider.GetRequiredService<IRequestContext>();
        
        var controller = new FavoritesController(
            addToFavoritesUseCase,
            listUserFavoritesUseCase,
            removeUserFavoriteUseCase,
            requestContext);
        
        Assert.NotNull(controller);
    }
}