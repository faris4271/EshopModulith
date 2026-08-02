using Shared.DDD;

namespace Module.Tax.Models
{
    public class TaxClass : EntityBase<Guid>
    {
        public Name Name { get; set; }
    }
}
