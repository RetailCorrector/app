namespace RetailCorrector.PluginSystem
{
    public readonly struct LocalPluginInfo
    {
        public Guid Id { get; }
        public string Name { get; }
        public Version Version { get; }
        public string Path { get; }

        public LocalPluginInfo(string filepath)
        {
            var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(filepath)!;
            var infoName = Patterns.AssemblyNameRegex().Match(assemblyName.Name!);
            Id = new Guid(infoName.Groups["guid"].Value);
            Name = infoName.Groups["guid"].Value;
            Version = assemblyName.Version!;
            Path = filepath;
        }
    }
}
