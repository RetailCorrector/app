using System.Windows;
using RetailCorrector.Utils;

namespace RetailCorrector
{
    public partial class Main : Window
    {
        public Main()
        {
            CommandBindings.AddRange(Commands.Init());
            InitializeComponent();
        }
    }
}
