using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class ProductOptionValue : EntityBase<Guid>
    {


        public Guid OptionId { get; set; }

        [StringLength(450)]
        public string Value { get; set; }

        public int SortIndex { get; set; }


        [StringLength(450)]
        public string DisplayType { get; set; }

        public ProductOption Option { get; set; }
        public Product Product { get; set; }

        public ProductOption ProductOption { get; set; }

        public Guid? ProductId { get; set; }
    }
}