using System.Runtime.Loader;

namespace RetailCorrector.ModuleManager;

public class ModuleLoadContext() : 
    AssemblyLoadContext(isCollectible: true) { }
