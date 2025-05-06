using System.Text.Json.Serialization;

namespace RetailCorrector.Wizard
{
    public class RepoPackage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public RepoPackageType Type { get; set; }
        [JsonPropertyName("download")]
        public string Url { get; set; }
        [JsonPropertyName("conftip")]
        public string[] ConfigTip { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<RepoPackageType>))]
        public enum RepoPackageType
        {
            Fiscal,
            Source
        }
    }
}
