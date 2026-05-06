using System.ComponentModel.DataAnnotations;
using Shared.DDD;

namespace Eshop.Module.Basket.Models
{
    public class CatalogRule : EntityBase<Guid>
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? StartOn { get; set; }

        public DateTimeOffset? EndOn { get; set; }

        [StringLength(450)]
        public string RuleToApply { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public IList<CatalogRuleCustomerGroup> CustomerGroups { get; set; } = new List<CatalogRuleCustomerGroup>();
    }
}
