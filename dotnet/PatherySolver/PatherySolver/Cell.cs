namespace PatherySolver
{
    public static class Cell
    {
        public const char Empty = ' ';
        public const char FixedEmpty = '.';
        public const char Ice = '/';
        public const char FixedWall = '+';
        public const char UserWall = '-';
        public const char Exit = '=';

        public const string Gate = "}{";
        public const string In = "1234567890";
        public const string Out = "!@#$%^&*()";
        public const string Start = "><";
        public const string Checkpoint = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        private const string Walkable = " ./=" + Start + Checkpoint + In + Out;

        public static bool IsCheckpoint(char cell, out int n) => Is(Checkpoint, cell, out n);
        public static bool IsIn(char cell, out int n) => Is(In, cell, out n);
        public static bool IsOut(char cell, out int n) => Is(Out, cell, out n);
        public static bool IsStart(char cell, out int n) => Is(Start, cell, out n);
        public static bool IsWalkable(char cell, int bot) => Is(Walkable + Gate[bot], cell);

        private static bool Is(string options, char cell) => options.Contains(cell);
        private static bool Is(string options, char cell, out int index)
        {
            index = options.IndexOf(cell);
            return index >= 0;
        }
    }
}
