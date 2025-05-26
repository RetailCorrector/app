using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetailCorrector.RegistryManager.Data;

public struct RemoteModules
{
    [JsonPropertyName("$schema")] public string Schema { get; set; }
    [JsonPropertyName("modules")] public RemoteModule[] Modules { get; set; }
}

[JsonConverter(typeof(RemoteModuleConverter))]
public struct RemoteModule
{
    [JsonConverter(typeof(JsonStringEnumConverter<ModuleType>))]
    public enum ModuleType { Fiscal, Source }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Version Version { get; set; }
    public ModuleType Type { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string File { get; set; }
    public string Hash { get; set; }
}


public class RemoteModuleConverter : JsonConverter<RemoteModule>
{
    public override RemoteModule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        var module = new RemoteModule();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            var propertyName = reader.GetString();
            reader.Read();
            switch (propertyName)
            {
                case "id":
                    module.Id = reader.GetGuid();
                    break;
                case "name":
                    module.Name = reader.GetString() ?? string.Empty;
                    break;
                case "version":
                    module.Version = Version.Parse(reader.GetString() ?? "0.0.0");
                    break;
                case "type":
                    module.Type = Enum.Parse<RemoteModule.ModuleType>(reader.GetString() ?? string.Empty, true);
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
                case "hash":
                    module.Hash = reader.GetString() ?? string.Empty;
                    break;
                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }
        return module;
    }

    public override void Write(Utf8JsonWriter writer, RemoteModule value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}