using System.Reflection;
using System.Runtime.InteropServices;

namespace RetailCorrector.ModuleManager.Data
{
    public readonly struct LocalModule(Assembly assembly, string path)
    {
        public Guid Id { get; } = new Guid(assembly.GetCustomAttribute<GuidAttribute>()!.Value);
        public string Name { get; } = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
        public Version Version { get; } = Version.Parse(assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version);
        public string Path { get; } = path;
    }
}
