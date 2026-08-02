using Shared.DDD;

namespace IdentityModule.Domain
{
    public class UserAddress : EntityBase<Guid>
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public Guid AddressId { get; set; }

        public AddressType AddressType { get; set; }

        public DateTimeOffset? LastUsedOn { get; set; }
    }
}
