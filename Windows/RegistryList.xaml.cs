using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RetailCorrector.Wizard.Windows
{
    public partial class RegistryList : Window
    {
        public static ObservableCollection<ValueItem> Registries { get; set; } = [];

        public int SelectedIndex { get; set; } = -1;

        static RegistryList()
        {
            if (!File.Exists(DirPath.RegistryListPath)) File.Create(DirPath.RegistryListPath).Dispose();
            var lines = File.ReadAllLines(DirPath.RegistryListPath);
            foreach(var line in lines) Registries.Add(new ValueItem(line));
        }

        public RegistryList()
        {
            Closed += (_, _) =>
            {
                var result = new string[Registries.Count];
                for (int i = 0; i < Registries.Count; i++)
                    result[i] = Registries[i].Value;
                File.WriteAllLines(DirPath.RegistryListPath, result);
            };
            InitializeComponent();
        }

        private void Add(object? s, RoutedEventArgs a) => Registries.Add(new ValueItem("https://..."));
        private void Delete(object? s, RoutedEventArgs a)
        {
            if (SelectedIndex < 0) return;
            Registries.RemoveAt(SelectedIndex);
        }

        public class ValueItem(string value = "") : INotifyPropertyChanged
        {
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
            private string _value = value;

            public event PropertyChangedEventHandler? PropertyChanged;
        }
    }
}
