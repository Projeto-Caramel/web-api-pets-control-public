namespace Caramel.Pattern.Services.Domain.Services
{
    public interface IBucketService
    {
        Task<string> UploadFileAsync(string base64Image, string key);
        Task<List<string>> GetImagesForEntityAsync(string key);
        Task DeleteFileAsync(string key);
        Task<bool> ImageExistsAsync(string key);
        Task<string> GetImageAsBase64Async(string key);
    }
}
