using System.ComponentModel;

namespace RetailCorrector.Utils
{
    public class ConsoleTransfer: INotifyPropertyChanged
    {
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Output)));
            }
        }

        private string _output = "";
        private readonly AutoFlushStringWriter _writer;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ConsoleTransfer()
        {
            _writer = new();
            Console.SetOut(_writer);
            Console.SetError(_writer);
            _writer.Flushed += () => Output = _writer.ToString();
        }
    }
}
