using MongoDB.Bson;
using System.Text.Json;

namespace backend
{
    public class ExternalPdfService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalPdfService> _logger;

        public ExternalPdfService(HttpClient httpClient, ILogger<ExternalPdfService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<FlyerPDF> FetchPdfFromNodeApi(string supermarketId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:3000/api/pdf?supermarketId={supermarketId}");
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
