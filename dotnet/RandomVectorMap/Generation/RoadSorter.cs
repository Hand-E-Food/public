using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Sorts all roads on the map in order of quality for the purposes of painting.
    /// </summary>
    public class RoadSorter : SingleStepMapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomVectorMap.Generation.RoadSorter"> class.
        /// </summary>
        public RoadSorter()
        {
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Sort all roads in order of quality for the purposes of painting.
            Map.Roads.Sort((x, y) => x.Quality.CompareTo(y.Quality));
            // This task is finished.
            base.Step();
        }
    }
}
