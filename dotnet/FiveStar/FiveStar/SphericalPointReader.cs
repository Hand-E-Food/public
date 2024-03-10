using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FiveStar
{
    public class SphericalPointReader : IEnumerable<SphericalPoint>
    {
        private readonly double defaultρ;
        private readonly TextReader reader;

        public SphericalPointReader(TextReader reader, double defaultρ = 1.0)
        {
            this.reader = reader;
            this.defaultρ = defaultρ;
        }

        public IEnumerator<SphericalPoint> GetEnumerator() => new Enumerator(reader, defaultρ);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<SphericalPoint>
        {
            private readonly double defaultρ;
            private readonly TextReader reader;

            public SphericalPoint Current { get; set; }
            object IEnumerator.Current => Current;

            public Enumerator(TextReader reader, double defaultρ)
            {
                this.reader = reader;
                this.defaultρ = defaultρ;
            }

            public void Dispose()
            { }

            public bool MoveNext()
            {
                string line;
                do
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        Current = default(SphericalPoint);
                        return false;
                    }
                }
                while (string.IsNullOrWhiteSpace(line));

                Current = Parse(line);
                return true;
            }

            private SphericalPoint Parse(string text)
            {
                var values = text.Split(new[] { ',' }, StringSplitOptions.None);
                if (values.Length < 2 || values.Length > 3)
                    throw new FormatException("All lines must contain 2 or 3 comma-separated values: logitude, latitude, and optionally altitude.");

                return new SphericalPoint(
                    double.Parse(values[0]),
                    double.Parse(values[1]),
                    values.Length >= 3 ? double.Parse(values[2]) : defaultρ
                );
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }
}
