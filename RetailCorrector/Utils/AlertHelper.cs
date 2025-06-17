using System.Windows;

namespace RetailCorrector.Utils
{
    public static class AlertHelper
    {
        /// <summary>
        /// Ошибка, вызывающая завершение работы программы
        /// </summary>
        public static void Fatal(string text, Exception? e = null)
        {
            Log.Fatal(e, text);
            MessageBox.Show(text, "КРИТИЧЕСКАЯ ОШИБКА!!!", MessageBoxButton.OK, MessageBoxImage.Hand);
            Environment.Exit(-1);
        }

        /// <summary>
        /// Ошибка
        /// </summary>
        public static void Error(string text, Exception? e = null)
        {
            Log.Error(e, text);
            MessageBox.Show(text, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Предупреждение
        /// </summary>
        public static void Warning(string text, Exception? e = null)
        {
            Log.Warning(e, text);
            MessageBox.Show(text, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Запись в лог с оповщением
        /// </summary>
        public static void Info(string text, Exception? e = null)
        {
            Log.Information(e, text);
            MessageBox.Show(text, "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Тихая запись в лог
        /// </summary>
        public static void Debug(string text, Exception? e = null) =>
            Log.Debug(e, text);
    }
}