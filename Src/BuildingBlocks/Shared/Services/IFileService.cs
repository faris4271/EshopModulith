using Microsoft.AspNetCore.Http;

namespace Shared.Services
{
    public interface IFileService
    {
        Task<List<string>> UploadFilesAsync(IFormFileCollection files, string filePath);
        Task<string> UploadFileAsync(IFormFile file, string filePath);

        void DeleteFiles(List<string> strings);
        void DeleteFile(string filePath);
    }
}
