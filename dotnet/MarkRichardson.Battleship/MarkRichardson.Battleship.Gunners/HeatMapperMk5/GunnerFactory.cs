using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners.HeatMapperMk5
{
    public class GunnerFactory : IGunnerFactory
    {
        public IGunner CreateGunner(Battlefield battlefield)
        {
            return new Gunner(battlefield);
        }

        public string Name
        {
            get { return "Heat Mapper with Heat Seeking"; }
        }
    }
}
