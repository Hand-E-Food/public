using PsiFi.Geometry;

namespace PsiFi.Models
{
    delegate bool CanPermeate(Cell cell);

    class Vision
    {
        /// <summary>
        /// Sixth-sense vision that permeates all materials.
        /// </summary>
        public static readonly CanPermeate Omni = cell => true;

        /// <summary>
        /// Traditional optical vision.
        /// </summary>
        public static readonly CanPermeate Optical = cell => cell.Occupant?.BlocksEnergy != true;

        /// <summary>
        /// The condition that checks whether this vision can permeate a particular cell.
        /// </summary>
        public CanPermeate CanPermeate { get; }

        /// <summary>
        /// The distance over which this vision is effective.
        /// </summary>
        public int Distance { get; }

        /// <summary>
        /// This vision's origin.
        /// </summary>
        public Point Origin { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Vision"/> class.
        /// </summary>
        /// <param name="origin">This vision's origin.</param>
        /// <param name="canPermeate">The condition that checks whether this vision can permeate a particular cell.</param>
        /// <param name="distance">The distance over which this vision is effective.</param>
        public Vision(Point origin, CanPermeate canPermeate, int distance)
        {
            Origin = origin;
            CanPermeate = canPermeate;
            Distance = distance;
        }
    }
}