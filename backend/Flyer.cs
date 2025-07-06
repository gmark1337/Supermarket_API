using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend
{
    public class Flyer
    {
        public string flyerID { get; set; } = Guid.NewGuid().ToString();

        public int SupermarketID { get; set; }

        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set;  }

        [JsonPropertyName("url")]
        public string ImageURL { get; set; }

        [JsonIgnore]
        public Supermarkets Supermarkets { get; set; }

    }
}
