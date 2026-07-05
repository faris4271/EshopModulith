using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }

        public AppRole(string name, string description) : base(name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            NormalizedName = name.ToUpperInvariant();
            Description = description;
        }

        public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
    }
}
