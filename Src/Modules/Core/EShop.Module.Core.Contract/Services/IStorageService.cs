namespace EShop.Module.Core.Contract.Services
{
    public interface IStorageService
    {
        string GetMediaUrl(string fileName);

        Task<string> SaveMediaAsync(Stream mediaBinaryStream, string fileName, string mimeType = null);

        Task DeleteMediaAsync(string fileName);
    }
}