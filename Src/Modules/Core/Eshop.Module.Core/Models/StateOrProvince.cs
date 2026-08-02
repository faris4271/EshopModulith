using Shared.DDD;

namespace Eshop.Module.Core.Models
{
    public class StateOrProvince : EntityBase<Guid>
    {
        public Guid CountryId { get; set; }

        public Country Country { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}