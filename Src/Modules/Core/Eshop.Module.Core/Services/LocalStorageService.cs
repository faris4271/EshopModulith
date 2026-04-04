using EShop.Module.Core.Contract.Services;

namespace Eshop.Module.Core.Services
{
    internal class LocalStorageService : IStorageService
    {
        private const string MediaRootFoler = "user-content";


        public async Task DeleteMediaAsync(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException();

            var path = Path.Combine("wwroot", MediaRootFoler, fileName);

            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
            }
        }

        public string GetMediaUrl(string fileName)
        {
            return $"/{MediaRootFoler}/{fileName}";
        }

        public async Task<string> SaveMediaAsync(Stream mediaBinaryStream, string fileName, string mimeType = null)
        {

            var dirPath = Path.Combine("wwroot", MediaRootFoler);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var filePath = Path.Combine(dirPath, fileName);
            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await mediaBinaryStream.CopyToAsync(output);
            }
            return filePath;

        }
    }
}
