using System.IO;

namespace RetailCorrector.Wizard
{
    public class LocalModule : IDisposable
    {
        public string Guid { get; set; } = "";
        public string Name { get; set; } = "";
        public Version Version { get; set; } = new Version();
        public string HashSum { get; set; } = "";
        public string FilePath { get; set; } = "";
        public AbstractSourceModule? EntryPoint { get; set; }
        public ModuleLoadContext LoadContext { get; set; } = null!;
        public Stream Stream { get; set; } = null!;

        public void Dispose()
        {
            EntryPoint!.OnUnload().Wait();
            EntryPoint = null;
            LoadContext.Unload();
            Stream.Dispose();
        }
    }
}