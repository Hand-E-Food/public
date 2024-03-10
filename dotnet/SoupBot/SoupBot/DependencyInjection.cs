using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IServiceProvider>(provider => provider);
        services.AddSingleton<Data.IAssetRepository, Data.AssetRepository>();
        services.AddScoped<Brains.IBrainFactory, Brains.BrainFactory>();
        services.AddScoped<Brains.IPlayerInput>(provider => provider.GetRequiredService<Brains.PlayerInput>());
        services.AddScoped<Brains.PlayerInput>();
        services.AddScoped<Brains.PlayerInputBrain>();
        services.AddScoped<Engine.IGameEngine, Engine.GameEngine>();
        services.AddScoped<Forms.MapForm>();
        services.AddScoped<Mapping.Map>(CreateMap);
        services.AddScoped<Mapping.IMapCellFactory, Mapping.MapCellFactory>();
        services.AddScoped<Mapping.IMapFactory, Mapping.MapFactory>();
        services.AddScoped<Models.Player>();
    }

    private static Mapping.Map CreateMap(IServiceProvider provider) =>
        provider.GetRequiredService<Mapping.IMapFactory>().CreateMap();
}
