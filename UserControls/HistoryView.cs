using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailCorrector.Wizard.UserControls
{
    public class HistoryView: Border
    {
        public HistoryView(IHistoryAction action)
        {
            Child = new TextBlock { Text = action.DisplayName };
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
        }

        public void SwitchDone(bool done)
        {
            Background = done ? Brushes.White : Brushes.LightGray;
        }
    }
}
