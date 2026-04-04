using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class ProductOptionCombination : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid OptionId { get; set; }

        public ProductOption Option { get; set; }

        [StringLength(450)]
        public string Value { get; set; }

        public int SortIndex { get; set; }
    }
}