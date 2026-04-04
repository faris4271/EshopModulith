using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Shared.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileService(IWebHostEnvironment environment)
        {

        }
        public void DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            var fullPath = Path.Combine(_environment.WebRootPath, path.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        public void DeleteFiles(List<string> paths)
        {
            if (paths == null) return;
            foreach (var path in paths)
            {
                DeleteFile(path);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null && file.Length == 0) return null;

            var folderPath = Path.Combine(_environment.WebRootPath, "images", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/images/{folderName}/{fileName}";
        }

        public async Task<List<string>> UploadFilesAsync(IFormFileCollection files, string filePath)
        {
            if (files == null && !files.Any()) return null;

            var ImagePaths = new List<string>();

            foreach (var file in files)
            {
                var imag = await UploadFileAsync(file, filePath);
                ImagePaths.Add(imag);
            }
            return ImagePaths;
        }
    }
}
