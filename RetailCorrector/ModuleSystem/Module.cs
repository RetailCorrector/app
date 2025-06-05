using System.IO;

namespace RetailCorrector.Wizard.ModuleSystem
{
    public class Module(string name, AbstractSourceModule ep, FileStream fs)
    {
        public string Name { get; set; } = name;
        public AbstractSourceModule? EntryPoint { get; set; } = ep;
        public FileStream? Stream { get; set; } = fs;
    }
}