using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CatalogRuleCustomerGroup:EntityBase<Guid>
    {
        public Guid CatalogRuleId { get; set; }

        public CatalogRule CatalogRule { get; set; }

        public Guid CustomerGroupId { get; set; }

  
    }
}