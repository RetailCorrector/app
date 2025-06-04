namespace RetailCorrector.Cashier.ModuleSystem
{
    public class Module(string name, AbstractFiscalModule ep, FileStream fs)
    {
        public string Name { get; set; } = name;
        public AbstractFiscalModule? EntryPoint { get; set; } = ep;
        public FileStream? Stream { get; set; } = fs;
    }
}