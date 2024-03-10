using System;
using System.Collections.Generic;
using System.IO;

namespace OrixoSolver
{
    class PuzzleReader : IDisposable
    {
        private const char PuzzleNameCharacter = '"';

        private bool isDisposed = false;
        private string line = null;
        private StreamReader reader;

        public PuzzleReader(Stream stream)
        {
            reader = new StreamReader(stream);
            line = reader.ReadLine();
        }

        public Puzzle ReadPuzzle()
        {
            while (line != null && !line.StartsWith(PuzzleNameCharacter))
                line = reader.ReadLine();
            if (line == null) return null;

            var name = line[1..];
            var map = new List<string>();

            while (true)
            {
                line = reader.ReadLine();
                if (line == null || line.StartsWith(PuzzleNameCharacter))
                    break;
                
                if (!string.IsNullOrWhiteSpace(line) || map.Count > 0)
                    map.Add(line);
            }

            return new Puzzle(name, map);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    reader.Dispose();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
