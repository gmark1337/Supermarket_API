using System.Net;
using System.Text.Json;

namespace backend
{
    public class ExternalFlyerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalFlyerService> _logger;

        private readonly IConfiguration _config; 
        public ExternalFlyerService(HttpClient httpClient, ILogger<ExternalFlyerService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }


        public async Task<List<Flyer>> FetchFromNodeApi(string supermarketId)
        {

            
            try {
                var baseurl = _config["ConnectionStrings:NodeJs_API"];
                var endpoint = _config["API_endpoints:getImageURL"];
                var fullURL = $"{baseurl}{endpoint}?supermarketId={supermarketId}";
                //_logger.LogInformation($"The request is {fullURL}");
                //_logger.LogInformation($"The other request is http://localhost:3000/api/data?supermarketId={supermarketId}");
                var response = await _httpClient.GetAsync(fullURL);
                //var response2 = await _httpClient.GetAsync($"http://localhost:3000/api/data?supermarketId={supermarketId}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
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
