using Catalog.Products.Models;
using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Brands.Moddels
{
    public class Brand : EntityBase<Guid>
    {
        public Brand(string name, string description, bool isPublished, bool isDeleted)
        {
            Id= Guid.NewGuid();
            Name = name;
            Description = description;
            IsPublished = isPublished;
            IsDeleted = isDeleted;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get;private set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Slug { get;private set; }

        public string Description { get;private set; }

        public bool IsPublished { get;private set; }

        public bool IsDeleted { get;private set; }

        public ICollection<Product> Products { get; private set; } = new List<Product>();

        public  void AddSafeSloge(string sloge)
        {
            Slug= sloge;
        }


    }
}