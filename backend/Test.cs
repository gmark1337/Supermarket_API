using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

class Test
{
    static async Task Main()
    {
        // Replace with your real values
        string accountId = "43e8a7074b8cc770eb5ab559bf27e6f8"; // from R2 dashboard
        string bucketName = "postwall-blob-storage"; // exact match, case-sensitive
        string accessKey = "7d653a3f422b89e7df315161e32a9046"; // from R2 API token creation
        string secretKey = "30b683f0fd8996246cc0d3d2aa898cd3521ff52a865e502dda5dca4a59099545"; // from R2 API token creation

        var awsCreds = new BasicAWSCredentials(accessKey, secretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true,
            AuthenticationRegion = "auto"
        };

        using var client = new AmazonS3Client(awsCreds, config);

        var fileBytes = File.ReadAllBytes("test.jpg"); // a local file for testing
        using var stream = new MemoryStream(fileBytes);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = "test-upload.jpg",
            InputStream = stream,
            ContentType = "image/jpeg",
            CannedACL = S3CannedACL.PublicRead
        };

        var response = await client.PutObjectAsync(putRequest);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Upload successful!");
            Console.WriteLine($"Public URL: https://{bucketName}.{accountId}.r2.cloudflarestorage.com/test-upload.jpg");
        }
        else
        {
            Console.WriteLine($"❌ Upload failed: {response.HttpStatusCode}");
        }
    }
}
