using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }

        public Role(string name, string description):base(name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            NormalizedName=name.ToUpperInvariant();
            Description = description;
        }
    }
}
