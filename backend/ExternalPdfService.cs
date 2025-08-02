using MongoDB.Bson;
using System.Text.Json;

namespace backend
{
    public class ExternalPdfService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalPdfService> _logger;
        private readonly IConfiguration _config;

        public ExternalPdfService(HttpClient httpClient, ILogger<ExternalPdfService> logger , IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }
        public async Task<FlyerPDF> FetchPdfFromNodeApi(string supermarketId)
        {
            try
            {
                var baseURL = _config["ConnectionStrings:NodeJs_API"];
                var endpoint = _config["API_endpoints:getPDFURL"];
                var fullURL = $"{baseURL}{endpoint}?supermarketId={supermarketId}";
                var response = await _httpClient.GetAsync(fullURL);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();

                var jsonForm = JsonSerializer.Deserialize<FlyerPDF>(content);

                return jsonForm;
            }catch(Exception ex)
            {
                _logger.LogInformation($"Unexpected error... \n{ex.Message} ");
                return null;
            }

        }
    }
}
