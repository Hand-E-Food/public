namespace FiveStar
{
    public struct SphericalPoint
    {
        /// <summary>
        /// A text description.
        /// </summary>
        public string Name;

        /// <summary>
        /// The polar angle or longitude.
        /// </summary>
        public double θ;

        /// <summary>
        /// The azimuthal angle or latitude.
        /// </summary>
        public double φ;

        /// <summary>
        /// The radial distance or altitude.
        /// </summary>
        public double ρ;

        /// <summary>
        /// Initialises a new instance of the <see cref="SphericalPoint"/> structure.
        /// </summary>
        /// <param name="θ">The polar angle or longitude.</param>
        /// <param name="φ">The azimuthal angle or latitude.</param>
        /// <param name="ρ">The radial distance or altitude.</param>
        /// <param name="name">A text description.</param>
        public SphericalPoint(double θ, double φ, double ρ = 1.0, string name = null)
        {
            this.Name = name;
            this.θ = θ;
            this.φ = φ;
            this.ρ = ρ;
        }
    }
}
