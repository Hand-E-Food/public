using Microsoft.Extensions.DependencyInjection;
using SpaceTraders.Api;

namespace SpaceTraders
{
    public class DependencyInjectionModule : IDependencyInjectionModule
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<AgentRepository>();
            services.AddScoped<ApiClient>();
            services.AddSingleton<GlobalManager>();
            services.AddScoped<HttpClient>();
            services.AddSingleton<HttpMessageHandler, SpaceTradersHttpMessageHandler>();
            services.AddSingleton<Paginator>();
            services.AddScoped(IServiceProvider);
        }

        private IServiceProvider IServiceProvider(IServiceProvider provider) => provider;
    }
}
