using System.Runtime.Loader;

namespace RetailCorrector.Cashier.ModuleSystem
{
    public class ModuleLoadContext() :
        AssemblyLoadContext(isCollectible: true)
    { }
}