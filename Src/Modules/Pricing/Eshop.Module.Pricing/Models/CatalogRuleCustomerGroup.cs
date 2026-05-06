using Eshop.Module.Pricing.Models;
using Shared.DDD;

namespace SimplCommerce.Module.Pricing.Models
{
    public class CatalogRuleCustomerGroup:EntityBase<Guid>
    {
        public Guid CatalogRuleId { get; set; }

        public CatalogRule CatalogRule { get; set; }

        public Guid CustomerGroupId { get; set; }

  
    }
}