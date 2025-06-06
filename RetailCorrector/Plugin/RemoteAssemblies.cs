using System.Text.Json.Serialization;

namespace RetailCorrector.Plugin;

public struct RemoteAssemblies()
{
    [JsonPropertyName("$schema")] public string Schema { get; set; }
    [JsonPropertyName("assemblies")] public RemoteAssembly[] Assemblies { get; set; } = [];
}
