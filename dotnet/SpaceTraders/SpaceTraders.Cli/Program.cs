using Microsoft.Extensions.DependencyInjection;
using SpaceTraders;

Main().Wait();

async Task Main()
{
    var serviceProvider = GetServiceProvider();
    var globalManager = serviceProvider.GetRequiredService<GlobalManager>();
    await globalManager.Initialise();
    //using var agentScope = globalManager.CreateAgentScope("HANDEFOOD");

}

IServiceProvider GetServiceProvider()
{
    var services = new ServiceCollection();
    services.AddModule(new SpaceTraders.Cli.DependencyInjectionModule());
    return services.BuildServiceProvider();
}
