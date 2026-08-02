using Shared.DDD;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Products.Models
{
    public class ProductPriceHistory : EntityBase<Guid>
    {
        public ProductPriceHistory()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        public Product Product { get; set; }

        public Guid CreatedById { get; set; }



        public DateTimeOffset CreatedOn { get; set; }

        public Money? Price { get; set; }

        public Money? OldPrice { get; set; }

        public Money? SpecialPrice { get; set; }

        public DateTimeOffset? SpecialPriceStart { get; set; }

        public DateTimeOffset? SpecialPriceEnd { get; set; }

        [NotMapped]
        public bool IsPriceChange { get; set; }
    }
}