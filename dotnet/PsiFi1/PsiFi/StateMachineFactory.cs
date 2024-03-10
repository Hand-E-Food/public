using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace PsiFi
{
    /// <summary>
    /// Creates and arranges the state machine nodes.
    /// </summary>
    static class StateMachineFactory
    {
        /// <summary>
        /// The interface type required by all state machine nodes.
        /// </summary>
        private static readonly Type nodeInterface = typeof(IStateMachineNode);

        /// <summary>
        /// All types used by the factory.
        /// </summary>
        private static readonly Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && nodeInterface.IsAssignableFrom(type))
            .ToArray();

        /// <summary>
        /// Creates the state machine.
        /// </summary>
        /// <returns>The first state machine node to execute.</returns>
        public static IStateMachineNode GetStateMachine(IServiceProvider serviceProvider)
        {
            var nodes = types
                .Select(serviceProvider.GetService)
                .Where(service => service != null)
                .Cast<IStateMachineNode>()
                .ToList();

            foreach (var node in nodes)
            {
                var fields = node.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(field => nodeInterface.IsAssignableFrom(field.FieldType));

                foreach (var field in fields)
                    field.SetValue(node, serviceProvider.GetService(field.FieldType));
            }

            return nodes.Single(node => node.GetType().GetCustomAttribute<InitialStateMachineNodeAttribute>() != null);
        }

        /// <summary>
        /// Registers all state machine node types with the <see cref="ServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to register with.</param>
        public static void RegisterServices(ServiceCollection services)
        {
            foreach (var nodeType in types)
                services.AddScoped(nodeType);
        }
    }
}
