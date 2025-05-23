namespace RetailCorrector.Constants
{
    public static class Pathes
    {
        #region Родительные каталоги
        public readonly static string App = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "RetailCorrector");
        #endregion

        #region Дочерние каталоги
        public readonly static string Wizard = Path.Combine(App, "Wizard");
        public readonly static string Cashier = Path.Combine(App, "Cashier");
        public readonly static string Settings = Path.Combine(App, "Settings");
        public readonly static string Logs = Path.Combine(App, "Log");
        public readonly static string Modules = Path.Combine(App, "Modules");
        #endregion

        #region Файлы
        public readonly static string Registry = Path.Combine(App, "registry.json");
        #endregion
    }
}
