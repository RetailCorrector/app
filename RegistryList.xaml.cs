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
            var fullDir = Path.GetDirectoryName(Pathes.RegistryList)!;
            if (!Directory.Exists(fullDir))
            {
                var dirParts = fullDir.Split(Path.DirectorySeparatorChar);
                var dir = dirParts[0];
                for(var i = 0; i < dirParts.Length; i++)
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    if (dirParts.Length - 1 > i)
                        dir = Path.Combine(dir, dirParts[i + 1]);
                }
            }
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
