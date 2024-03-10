using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Centres the map.
    /// </summary>
    public class MapCentralizer : SingleStepMapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="MapCentralizer"/> class.
        /// </summary>
        public MapCentralizer()
        {
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            Rectangle bounds = new Rectangle();
            bounds.X = Map.Junctions.Min((j) => j.Location.X);
            bounds.Y = Map.Junctions.Min((j) => j.Location.Y);
            bounds.Width = Map.Junctions.Max((j) => j.Location.X) - bounds.X;
            bounds.Height = Map.Junctions.Max((j) => j.Location.Y) - bounds.Y;

            Size offset = new Size(
                -bounds.Width / 2 - bounds.X,
                -bounds.Height / 2 - bounds.Y
            );
            foreach(var junction in Map.Junctions)
            {
                junction.Location += offset; 
            }
            base.Step();
        }
    }
}
