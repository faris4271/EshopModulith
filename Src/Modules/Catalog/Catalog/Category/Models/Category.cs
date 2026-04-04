using Catalog.Products.Models;
using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Category.Models
{
    public class Category : EntityBase<Guid>
    {
        public Category(Name name, string metaTitle,
            string metaKeywords, string metaDescription,
            Description description, int displayOrder, bool isPublished,
             Guid? parentId
           )
        {
            Id = Guid.NewGuid();
            Name = name;
            MetaTitle = metaTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
            Description = description;
            DisplayOrder = displayOrder;
            IsPublished = isPublished;
            ParentId = parentId;

        }
        public Name Name { get; private set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Slug { get; private set; }

        [StringLength(450)]
        public string MetaTitle { get; private set; }

        [StringLength(450)]
        public string MetaKeywords { get; private set; }

        public string MetaDescription { get; private set; }

        public Description Description { get; private set; }

        public int DisplayOrder { get; private set; }

        public bool IsPublished { get; private set; }

        public bool IncludeInMenu { get; private set; }

        public bool IsDeleted { get; private set; }

        public Guid? ParentId { get; private set; }

        public Category Parent { get; private set; }

        public IList<Category> Children { get; private set; } = new List<Category>();
        public IList<ProductCategory> Products { get; private set; } = new List<ProductCategory>();

        public Guid ThumbnailImageId { get; private set; }

        public void AddSafeSluge(string sluge)
        {
            Slug = sluge;
        }

        public void AddMediaId(Guid guid)
        {
            ThumbnailImageId = guid;
        }

        public void Update(string name, string metaTitle, string sluge,
            string metaKeywords, string metaDescription,
            string description, int displayOrder, bool isPublished,
             Guid? parentId)
        {
            Id = Guid.NewGuid();
            Name = new Name(name);
            Slug = sluge;
            MetaTitle = metaTitle;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
            Description = new Description(description);
            DisplayOrder = displayOrder;
            IsPublished = isPublished;
            ParentId = parentId;
        }

    }
}
