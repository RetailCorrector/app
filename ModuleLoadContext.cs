using System.Runtime.Loader;

namespace RetailCorrector.Cashier
{
    public class ModuleLoadContext() :
        AssemblyLoadContext(isCollectible: true)
    { }
}