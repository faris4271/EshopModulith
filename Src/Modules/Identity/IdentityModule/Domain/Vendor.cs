using Shared.DDD;

namespace IdentityModule.Domain
{
    public class Vendor : EntityBase<Guid>, IAuditableEntity
    {

        public Name Name { get; private set; }

        public string Slug { get; private set; }

        public Description Description { get; private set; }

        public Email Email { get; private set; }


        public bool IsActive { get; private set; }

        public bool IsDeleted { get; private set; }

        public IList<AppUser> Users { get; private set; } = new List<AppUser>();

        public DateTimeOffset CreatedOn { get; private set; }

        public string? CreatedById { get; private set; }

        public DateTimeOffset? LatestUpdatedOn { get; private set; }

        public string? LatestUpdatedById { get; private set; }

        public static Vendor Create(string name, string description, string email, bool isActive)
        {
            return new Vendor
            {
                Id = Guid.NewGuid(),
                Name = new Name(name),

                Description = new Description(description),
                Email = new Email(email),
                IsActive = isActive,
                IsDeleted = false,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }


        public void AddSafeSlug(string slug)
        {
            Slug = slug;
        }
    }
}
