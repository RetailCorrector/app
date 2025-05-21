using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RetailCorrector.Wizard
{
    public class KVPair(string key = "", object value = null!) : INotifyPropertyChanged
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
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public DateTime? ValueAsDateTime
        {
            get
            {
                return Value is DateOnly d ? d.ToDateTime(new TimeOnly(0)) : null;
            }
            set
            {
                if (value.HasValue)
                    Value = DateOnly.FromDateTime(value.Value);
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> ValuesFromEnum
        {
            get
            {
                var type = Value.GetType();
                if (!type.IsEnum) return [];
                var values = Enum.GetValues(type);
                var res = new ObservableCollection<KeyValuePair<int, string>>();
                var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
                for (var i = 0; i < values.Length; i++)
                {
                    var value = values.GetValue(i);
                    var display = fields.First(m => (int)m.GetValue(null)! == (int)value!)
                        .GetCustomAttribute<DisplayAttribute>();
                    res.Add(new KeyValuePair<int, string>((int)value!, display?.Name ?? "???"));
                }
                return res;
            }
        }

        private string _key = key;
        private object _value = value;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") =>
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
    }
}
