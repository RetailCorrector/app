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
        [JsonPropertyName("typepath")]
        public string EndpointPath { get; set; }
        [JsonPropertyName("conftip")]
        public string[] ConfigTip { get; set; }
        [JsonPropertyName("depends")]
        public RepoDepend[] Depends { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<RepoPackageType>))]
        public enum RepoPackageType
        {
            Fiscal,
            Source
        }

        public class RepoDepend
        {
            [JsonPropertyName("filename")]
            public string FileName { get; set; }
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }
    }
}
