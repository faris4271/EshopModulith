using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Category.Dtos
{
    public record CategoryDto
    {


        public CategoryDto()
        {
            IsPublished = true;
        }

      

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public int DisplayOrder { get; set; }

       
        public Guid? ParentId { get; set; }

        public bool IncludeInMenu { get; set; }

        public bool IsPublished { get; set; }

        public IFormFileCollection ThumbnailImages { get; set; }

       
    }
}
