using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain
{
    public class UserRoles : IdentityUserRole<string>
    {
        public virtual AppUser User { get; set; } = null!;
        public virtual AppRole Role { get; set; } = null!;
    }
}
