using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class ProductOption : EntityBase<Guid>
    {

        public ProductOption(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; private set; }



        public ICollection<ProductOptionValue> Values { get; private set; }

        public void AddValues(ProductOptionValue value)
        {
            this.Values.Add(value);
        }

        public void Update(string name)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name, nameof(name));

            this.Name = name;
        }


    }
}
