namespace Caramel.Pattern.Services.Domain.Services
{
    public interface IBucketService
    {
        Task<string> UploadFileAsync(string base64Image, string key);
        Task<List<string>> GetImagesForEntityAsync(string key);
        Task DeleteFileAsync(string key);
    }
}
