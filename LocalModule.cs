namespace RetailCorrector.Wizard
{
    public class LocalModule
    {
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; } = "";
        public AbstractSourceModule? EntryPoint { get; set; }
    }
}