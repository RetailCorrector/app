using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.ModuleManager
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
            Draw();
        }

        private void Draw()
        {
            WindowStyle = WindowStyle.ToolWindow;
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "Менеджер реестров";
            Height = 200;
            DataContext = this;
            Width = 375;
            var textBox = new TextBox
            {
                Margin = new Thickness(5),
                AcceptsReturn = true,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            textBox.SetBinding(TextBox.TextProperty, nameof(Text));
            Content = textBox;
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
