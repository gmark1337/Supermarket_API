using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using backend.Model;
using MongoDB.Driver;
using System.Text.Json;

namespace backend
{
    public class BlobService
    {

        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _accountId;
        private readonly HttpClient _httpClient;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IAmazonS3 s3client, IConfiguration configuration, HttpClient httpClient, ILogger<BlobService> logger)
        {
            _s3Client = s3client;
            _bucketName = configuration["CloudFlareConnectionStrings:BucketName"];
            _accountId = configuration["CloudFlareConnectionStrings:AccountID"];
            _httpClient = httpClient;
            _logger = logger;

        }


        public async Task<string> UploadImageAsync(byte[] imageBytes, string key)
        {
            using var stream = new MemoryStream(imageBytes);


            _logger.LogInformation($"The bucket info is :{_bucketName}");
            var uploadReq = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = "image/jpeg",
                DisablePayloadSigning = true
            };


            _logger.LogInformation("Uploading image to bucket {Bucket} with key {Key}", _bucketName, key);
            var response = await _s3Client.PutObjectAsync(uploadReq);

            if(response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Failed to upload image to R2. status: {response.HttpStatusCode}");
            }

            return $"https://{_accountId}.r2.cloudflarestorage.com/{_bucketName}/{key}";
        }
        public async Task<List<Flyer>>UploadBlobIntoCloud(List<Flyer> data)
        {
            foreach(var flyer in data)
            {
                var imageBytes = await _httpClient.GetByteArrayAsync(flyer.ImageURL);

                var key =$"flyers/{flyer.SupermarketID}/{flyer.ActualDate}/{flyer.PageIndex}.jpg";

                var publicUrl = await UploadImageAsync(imageBytes, key);

            }

            return data;
        }

        public async Task<List<string>> FetchCloudFlareURLAsync(string supermarketId,  string actualDate)
        {
            var urls = new List<string>();

            var filter = $"flyers/{supermarketId}/{actualDate}";

            var req = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = filter,
            };

            ListObjectsV2Response listRes;

            do
            { 
                listRes = await _s3Client.ListObjectsV2Async(req);

                foreach (var obj in listRes.S3Objects)
                {
                    var presignReq = new GetPreSignedUrlRequest
                    {
                        BucketName = _bucketName,
                        Key = obj.Key,
                        Verb = HttpVerb.GET,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };

                    string presignedUrl = _s3Client.GetPreSignedURL(presignReq);
                    urls.Add(presignedUrl);
                } 

                req.ContinuationToken = listRes.NextContinuationToken;
            } while (listRes.IsTruncated);
            foreach(var url in urls)
            {
                _logger.LogInformation($"One url is {url}");
            }

            //_logger.LogInformation($"Retrieved data: {string.Join("\n",urls)}");
            return urls;
        }
    }
   
}
