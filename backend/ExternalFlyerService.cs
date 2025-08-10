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
                _logger.LogInformation(fullURL);
                //_logger.LogInformation($"The request is {fullURL}");
                //_logger.LogInformation($"The other request is http://localhost:3000/api/data?supermarketId={supermarketId}");
                var response = await _httpClient.GetAsync(fullURL);
                //var response2 = await _httpClient.GetAsync($"http://localhost:3000/api/data?supermarketId={supermarketId}");
                _logger.LogInformation($"The body is {response.Content.ToString()}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                //_logger.LogInformation($"Received content is {content}");
                var flyerMap = JsonSerializer.Deserialize<NodeResponse>(content);
                //_logger.LogInformation("Received information is: {@flyerMap}", flyerMap);
                

                if (flyerMap == null)
                {
                    return null;
                }
                var flyer = flyerMap.data;
                //_logger.LogInformation("Flyer data is : {@flyer}", flyer);
                //var date = flyerMap.data.ActualDate;
                var Allflyers = new List<Flyer>();
                foreach (var flyers in flyer.Pages)
                {
                        flyers.SupermarketID = supermarketId;
                        flyers.ActualDate = flyer.ActualDate;
                        flyers.ServiceType = flyer.serviceType;
                        Allflyers.Add(flyers);
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
