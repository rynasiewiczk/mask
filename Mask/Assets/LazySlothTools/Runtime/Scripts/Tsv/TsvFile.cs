namespace LazySloth.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TsvFile
    {
        private readonly List<TsvLine> _lines = new List<TsvLine>();

        public void Save(string path)
        {
            File.WriteAllLines(path, _lines.Select(l => l.Line));
        }

        public void AddLine(TsvLine line)
        {
            _lines.Add(line);
        }

        public void AddLines(IEnumerable<TsvLine> lines)
        {
            _lines.AddRange(lines);
        }
    }
}