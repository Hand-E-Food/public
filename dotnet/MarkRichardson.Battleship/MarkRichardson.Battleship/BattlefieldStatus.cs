namespace MarkRichardson.Battleship
{

    /// <summary>
    /// The results of an attack.
    /// </summary>
    public class BattlefieldStatus
    {
        public AttackResult AttackResult;
        public int Attacks;
        public Cell Cell;
        public bool Victory;
    }
}
