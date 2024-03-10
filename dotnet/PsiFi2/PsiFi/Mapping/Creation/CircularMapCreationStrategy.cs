using Rogue;
using Rogue.Mapping.Creation;

namespace PsiFi.Mapping.Creation
{
    public class CircularMapCreationStrategy : CircularMapCreationStrategy<Map, Cell>
    {
        public CircularMapCreationStrategy(Size size) : base(new CellFactory(), size)
        { }
    }
}
