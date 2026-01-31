namespace LazySloth.Utilities
{
    public class TsvLine
    {
        private string line = "";

        public string Line => line;

        public void AddValueToLine(string value)
        {
            if (! line.IsNullOrEmpty())
            {
                line += '\t';
            }

            line += value;
        }
    }
}