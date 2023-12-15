using Microsoft.Extensions.DependencyInjection;
using SoupBot.Data;
using SoupBot.Forms;
using SoupBot.Generation;
using SoupBot.Models;

namespace SoupBot;

public static class DependencyInjection
{
    public static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        RegisterServices(services);
        return services.BuildServiceProvider();
    }

    public static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IAssetRepository, AssetRepository>();
        services.AddScoped<Map>(CreateMap);
        services.AddScoped<IMapCellGenerator, MapCellGenerator>();
        services.AddScoped<IMapGenerator, MapGenerator>();
        services.AddScoped<MapForm>();
    }

    private static Map CreateMap(IServiceProvider provider) =>
        provider.GetRequiredService<IMapGenerator>().GenerateMap();
}
