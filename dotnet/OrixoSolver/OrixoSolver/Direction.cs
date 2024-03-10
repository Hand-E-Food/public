namespace OrixoSolver
{
    public class Direction
    {
        public static readonly Direction E = new Direction(1, 0);
        public static readonly Direction N = new Direction(0, -1);
        public static readonly Direction S = new Direction(0, 1);
        public static readonly Direction W = new Direction(-1, 0);
        public static readonly Direction[] Cardinal = { N, E, S, W };

        public int DX { get; }
        
        public int DY { get; }

        public Direction(int dx, int dy)
        {
            DX = dx;
            DY = dy;
            hashCode = (dx + 1) * 3 + (dy + 1);
        }

        public override bool Equals(object obj) => obj is Direction that && this == that;

        public override int GetHashCode() => hashCode;
        private readonly int hashCode;

        public static bool operator ==(Direction left, Direction right) => left.DX == right.DX && left.DY == right.DY;

        public static bool operator !=(Direction left, Direction right) => !(left == right);
    }
}
