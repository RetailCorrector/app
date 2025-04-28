using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    /// <summary>
    /// Логика взаимодействия для Publisher.xaml
    /// </summary>
    public partial class Publisher : UserControl
    {
        public Publisher()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var chunks = App.Receipts.Edited.Chunk(25);
            foreach (var chunk in chunks)
            {
                var text = JsonSerializer.Serialize(chunk);
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetRandomFileName());
                File.WriteAllText(path, text);
            }
        }
    }
}
