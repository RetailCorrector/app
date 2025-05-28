using System.IO;

namespace RetailCorrector.Wizard.ModuleSystem
{
    public class Module(string guid, string name, AbstractSourceModule ep, FileStream fs)
    {
        public Guid Guid { get; set; } = new Guid(guid);
        public string Name { get; set; } = name;
        public AbstractSourceModule? EntryPoint { get; set; } = ep;
        public FileStream? Stream { get; set; } = fs;
    }
}