using System.Text.Json.Serialization;

namespace backend
{
    public class NodeResponse
    {
        [JsonPropertyName("lidlImages")]
        public FlyerImages data { get; set; }
    }
}
