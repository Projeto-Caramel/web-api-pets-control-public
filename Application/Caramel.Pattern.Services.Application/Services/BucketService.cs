using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Caramel.Pattern.Services.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class BucketService : IBucketService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public BucketService(IConfiguration configuration)
        {
            var accessKey = configuration["BucketSettings:AccessKey"];
            var secretKey = configuration["BucketSettings:SecretKey"];
            _bucketName = configuration["BucketSettings:BucketName"];

            _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.SAEast1);
        }

        public async Task<string> UploadFileAsync(string base64Image, string key)
        {
            byte[] imageData = Convert.FromBase64String(base64Image);

            using var memoryStream = new MemoryStream(imageData);

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = memoryStream,
                ContentType = "image/jpeg"
            };

            var response = await _s3Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return GetImageUrl(key);
            else
                throw new Exception("Falha ao salvar a imagem na Base.");
        }

        public async Task<List<string>> GetImagesForEntityAsync(string key)
        {
            var imageUrls = new List<string>();

            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = $"{key}"
                };

                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);

                    var imageKeys = response.S3Objects
                        .Where(o => o.Key.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                    o.Key.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                    o.Key.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        .Select(o => GetImageUrl(o.Key));

                    imageUrls.AddRange(imageKeys);
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                return imageUrls;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving images: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteFileAsync(string key)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    Console.WriteLine($"File deleted successfully: {key}");
                else
                    throw new Exception("Failed to delete file from S3.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                throw;
            }
        }

        private string GetImageUrl(string key)
        {
            return $"https://{_bucketName}.s3.sa-east-1.amazonaws.com/{key}";
        }
    }
}
