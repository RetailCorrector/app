using System.Text.Json.Nodes;

namespace RetailCorrector.Wizard.Repository
{
    public class SourcePackage: RepoPackage
    {
        public StringsPair[] Properties { get; set; }

        public SourcePackage(JsonNode node) : base(node)
        {
            var arr = node["properties"]!.AsArray();
            Properties = new StringsPair[arr.Count];
            for (var i = 0; i < Properties.Length; i++)
            {
                var name = arr[i]!["name"]!.GetValue<string>();
                var tooltip = arr[i]!["tooltip"]!.GetValue<string>();
                Properties[i] = new StringsPair(name, tooltip);
            }
        }
    }
}
