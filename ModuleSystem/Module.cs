namespace RetailCorrector.Wizard.ModuleSystem
{
    public class Module
    {
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; } = "";
        public AbstractSourceModule? EntryPoint { get; set; }
    }
}