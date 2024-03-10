using PsiFi.Models;
using PsiFi.Models.Structures;
using System.Linq;

namespace PsiFi.Generators.Maps
{
    abstract class EmptyMap : IMapGenerator
    {
        protected const int Width = 80;
        protected const int Height = 40;

        protected Map Map { get; private set; } = null!;
        protected Random Random => State.Random;
        protected State State { get; private set; } = null!;

        public Map GenerateMap(State state)
        {
            State = state;
            Map = new Map(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                Map[x, 0].Occupant = Wall.Indestructable;
                Map[x, Height - 1].Occupant = Wall.Indestructable;
            }

            for (int y = 1; y < Height - 1; y++)
            {
                Map[0, y].Occupant = Wall.Indestructable;
                Map[Width - 1, y].Occupant = Wall.Indestructable;
            }

            GenerateMap();

            CollateActors();

            return Map;
        }

        protected virtual void GenerateMap()
        { }

        private void CollateActors()
        {
            var actors = new[] {
                Map.AllCells.Select(cell => cell.Annotation).OfType<IActor>(),
                Map.AllCells.Select(cell => cell.Occupant).OfType<IActor>(),
                Map.AllCells.Select(cell => cell.Item).OfType<IActor>(),
                Map.AllCells.Select(cell => cell.Floor).OfType<IActor>(),
            }
                .SelectMany(actor => actor)
                .ToList();

            for (var i = 0; i < actors.Count; i++)
                actors.AddRange(actors[i].GetChildActors());

            foreach (var actor in actors)
                Map.Actors.Add(actor);
        }
    }
}
