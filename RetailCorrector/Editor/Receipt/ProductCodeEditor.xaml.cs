using System.Windows;

namespace RetailCorrector.Editor.Receipt
{
    public partial class ProductCodeEditor : Window
    {
        public ProductCodeEditor(PositionViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
