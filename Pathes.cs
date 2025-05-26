namespace RetailCorrector.Constants;

public static class Pathes
{
    #region Родительные каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\</code>
    /// </summary>
    public readonly static string App = 
        SafeCombine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "RetailCorrector");
    #endregion

    #region Дочерние каталоги
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Wizard\</code>
    /// </summary>
    public readonly static string Wizard = SafeCombine(App, "Wizard");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Cashier\</code>
    /// </summary>
    public readonly static string Cashier = SafeCombine(App, "Cashier");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\RegistryManager\</code>
    /// </summary>
    public readonly static string RegistryManager = SafeCombine(App, "RegistryManager");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\</code>
    /// </summary>
    public readonly static string Settings = SafeCombine(App, "Settings");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\</code>
    /// </summary>
    public readonly static string Logs = SafeCombine(App, "Logs");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Modules\</code>
    /// </summary>
    public readonly static string Modules = SafeCombine(App, "Modules");
    #endregion

    #region Файлы
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\registry.json</code>
    /// </summary>
    public readonly static string IndexedRegistry = Path.Combine(App, "registry.json");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Settings\registry.list</code>
    /// </summary>
    public readonly static string RegistryList = Path.Combine(Settings, "registry.list");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\wizard.log</code>
    /// </summary>
    public readonly static string WizardLog = Path.Combine(Logs, "wizard.log");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\cashier.log</code>
    /// </summary>
    public readonly static string CashierLog = Path.Combine(Logs, "cashier.log");
    /// <summary>
    /// <code>C:\Program Files (x86)\RetailCorrector\Logs\registry.log</code>
    /// </summary>
    public readonly static string RegistryManagerLog = Path.Combine(Logs, "registry.log");
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
