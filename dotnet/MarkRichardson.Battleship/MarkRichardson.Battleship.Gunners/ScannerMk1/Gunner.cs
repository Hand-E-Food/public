using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners.ScannerMk1
{
    public class Gunner : IGunner
    {
        private Battlefield _battlefield;
        private Cell _target = new Cell();

        public Gunner(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        public BattlefieldStatus Fire()
        {
            _target.X = (_target.X + 7);
            if (_target.X >= Battlefield.Size)
            {
                _target.X = _target.X % Battlefield.Size;
                _target.Y = (_target.Y + 1) % Battlefield.Size;
            }
            return _battlefield.Attack(_target);
        }
    }
}
