using System.Text.Json.Nodes;

namespace RetailCorrector.Wizard.Repository
{
    public class FiscalPackage: RepoPackage
    {
        public string[] Tooltip { get; set; }

        public FiscalPackage(JsonNode node) : base(node)
        {
            var arr = node["tooltip"]!.AsArray();
            Tooltip = new string[arr.Count];
            for (var i = 0; i < Tooltip.Length; i++)
                Tooltip[i] = arr[i]!.GetValue<string>();
        }
    }
}
