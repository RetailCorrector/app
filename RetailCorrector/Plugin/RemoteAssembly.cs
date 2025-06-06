using System.Text.Json.Serialization;

namespace RetailCorrector.Plugin;

[JsonConverter(typeof(RemoteAssemblyConverter))]
public struct RemoteAssembly()
{
    public AssemblyInfo Info { get; set; } = new();
    public string Download { get; set; } = "";
}