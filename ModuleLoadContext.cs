using System.Runtime.Loader;

namespace RetailCorrector.RegistryManager;

public class ModuleLoadContext() : 
    AssemblyLoadContext(isCollectible: true) { }
