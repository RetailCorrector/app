using OrnaUtils;
using System.IO;

namespace RetailCorrector.Constants;

public static class Pathes
{
    #region Родительные каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\</code>
    /// </summary>
    public static string App => app.ToString();

    private static readonly DirectoryWatcher app = 
        new DirectoryWatcher(Environment.SpecialFolder.ProgramFilesX86)
        .Add("RetailCorrector");
    #endregion

    #region Дочерние каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\</code>
    /// </summary>
    public static string Settings => settings.ToString();
    private static readonly DirectoryWatcher settings = 
        new DirectoryWatcher(app).Add("Settings");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\</code>
    /// </summary>
    public static string Logs => logs.ToString();
    private static readonly DirectoryWatcher logs =
        new DirectoryWatcher(app).Add("Logs");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Plugins\</code>
    /// </summary>
    public static string Plugins => plugins.ToString();
    private static readonly DirectoryWatcher plugins =
        new DirectoryWatcher(app).Add("Plugins");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Receipts\</code>
    /// </summary>
    public static string Receipts => receipts.ToString();
    private static readonly DirectoryWatcher receipts =
        new DirectoryWatcher(app).Add("Receipts");
    #endregion

    #region Файлы
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\report.json</code>
    /// </summary>
    public static readonly string Report = Path.Combine(Settings, "report.json");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\registry.json</code>
    /// </summary>
    public static readonly string IndexedRegistry = Path.Combine(App, "registry.json");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\registry.list</code>
    /// </summary>
    public static readonly string RegistryList = Path.Combine(Settings, "registry.list");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\.log</code>
    /// </summary>
    public static readonly string Log = Path.Combine(Logs, ".log");
    #endregion

}
