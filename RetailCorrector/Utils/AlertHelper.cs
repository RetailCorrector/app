using System.Windows;

namespace RetailCorrector.Utils
{
    public static class AlertHelper
    {
        public static void ErrorAlert(Exception e, string text)
        {
            Log.Error(e, text);
            MessageBox.Show(e.Message, text, 0, MessageBoxImage.Error);
        }

        public static void ErrorAlert(string text)
        {
            Log.Error(text);
            MessageBox.Show(text, "", 0, MessageBoxImage.Error);
        }

        public static void Alert(string text)
        {
            Log.Information(text);
            MessageBox.Show(text);
        }
    }
}
