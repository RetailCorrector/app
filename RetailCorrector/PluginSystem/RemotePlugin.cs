using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetailCorrector.PluginSystem;

public struct RemotePlugins
{
    [JsonPropertyName("$schema")] public string Schema { get; set; }
    [JsonPropertyName("plugins")] public RemotePlugin[] Plugins { get; set; }
}

[JsonConverter(typeof(RemotePluginConverter))]
public struct RemotePlugin
{
    [JsonConverter(typeof(JsonStringEnumConverter<PluginType>))]
    public enum PluginType { Fiscal, Source, Pack }

    public string Name { get; set; }
    public Version Version { get; set; }
    public PluginType Type { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string File { get; set; }
}


public class RemotePluginConverter : JsonConverter<RemotePlugin>
{
    public override RemotePlugin Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        var module = new RemotePlugin();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            var propertyName = reader.GetString();
            reader.Read();
            switch (propertyName)
            {
                case "name":
                    module.Name = reader.GetString() ?? string.Empty;
                    break;
                case "version":
                    module.Version = Version.Parse(reader.GetString() ?? "0.0.0");
                    break;
                case "type":
                    module.Type = Enum.Parse<RemotePlugin.PluginType>(reader.GetString() ?? string.Empty, true);
                    break;
                case "author":
                    module.Author = reader.GetString() ?? string.Empty;
                    break;
                case "description":
                    module.Description = reader.GetString() ?? string.Empty;
                    break;
                case "file":
                    module.File = reader.GetString() ?? string.Empty;
                    break;
                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }
        return module;
    }

    public override void Write(Utf8JsonWriter writer, RemotePlugin value, JsonSerializerOptions options) => 
        throw new NotImplementedException();
}