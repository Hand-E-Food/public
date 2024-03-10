namespace OrixoSolver
{
    public abstract class Cell
    {
        public bool IsUsed { get; set; } = false;
        
        public Point Location { get; }

        public Cell(Point location)
        {
            Location = location;
        }
    }
}
