using Microsoft.Extensions.DependencyInjection;

namespace SpaceTraders
{
    public interface IDependencyInjectionModule
    {
        void AddServices(IServiceCollection services);
    }
}
