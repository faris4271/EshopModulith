using System.ComponentModel.DataAnnotations;

namespace CatalogContract.Dtos
{
    public class ProductTempletDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public IList<ProductAttributeDto> Attributes { get; set; } = new List<ProductAttributeDto>();
    }
}
