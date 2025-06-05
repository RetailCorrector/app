using System.IO;
using RetailCorrector.Plugin;

namespace RetailCorrector.PluginSystem
{
    public class Plugin
    {
        public string Name { get; set; }
        public IPlugin? EntryPoint { get; set; }
        public FileStream? Stream { get; set; }

        private Plugin(string name, IPlugin ep, FileStream fs)
        {
            Name = name;
            EntryPoint = ep;
            Stream = fs;
        }
        public Plugin(string name, FiscalPlugin ep, FileStream fs) : this(name, (IPlugin)ep, fs) { }
        public Plugin(string name, SourcePlugin ep, FileStream fs) : this(name, (IPlugin)ep, fs) { }
    }
}