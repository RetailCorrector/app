using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RetailCorrector.RegistryManager
{
    public partial class RegistryList : Window
    {
        public static ObservableCollection<string> Registries { get; set; } = [];
        public string Text { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        static RegistryList()
        {
            if (!File.Exists(Pathes.RegistryList)) 
                File.WriteAllText(Pathes.RegistryList, "");
            Registries = [..File.ReadAllLines(Pathes.RegistryList)];
        }

        public RegistryList()
        {
            Text = string.Join("\r\n", Registries);
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            Registries.Clear();
            foreach(var line in Text.Split("\r\n"))
                Registries.Add(line); 
            File.WriteAllLines(Pathes.RegistryList, Registries);
        }
    }
}
