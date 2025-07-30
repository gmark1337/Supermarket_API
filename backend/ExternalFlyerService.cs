using System.Net;
using System.Text.Json;

namespace backend
{
    public class ExternalFlyerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalFlyerService> _logger;
        public ExternalFlyerService(HttpClient httpClient, ILogger<ExternalFlyerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<List<Flyer>> FetchFromNodeApi(string supermarketId)
        {
            try { 
                var respone = await _httpClient.GetAsync($"http://localhost:3000/api/data?supermarketId={supermarketId}");
                if (!respone.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await respone.Content.ReadAsStringAsync();
                var flyerMap = JsonSerializer.Deserialize<Dictionary<string, FlyerImages>>(content);

                if (flyerMap == null || flyerMap.Count == 0)
                {
                    return null;
                }
                var Allflyers = new List<Flyer>();
                foreach (var flyers in flyerMap)
                {
                    foreach (var flyer in flyers.Value.Pages)
                    {
                        flyer.SupermarketID = supermarketId;
                        flyer.ActualDate = flyers.Value.ActualDate;
                        Allflyers.Add(flyer);
                    }
                }
                return Allflyers;
            }catch(Exception ex)
            {
                _logger.LogInformation($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}
