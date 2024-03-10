using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners.ScannerMk1
{
    public class GunnerFactory : IGunnerFactory
    {
        public IGunner CreateGunner(Battlefield battlefield)
        {
            return new Gunner(battlefield);
        }

        public string Name
        {
            get { return "Blind Scanner"; }
        }
    }
}
