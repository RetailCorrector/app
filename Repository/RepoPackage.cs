using System.Text.Json.Nodes;

namespace RetailCorrector.Wizard.Repository
{
    public abstract class RepoPackage(JsonNode node)
    {
        public string Name { get; set; } = node["name"]!.GetValue<string>();
        public Uri Uri { get; set; } = new Uri(node["uri"]!.GetValue<string>());

        public static RepoPackage[] Parse(JsonArray arr)
        {
            var res = new RepoPackage[arr.Count];
            for(var i = 0; i < arr.Count; i++)
            {
                var node = arr[i]!;
                res[i] = node["type"]!.GetValue<string>() switch
                {
                    "Fiscal" => new FiscalPackage(node),
                    "Source" => new SourcePackage(node),
                    _ => throw new ArgumentException("Тип интеграции не определён"),
                };
            }
            return res;
        }
    }
}
