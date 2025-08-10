using System.Text.Json.Serialization;

namespace backend
{
    public class FlyerImages
    {
        [JsonPropertyName("actualDate")]
        public string ActualDate { get; set; }

        [JsonPropertyName("serviceType")]
        public string serviceType { get; set; }

        [JsonPropertyName("pages")]
        public List<Flyer> Pages { get; set; }
    }
}
