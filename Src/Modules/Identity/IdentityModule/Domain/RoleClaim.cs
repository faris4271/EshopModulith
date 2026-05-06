using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Domain
{
    public class RoleClaim:IdentityRoleClaim<string>
    {
        public string? CreatedBy { get; init; }
        public DateTimeOffset CreatedOn { get; init; }
    }
}
