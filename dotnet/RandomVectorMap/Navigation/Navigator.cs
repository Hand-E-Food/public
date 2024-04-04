using RandomVectorMap.Mapping;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Navigation;

/// <summary>
/// Performs best-path analysis on a map of roads.
/// </summary>
/// <typeparam name="T">The type of route information to store for each candidate.</typeparam>
public abstract partial class Navigator<T> where T : Route
{
    /// <summary>
    /// Gets the best route found so far.
    /// </summary>
    public T? BestRoute { get; protected set; } = null;

    /// <summary>
    /// The list of candidates.
    /// </summary>
    protected SortedSet<T> Candidates { get; private set; } = [];

    /// <summary>
    /// Gets the dictionary of the best route found to reach any junction.
    /// </summary>
    protected Dictionary<Junction, T> History { get; private set; } = [];

    /// <summary>
    /// Gets the collection of road qualities to allow in routes.
    /// </summary>
    /// <value>A collection of road qualities.</value>
    public List<RoadQuality> RoadQualities { get; protected set; } = [];

    /// <summary>
    /// Adds a route to the candidate list.
    /// </summary>
    /// <param name="route">The route to add.</param>
    protected void AddCandidate(T route)
    {
        // Filter out unsuitable routes.
        if (!IsViable(route)) return;
        // Add the route to the candidates list.
        Candidates.Add(route);
    }

    /// <summary>
    /// Adds a route to the history.
    /// </summary>
    /// <param name="route">The route to add.</param>
    /// <returns>True if the route was added.  False if a better route already exists in the history.</returns>
    private bool AddHistory(T route)
    {
        var junction = route.Junctions.Last();
        if (History.TryGetValue(junction, out var historicalRoute))
        {
            if (route.Score < historicalRoute.Score)
            {
                UpdateRoute(historicalRoute, historicalRoute, route);
                historicalRoute.Parent = route.Parent;
            }
            return false;
        }
        else
        {
            History.Add(junction, route);
            return true;
        }
    }

    /// <summary>
    /// Extends a route in all directions.
    /// </summary>
    /// <param name="route">The route to extend.</param>
    private void ExtendRoute(T route)
    {
        // Extend the candidate along each road of allowed quality that doesn't cause the route to form a loop.
        var junction = route.Junctions.Last();
        var roads = junction.Roads.Where(r => RoadQualities.Contains(r.Quality) && !route.Junctions.Contains(r.Other(junction)));
        foreach (var road in roads)
        {
            AddCandidate(ExtendRoute(route, road));
        }
    }

    /// <summary>
    /// Extends a route along a specified road.
    /// </summary>
    /// <param name="route">The route to extend.</param>
    /// <param name="road">The road used to extend the route.</param>
    /// <returns>The extended route.</returns>
    protected abstract T ExtendRoute(T route, Road road);

    /// <summary>
    /// Gets the next viable candidate route.
    /// </summary>
    /// <param name="state">Returns the next viable candidate route.</param>
    /// <returns>True if a viable candidate route was found; otherwise, false.</returns>
    private bool GetNextCandidate([NotNullWhen(true)] out T? route)
    {
        while (Candidates.Count > 0)
        {
            route = Candidates.First();
            Candidates.Remove(route);
            if (IsViable(route) && AddHistory(route))
                return true;
        }
        route = null;
        return false;
    }

    /// <summary>
    /// Checks whether a route is a viable solution.
    /// </summary>
    /// <param name="route">The route to test.</param>
    /// <returns>True if the route is a viable solution; otherwise, false.</returns>
    protected abstract bool IsSolution(T route);

    /// <summary>
    /// Determines whether the route is too long to be considered as a candidate.
    /// </summary>
    /// <param name="route">The route to test.</param>
    /// <returns>True if the route is too long to be considered as a candidate; otherwise, false.</returns>
    protected virtual bool IsViable(T route) => BestRoute is null || route.Length < BestRoute.Length;

    /// <summary>
    /// Finding the best route to the destination.
    /// </summary>
    public void Solve()
    {
        while (GetNextCandidate(out var route))
        {
            if (!IsViable(route)) continue;

            if (IsSolution(route))
                BestRoute = route;
            else
                ExtendRoute(route);
        }
        Candidates.Clear();
    }

    /// <summary>
    /// Updates the parent route and all children routes to use the new route instead of the old route.
    /// </summary>
    /// <param name="parentRoute">The parent route to update.</param>
    /// <param name="oldRoute">The old route fragment to replace.</param>
    /// <param name="newRoute">The new route fragment to insert.</param>
    private void UpdateRoute(T parentRoute, T oldRoute, T newRoute)
    {
        // Update each child route.
        foreach (T childRoute in parentRoute.Children)
        {
            UpdateRoute(childRoute, oldRoute, newRoute);
        }
        // Update the parent route.
        parentRoute.Replace(oldRoute, newRoute);
        // Ensure the parent route is correctly sorted.
        if (Candidates.Remove(parentRoute))
            Candidates.Add(parentRoute);
    }
}
