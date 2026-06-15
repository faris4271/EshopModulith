using System.ComponentModel.DataAnnotations;

namespace CatalogContract.Dtos
{
    public class CreateProductAttributeDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Attribute Group field is required.")]
        public Guid GroupId { get; set; }

        public string GroupName { get; set; }
    }
}
