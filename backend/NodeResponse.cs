using System.Text.Json.Serialization;

namespace backend
{
    public class NodeResponse
    {
        public Dictionary<string, FlyerImages> data { get; set; }
    }
}
