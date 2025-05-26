using System.Text.Json.Serialization;

namespace RetailCorrector.RegistryManager;

public struct RemoteModules
{
    [JsonPropertyName("$schema")] public string Schema { get; set; }
    [JsonPropertyName("modules")] public RemoteModule[] Modules { get; set; }
}

public struct RemoteModule
{
    [JsonConverter(typeof(JsonStringEnumConverter<ModuleType>))]
    public enum ModuleType { Fiscal, Source }

    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("version")] public Version Version { get; set; }
    [JsonPropertyName("type")] public ModuleType Type { get; set; }
    [JsonPropertyName("author")] public string Author { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("file")] public string File { get; set; }
    [JsonPropertyName("hash")] public string Hash { get; set; }
}
