using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners.ScannerMk2
{
    public class GunnerFactory : IGunnerFactory
    {
        public IGunner CreateGunner(Battlefield battlefield)
        {
            return new Gunner(battlefield);
        }

        public string Name
        {
            get { return "Scanner"; }
        }
    }
}
