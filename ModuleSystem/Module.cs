namespace RetailCorrector.Cashier.ModuleSystem
{
    public class Module(string guid, string name, AbstractFiscalModule ep, FileStream fs)
    {
        public Guid Guid { get; set; } = new Guid(guid);
        public string Name { get; set; } = name;
        public AbstractFiscalModule? EntryPoint { get; set; } = ep;
        public FileStream? Stream { get; set; } = fs;
    }
}