using System.IO;

namespace RetailCorrector.Utils
{
    public class AutoFlushStringWriter : StringWriter
    {
        public event Action? Flushed;

        public override void Flush()
        {
            base.Flush();
            Flushed?.Invoke();
        }

        public override void Write(char value)
        {
            base.Write(value);
            Flush();
        }

        public override void Write(string? value)
        {
            base.Write(value);
            Flush();
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            Flush();
        }
    }
}
