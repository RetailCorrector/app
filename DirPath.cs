using System.IO;

namespace RetailCorrector.Wizard
{
    internal static class DirPath
    {
        public static string AppDir { get; } = AppContext.BaseDirectory;
        public static string LogsDir { get; } = Path.Combine(AppDir, "Logs");
    }
}
