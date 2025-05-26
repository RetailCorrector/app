using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RetailCorrector.RegistryManager
{
    public partial class RegistryList : Window
    {
        public static string[] Registries { get; set; } = [];
        public string Text { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        static RegistryList()
        {
            if (!File.Exists(Pathes.RegistryList)) 
                File.WriteAllText(Pathes.RegistryList, "");
            Registries = File.ReadAllLines(Pathes.RegistryList);
        }

        public RegistryList()
        {
            Text = string.Join("\r\n", Registries);
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            Registries = Text.Split("\r\n"); 
            File.WriteAllLines(Pathes.RegistryList, Registries);
        }
    }
}
