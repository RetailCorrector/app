using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RetailCorrector.Editor.Report
{
    public partial class Report
    {
        public class StringsPair(string key = "", string value = "") : INotifyPropertyChanged
        {
            public string Key
            {
                get => _key;
                set
                {
                    _key = value;
                    OnPropertyChanged();
                }
            }
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }

            private string _key = key;
            private string _value = value;

            public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propName = "") =>
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
        }
    }
}