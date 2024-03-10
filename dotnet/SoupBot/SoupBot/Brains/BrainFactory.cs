using Microsoft.Extensions.DependencyInjection;
using SoupBot.Models;

namespace SoupBot.Brains;
/// <summary>
/// Creates brains for robots.
/// </summary>
public interface IBrainFactory
{
    /// <summary>
    /// Creates the specified brain for the specified robot.
    /// </summary>
    /// <typeparam name="T">The type of brain to create.</typeparam>
    /// <param name="robot">The robot controlled by this brain.</param>
    /// <returns>The brain.</returns>
    public T Create<T>(Robot robot) where T : IBrain;
}

public class BrainFactory : IBrainFactory
{
    private readonly IServiceProvider serviceProvider;

    public BrainFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public T Create<T>(Robot robot) where T : IBrain
    {
        var brain = serviceProvider.GetRequiredService<T>();
        brain.Robot = robot;
        return brain;
    }
}
