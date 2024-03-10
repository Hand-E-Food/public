using System;
using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;
using System.Collections.Generic;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Creates the layout of junctions, roads and zones.
    /// </summary>
    public class LayoutGenerator : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="LayoutGenerator"/> class.
        /// </summary>
        public LayoutGenerator()
        {
            Factory = new DefaultMappingObjectFactory();
            MaximumJunctions = 100;
            MaximumRoadLength = 150;
            MinimumRoadLength = 50;
            Origin = new Point(0, 0);
        }

        #region Properties ...

        /// <summary>
        /// True if the direction has been reversed since the last zone was created; otherwise, false.
        /// </summary>
        private bool reversed = false;
        /// <summary>
        /// The junction that if selected as a terminal junction more than once.
        /// </summary>
        private Junction abandonJunction = null;
        /// <summary>
        /// An older junction away from which new junction should be created.
        /// </summary>
        private Junction biasJunction = null;
        /// <summary>
        /// The junction cursor around which zones are generated.
        /// </summary>
        private Junction pivotJunction = null;
        /// <summary>
        /// The junction cursor defining the edge where zones are generated.
        /// </summary>
        private Junction radialJunction = null;
        /// <summary>
        /// The junction cursor where a new pivot must be selected.
        /// </summary>
        private Junction terminalJunction = null;
        /// <summary>
        /// The road cursor.
        /// </summary>
        private Road newestRoad = null;
        /// <summary>
        /// The road between pivotJunction and terminalJunction.
        /// </summary>
        private Road terminalRoad = null;

        /// <summary>
        /// Gets or sets the factory used to create base mapping objects.
        /// </summary>
        /// <value>The factory used to create base mapping objects.</value>
        public IMappingObjectFactory Factory { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && _IsFinished; } }
        private bool _IsFinished = false;

        /// <summary>
        /// Gets or sets the maximum allowed number of junctions.
        /// </summary>
        public int MaximumJunctions { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of a road.
        /// </summary>
        public double MaximumRoadLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of a road.
        /// </summary>
        public double MinimumRoadLength { get; set; }

        /// <summary>
        /// Gets or sets the location of the first junction.
        /// </summary>
        /// <value>The location of the first junction.</value>
        public Point Origin { get; set; }

        /// <summary>
        /// Gets or sets the zone generator used to define a new zone.
        /// </summary>
        /// <value>A zone generator.</value>
        public TriangleGenerator ZoneGenerator { get; set; }

        #endregion

        /// <summary>
        /// Completes a zone that alread has two roads.
        /// </summary>
        /// <returns>True if progress was made; otherwise, false.</returns>
        private bool CompleteZone()
        {
            // If the new road could not reach the terminal junction or will intersect an existing road, fail.
            Line newRoadLine = new Line(radialJunction, terminalJunction);
            if (terminalJunction == biasJunction
                || terminalJunction == radialJunction
                || new Vector(radialJunction, terminalJunction).Length > MaximumRoadLength
                || Map.Roads.Any((r) => r.Line.Intersects(newRoadLine)))
                    return false;
            
            // Create the radial road.
            var previousRoad = newestRoad;
            newestRoad = Factory.CreateRoad(radialJunction, terminalJunction);
            newestRoad.Name = radialJunction.Name + " - " + terminalJunction.Name;
            Map.Roads.Add(newestRoad);
            
            // Create the new zone.
            Zone newZone = Factory.CreateZone(new[] { previousRoad, terminalRoad, newestRoad });
            newZone.Name = NextZoneName();
            Map.Zones.Add(newZone);

            // Reassign the junctions.
            biasJunction = pivotJunction;
            pivotJunction = terminalJunction;
            terminalRoad = pivotJunction.Roads.First((road) => road.Zones.Contains(null) && road != newestRoad);
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
            pivotJunction.Name = NextJunctionName();
            Map.Junctions.Add(pivotJunction);
        }

        /// <summary>
        /// Creates the first road.
        /// </summary>
        private void CreateFirstRoad()
        {
            // Create the new junction.
            double length = Random.NextDouble() * (MaximumRoadLength - MinimumRoadLength) + MinimumRoadLength;
            double angle = Random.NextDouble() * Math.PI * 2;
            radialJunction = Factory.CreateJunction(new Point((int)(Math.Sin(angle) * length), (int)(Math.Cos(angle) * length)));
            radialJunction.Name = NextJunctionName();
            Map.Junctions.Add(radialJunction);

            // Create the new road.
            newestRoad = Factory.CreateRoad(pivotJunction, radialJunction);
            newestRoad.Name = pivotJunction.Name + " - " + radialJunction.Name;
            Map.Roads.Add(newestRoad);

            // Assign cursors.
            terminalJunction = radialJunction;
            terminalRoad = newestRoad;
        }

        /// <summary>
        /// Creates the first zone.
        /// </summary>
        /// <returns>True if progress was made; otherwise, false.</returns>
        private bool CreateZone()
        {
            // Select the location of the new junction.
            Point newPoint = ZoneGenerator.GenerateTriangle(pivotJunction, radialJunction, biasJunction);
            foreach (Line line in new[] {new Line(newPoint, radialJunction), new Line(newPoint, pivotJunction)})
            {
                if (Map.Roads.Any((r) => r.Line.Intersects(line)))
                    return false;
            }

            // Create the new junction.
            Junction newJunction = Factory.CreateJunction(newPoint);
            newJunction.Name = NextJunctionName();
            Map.Junctions.Add(newJunction);

            var zoneRoads = new Road[3];
            // Add the current road to the zone.
            zoneRoads[0] = newestRoad;
            // Create the radial road and add it to the zone.
            newestRoad = Factory.CreateRoad(radialJunction, newJunction);
            newestRoad.Name = radialJunction.Name + " - " + newJunction.Name;
            Map.Roads.Add(newestRoad);
            zoneRoads[1] = newestRoad;
            // Create the spoke road and add it to the zone.
            newestRoad = Factory.CreateRoad(pivotJunction, newJunction);
            newestRoad.Name = pivotJunction.Name + " - " + newJunction.Name;
            Map.Roads.Add(newestRoad);
            zoneRoads[2] = newestRoad;

            // Create the new zone.
            Zone newZone = Factory.CreateZone(zoneRoads);
            newZone.Name = NextZoneName();
            Map.Zones.Add(newZone);

            // Reassign the junctions.
            biasJunction = radialJunction;
            radialJunction = newJunction;
            terminalRoad = pivotJunction.Roads.First((r) => r != newestRoad && r.Zones.Contains(null));
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
        /// Gets the next junction name.
        /// </summary>
        /// <returns>A junction name</returns>
        public string NextJunctionName()
        {
            return Map.Junctions.Count.ToString();
        }

        /// <summary>
        /// Gets the next zone's name.
        /// </summary>
        /// <returns>The next zone's name.</returns>
        public string NextZoneName()
        {
            int value = Map.Zones.Count;
            string name = "";
            do
            {
                name = char.ConvertFromUtf32((value % 26) + 65) + name;
                value = value / 26;
            }
            while (value > 0);
            return name;
        }

        /// <summary>
        /// Moves the terminal to the other side of the pivot junction.
        /// </summary>
        /// <returns>True if progress was made; otherwise, false.</returns>
        private bool ReverseDirection()
        {
            // Check if the direction has already been reversed since the last zone was created.
            if (reversed) return false;
            reversed = true;

            // Reassign the junctions.
            biasJunction = pivotJunction;
            newestRoad = radialJunction.Roads.First((r) => r.Zones.Contains(null) && r.Other(radialJunction) != biasJunction);
            pivotJunction = newestRoad.Other(radialJunction);

            // Set the terminal junction.
            terminalRoad = pivotJunction.Roads.First((road) => road.Zones.Contains(null) && road != newestRoad);
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
            // Assign the terminal road.
            terminalRoad = newestRoad;
            terminalJunction = pivotJunction;
            abandonJunction = terminalJunction;

            // Shift the pivot and radial junctions along the coast in the direction of the radial junction.
            newestRoad = radialJunction.Roads.First((r) => r.Zones.Contains(null) && r != newestRoad);
            pivotJunction = radialJunction;
            radialJunction = newestRoad.Other(radialJunction);
            
            // Assign the bias junction as the junction that completes a triangle between the pivot and radial junctions.
            biasJunction = 
                pivotJunction.Roads.Select((r) => r.Other(pivotJunction))
                .Intersect(
                    radialJunction.Roads.Select((r) => r.Other(radialJunction))
                ).First();

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
            if (abandonJunction != null) abandonJunction.DebugColor = Color.Black;
            if (terminalJunction != null) terminalJunction.DebugColor = Color.Red;
            if (biasJunction != null) biasJunction.DebugColor = Color.Yellow;
            if (pivotJunction != null) pivotJunction.DebugColor = Color.Green;
            if (radialJunction != null) radialJunction.DebugColor = Color.Blue;
            if (terminalRoad != null) terminalRoad.DebugColor = Color.Red;
            if (newestRoad != null) newestRoad.DebugColor = Color.Blue;
            // Check if this task is finished.
            if (DebugBreakpoint())
            {
                _IsFinished = true;
                new System.Threading.Thread(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    _IsFinished = false;
                }).Start();
                return;
            }
            else
            {
                _IsFinished = Map.Junctions.Count >= MaximumJunctions;
            }
        }
    }
}
