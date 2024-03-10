using System;
using System.Linq;

namespace PatherySolver.Player
{
    internal struct Id : IEquatable<Id>
    {
        private readonly byte[] bytes;
        private readonly int hashCode;

        /// <summary>
        /// Initialises a new <see cref="Id"/> from a map.
        /// </summary>
        /// <param name="map">The map used to form the id.</param>
        public Id(Map map)
        {
            var locations = map.Find(Cell.UserWall).ToList();
            bytes = new byte[locations.Count * 2];

            var hasher = new HashCode();
            for (int i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                bytes[i * 2 + 0] = (byte)location.X;
                bytes[i * 2 + 1] = (byte)location.Y;
                hasher.Add(location);
            }
            hashCode = hasher.ToHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Id other && this == other;

        /// <inheritdoc/>
        public bool Equals(Id other) => this == other;

        /// <inheritdoc/>
        public override int GetHashCode() => hashCode;

        public static bool operator ==(Id a, Id b) =>
            a.hashCode == b.hashCode && a.bytes.SequenceEqual(b.bytes);

        public static bool operator !=(Id a, Id b) => !(a == b);
    }
}
