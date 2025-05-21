using System.Runtime.Loader;

namespace RetailCorrector.Wizard
{
    public class ModuleLoadContext() : 
        AssemblyLoadContext(isCollectible: true) { }
}
