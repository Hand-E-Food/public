using PsiFi.Models.Mapping;

namespace PsiFi.Models
{
    class Map
    {
        public Cell[,] Cells { get; }
        public Size Size { get; }

        public Map(Size size)
        {
            Size = size;
            Cells = new Cell[size.Width, size.Height];
            for (int y = 0; y < size.Height; y++)
                for (int x = 0; x < size.Width; x++)
                    Cells[x, y] = new Cell(x, y);
        }
    }
}
