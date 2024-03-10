using Microsoft.Extensions.DependencyInjection;

namespace SpaceTraders
{
    public static class IServiceCollectionExtensions
    {
        public static void AddModule(this IServiceCollection services, IDependencyInjectionModule module)
        {
            if (services.Any(service => service.ServiceType == module.GetType()))
                return;

            services.Add(new ServiceDescriptor(module.GetType(), module));
            module.AddServices(services);
        }
    }
}
