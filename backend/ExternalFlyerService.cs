using System.Text.Json;

namespace backend
{
    public class ExternalFlyerService
    {
        private readonly HttpClient _httpClient;

        public ExternalFlyerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<Flyer>> FetchFromNodeApi(string supermarketId)
        {
            var respone = await _httpClient.GetAsync($"http://localhost:3000/api/data?supermarketId={supermarketId}");
            if (!respone.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await respone.Content.ReadAsStringAsync();
            var flyerMap = JsonSerializer.Deserialize<Dictionary<string, FlyerImages>>(content);

            if(flyerMap == null || flyerMap.Count == 0)
            {
                return null;
            }
            var Allflyers = new List<Flyer>();
            foreach(var flyers in flyerMap)
            {
                foreach(var flyer in flyers.Value.Pages)
                {
                    flyer.SupermarketID = supermarketId;
                    flyer.ActualDate = flyers.Value.ActualDate;
                    Allflyers.Add(flyer);
                }
            }
            return Allflyers;
        }
    }
}
