using MarkRichardson.Battleship.Gunners;
using MarkRichardson.Battleship.Navy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkRichardson.Battleship.Comparison
{
    class GunnerRecord
    {
        private IGunnerFactory _factory;

        public int Attacks { get; set; }

        public string Name { get { return _factory.Name; } }

        public GunnerRecord(IGunnerFactory factory)
        {
            _factory = factory;
        }

        public int Battle(Battlefield battlefield)
        {
            var gunner = _factory.CreateGunner(battlefield);
            BattlefieldStatus result;
            do
            {
                result = gunner.Fire();
            }
            while (!result.Victory);
            return result.Attacks;

        }
    }
}
