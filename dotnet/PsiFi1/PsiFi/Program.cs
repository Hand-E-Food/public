using Microsoft.Extensions.DependencyInjection;
using PsiFi.Models;
using PsiFi.View;

namespace PsiFi
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var services = RegisterServices())
            {
                var state = new State();
                var stateMachine = StateMachineFactory.GetStateMachine(services);
                while (stateMachine != null)
                    stateMachine = stateMachine.Execute(state);
            }
        }

        static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<KeyboardInput>();
            services.AddSingleton<MapView>();
            services.AddSingleton<Random>();
            
            StateMachineFactory.RegisterServices(services);
            return services.BuildServiceProvider();
        }
    }
}
