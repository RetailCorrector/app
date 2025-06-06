using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetailCorrector.Plugin;

public class RemoteAssemblyConverter: JsonConverter<RemoteAssembly>
{
    public override RemoteAssembly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();
        var assembly = new RemoteAssembly();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) break;
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            var propName = reader.GetString();
            reader.Read();
            switch (propName)
            {
                case "id":
                    assembly.Info = new AssemblyInfo(assembly.Info, id: reader.GetString());
                    break;
                case "name":
                    assembly.Info = new AssemblyInfo(assembly.Info, name: reader.GetString());
                    break;
                case "version":
                    assembly.Info = new AssemblyInfo(assembly.Info, ver: reader.GetString());
                    break;
                case "author":
                    assembly.Info = new AssemblyInfo(assembly.Info, author: reader.GetString());
                    break;
                case "description":
                    assembly.Info = new AssemblyInfo(assembly.Info, desc: reader.GetString());
                    break;
                case "file":
                    assembly.Download = reader.GetString()!;
                    break;
                default: break;
            }
        }
        return assembly;
    }

    public override void Write(Utf8JsonWriter writer, RemoteAssembly value, JsonSerializerOptions options) => 
        throw new NotImplementedException();
}
