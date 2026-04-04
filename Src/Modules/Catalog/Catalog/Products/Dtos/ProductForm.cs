using Microsoft.AspNetCore.Http;

namespace Catalog.Products.Dtos
{
    public class ProductForm
    {
        public CreatProductDto Product { get; set; } = new CreatProductDto();

        public IFormFile ThumbnailImage { get; set; }

        public IList<IFormFile> ProductImages { get; set; } = new List<IFormFile>();

        public IList<IFormFile> ProductDocuments { get; set; } = new List<IFormFile>();
    }
}
