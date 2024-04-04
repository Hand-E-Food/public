using RandomVectorMap.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RandomVectorMap.Generation;

/// <summary>
/// Creates the layout of junctions, roads and zones.
/// </summary>
public class LayoutGenerator : MapGeneratorComponent
{
    /// <summary>
    /// True if the direction has been reversed since the last zone was created; otherwise, false.
    /// </summary>
    private bool reversed = false;
    /// <summary>
    /// The junction that if selected as a terminal junction more than once.
    /// </summary>
    private Junction? abandonJunction = null;
    /// <summary>
    /// An older junction away from which new junction should be created.
    /// </summary>
    private Junction? biasJunction = null;
    /// <summary>
    /// The junction cursor around which zones are generated.
    /// </summary>
    private Junction? pivotJunction = null;
    /// <summary>
    /// The junction cursor defining the edge where zones are generated.
    /// </summary>
    private Junction? radialJunction = null;
    /// <summary>
    /// The junction cursor where a new pivot must be selected.
    /// </summary>
    private Junction? terminalJunction = null;
    /// <summary>
    /// The road cursor.
    /// </summary>
    private Road? newestRoad = null;
    /// <summary>
    /// The road between pivotJunction and terminalJunction.
    /// </summary>
    private Road? terminalRoad = null;

    /// <summary>
    /// Gets or sets the factory used to create base mapping objects.
    /// </summary>
    /// <value>The factory used to create base mapping objects.</value>
    public IMappingObjectFactory Factory { get; set; } = new DefaultMappingObjectFactory();

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished { get { return IsInitialized && isFinished; } }
    private bool isFinished = false;

    /// <summary>
    /// Gets or sets the maximum allowed number of junctions.
    /// </summary>
    public int MaximumJunctions { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum length of a road.
    /// </summary>
    public double MaximumRoadLength { get; set; } = 150;

    /// <summary>
    /// Gets or sets the minimum length of a road.
    /// </summary>
    public double MinimumRoadLength { get; set; } = 50;

    /// <summary>
    /// Gets or sets the location of the first junction.
    /// </summary>
    /// <value>The location of the first junction.</value>
    public Point Origin { get; set; } = new(0, 0);

    /// <summary>
    /// Gets or sets the zone generator used to define a new zone.
    /// </summary>
    /// <value>A zone generator.</value>
    public required TriangleGenerator ZoneGenerator { get; init; }

    /// <summary>
    /// Asserts that the specified class member is not null.
    /// </summary>
    /// <param name="member">The member to assert.</param>
    /// <param name="paramName">Do not specify this.</param>
    /// <exception cref="InvalidOperationException"><paramref name="member"/> is null.</exception>
    private static void AssertNotNull([NotNull] object? member, [CallerArgumentExpression(nameof(member))] string? paramName = null)
    {
        if (member is null) throw new InvalidOperationException($"{paramName} is null.");
    }

    /// <summary>
    /// Completes a zone that alread has two roads.
    /// </summary>
    /// <returns>True if progress was made; otherwise, false.</returns>
    private bool CompleteZone()
    {
        AssertNotNull(newestRoad);
        AssertNotNull(radialJunction);
        AssertNotNull(terminalJunction);
        AssertNotNull(terminalRoad);

        // If the new road could not reach the terminal junction or will intersect an existing road, fail.
        Line newRoadLine = new(radialJunction, terminalJunction);
        if (terminalJunction == biasJunction
            || terminalJunction == radialJunction
            || new Vector(radialJunction, terminalJunction).Length > MaximumRoadLength
            || Map.Roads.Any(r => r.Line.Intersects(newRoadLine)))
            return false;

        // Create the radial road.
        Road previousRoad = newestRoad;
        newestRoad = Factory.CreateRoad(radialJunction, terminalJunction);
        Map.Roads.Add(newestRoad);

        // Create the new zone.
        Zone newZone = Factory.CreateZone([previousRoad, terminalRoad, newestRoad]);
        Map.Zones.Add(newZone);

        // Reassign the junctions.
        biasJunction = pivotJunction;
        pivotJunction = terminalJunction;
        terminalRoad = pivotJunction.Roads.First(road => road.Zones.Contains(null) && road != newestRoad);
        terminalJunction = terminalRoad.Other(pivotJunction);
        abandonJunction = terminalJunction;

        // Return success.
        reversed = false;
        return true;
    }

    /// <summary>
    /// Creates the first junction.
    /// </summary>
    private void CreateFirstJunction()
    {
        pivotJunction = Factory.CreateJunction(Origin);
        Map.Junctions.Add(pivotJunction);
    }

    /// <summary>
    /// Creates the first road.
    /// </summary>
    private void CreateFirstRoad()
    {
        AssertNotNull(pivotJunction);

        // Create the new junction.
        double length = Random.NextDouble() * (MaximumRoadLength - MinimumRoadLength) + MinimumRoadLength;
        double angle = Random.NextDouble() * Math.PI * 2;
        radialJunction = Factory.CreateJunction(new((int)(Math.Sin(angle) * length), (int)(Math.Cos(angle) * length)));
        Map.Junctions.Add(radialJunction);

        // Create the new road.
        newestRoad = Factory.CreateRoad(pivotJunction, radialJunction);
        Map.Roads.Add(newestRoad);

        // Assign cursors.
        terminalJunction = radialJunction;
        terminalRoad = newestRoad;
        biasJunction = pivotJunction;
    }

    /// <summary>
    /// Creates the first zone.
    /// </summary>
    /// <returns>True if progress was made; otherwise, false.</returns>
    private bool CreateZone()
    {
        AssertNotNull(biasJunction);
        AssertNotNull(newestRoad);
        AssertNotNull(pivotJunction);
        AssertNotNull(radialJunction);

        // Select the location of the new junction.
        Point newPoint = ZoneGenerator.GenerateTriangle(pivotJunction, radialJunction, biasJunction);
        foreach (Line line in new Line[] { new(newPoint, radialJunction), new(newPoint, pivotJunction) })
        {
            if (Map.Roads.Any(r => r.Line.Intersects(line)))
                return false;
        }

        // Create the new junction.
        Junction newJunction = Factory.CreateJunction(newPoint);
        Map.Junctions.Add(newJunction);

        var zoneRoads = new Road[3];
        // Add the current road to the zone.
        zoneRoads[0] = newestRoad;
        // Create the radial road and add it to the zone.
        newestRoad = Factory.CreateRoad(radialJunction, newJunction);
        Map.Roads.Add(newestRoad);
        zoneRoads[1] = newestRoad;
        // Create the spoke road and add it to the zone.
        newestRoad = Factory.CreateRoad(pivotJunction, newJunction);
        Map.Roads.Add(newestRoad);
        zoneRoads[2] = newestRoad;

        // Create the new zone.
        Zone newZone = Factory.CreateZone(zoneRoads);
        Map.Zones.Add(newZone);

        // Reassign the junctions.
        biasJunction = radialJunction;
        radialJunction = newJunction;
        terminalRoad = pivotJunction.Roads.First(r => r != newestRoad && r.Zones.Contains(null));
        terminalJunction = terminalRoad.Other(pivotJunction);

        // Return success.
        reversed = false;
        return true;
    }

    /// <summary>
    /// The condition on which to break execution.
    /// </summary>
    /// <returns>True if the breakpoint should be triggered; otherwise, false.</returns>
    private bool DebugBreakpoint()
    {
        return Map.Zones.Count == 1396;
    }

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        ZoneGenerator.MaximumRoadLength = MaximumRoadLength;
        ZoneGenerator.MinimumRoadLength = MinimumRoadLength;
        ZoneGenerator.Random = Random;
    }

    /// <summary>
    /// Moves the terminal to the other side of the pivot junction.
    /// </summary>
    /// <returns>True if progress was made; otherwise, false.</returns>
    private bool ReverseDirection()
    {
        AssertNotNull(pivotJunction);
        AssertNotNull(radialJunction);

        // Check if the direction has already been reversed since the last zone was created.
        if (reversed) return false;
        reversed = true;

        // Reassign the junctions.
        biasJunction = pivotJunction;
        newestRoad = radialJunction.Roads.First(r => r.Zones.Contains(null) && r.Other(radialJunction) != biasJunction);
        pivotJunction = newestRoad.Other(radialJunction);

        // Set the terminal junction.
        terminalRoad = pivotJunction.Roads.First(road => road.Zones.Contains(null) && road != newestRoad);
        terminalJunction = terminalRoad.Other(pivotJunction);

        return true;
    }

    /// <summary>
    /// Creates subsequent zones.
    /// </summary>
    private void SelectNextMethod()
    {
        if (CompleteZone()) return;
        if (CreateZone()) return;
        if (ReverseDirection()) return;
        if (ShiftPivot()) return;
    }

    /// <summary>
    /// Shifts the pivot and radial junctions around the coast to find a roomier area.
    /// </summary>
    /// <returns>True if progress was made; otherwise, false.</returns>
    private bool ShiftPivot()
    {
        AssertNotNull(newestRoad);
        AssertNotNull(pivotJunction);
        AssertNotNull(radialJunction);

        // Assign the terminal road.
        terminalRoad = newestRoad;
        terminalJunction = pivotJunction;
        abandonJunction = terminalJunction;

        // Shift the pivot and radial junctions along the coast in the direction of the radial junction.
        newestRoad = radialJunction.Roads.First(road => road.Zones.Contains(null) && road != newestRoad);
        pivotJunction = radialJunction;
        radialJunction = newestRoad.Other(radialJunction);

        // Assign the bias junction as the junction that completes a triangle between the pivot and radial junctions.
        biasJunction =
            pivotJunction.Roads
            .Select(road => road.Other(pivotJunction))
            .Intersect(radialJunction.Roads.Select(road => road.Other(radialJunction)))
            .First();

        // Return success.
        return true;
    }

    /// <summary>
    /// Performs a single step of its task.
    /// </summary>
    public override void Step()
    {
        switch (Map.Junctions.Count)
        {
            case 0: CreateFirstJunction(); break;
            case 1: CreateFirstRoad(); break;
            case 2: CreateZone(); break;
            default: SelectNextMethod(); break;
        }
        // Mark debugging information.
        if (abandonJunction is not null) abandonJunction.DebugColor = Color.Black;
        if (terminalJunction is not null) terminalJunction.DebugColor = Color.Red;
        if (biasJunction is not null) biasJunction.DebugColor = Color.Yellow;
        if (pivotJunction is not null) pivotJunction.DebugColor = Color.Green;
        if (radialJunction is not null) radialJunction.DebugColor = Color.Blue;
        if (terminalRoad is not null) terminalRoad.DebugColor = Color.Red;
        if (newestRoad is not null) newestRoad.DebugColor = Color.Blue;
        // Check if this task is finished.
        if (DebugBreakpoint())
        {
            isFinished = true;
            new Thread(() =>
            {
                Thread.Sleep(100);
                isFinished = false;
            }).Start();
            return;
        }
        else
        {
            isFinished = Map.Junctions.Count >= MaximumJunctions;
        }
    }
}
