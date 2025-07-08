using System.Text.Json.Serialization;

namespace backend
{
    public class NodeResponse
    {
        //In the Dictionary the id is the SupermarketId!!
        //Lidl == 1
        //Spar == 2
        //Penny == 3 
        public Dictionary<string, FlyerImages> data { get; set; }
    }
}
