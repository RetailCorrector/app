using System.IO;

namespace RetailCorrector.Constants;

public static class Pathes
{
    #region Родительные каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\</code>
    /// </summary>
    public static readonly string App = 
        SafeCombine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "RetailCorrector");
    #endregion

    #region Дочерние каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\</code>
    /// </summary>
    public static readonly string Settings = SafeCombine(App, "Settings");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\</code>
    /// </summary>
    public static readonly string Logs = SafeCombine(App, "Logs");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Modules\</code>
    /// </summary>
    public static readonly string Modules = SafeCombine(App, "Modules");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Receipts\</code>
    /// </summary>
    public static readonly string Receipts = SafeCombine(App, "Receipts");
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

    private static string SafeCombine(params string[] paths)
    {
        var path = paths[0];
        for(var i = 1; i < paths.Length; i++)
        {
            path = Path.Combine(path, paths[i]);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        return path;
    }
}
