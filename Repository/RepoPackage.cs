using System.Text.Json.Nodes;

namespace RetailCorrector.Wizard.Repository
{
    public class RepoPackage(JsonNode node)
    {
        public string Guid { get; set; } = node["id"]!.GetValue<string>();
        public string Name { get; set; } = node["name"]!.GetValue<string>();
        public string Description { get; set; } = node["description"]!.GetValue<string>();
        public Uri Uri { get; set; } = new Uri(node["uri"]!.GetValue<string>());
        public string HashSum { get; set; } = node["hash"]!.GetValue<string>();
        public Version Version { get; set; } = Version.Parse(node["version"]!.GetValue<string>());

        public static RepoPackage[] Parse(JsonArray arr)
        {
            try
            {
                var res = new RepoPackage[arr.Count];
                for (var i = 0; i < arr.Count; i++)
                {
                    var node = arr[i]!;
                    var type = node["type"]!.GetValue<string>();
                    res[i] = node["type"]!.GetValue<string>().ToLower() switch
                    {
                        "fiscal" => new FiscalPackage(node),
                        "source" => new RepoPackage(node),
                        _ => throw new ArgumentException($"Тип интеграции ({type}) не определён"),
                    };
                }
                return res;
            }
            catch
            {
                return [];
            }
        }
    }
}
