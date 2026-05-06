using Microsoft.AspNetCore.Http;

namespace CatalogContract.Dtos
{
    public class ProductForm
    {
        public CreatProductDto Product { get; set; } = new CreatProductDto();

        public IFormFile ThumbnailImage { get; set; }

        public List<IFormFile> ProductImages { get; set; } 

        public List<IFormFile> ProductDocuments { get; set; } 
    }
}
