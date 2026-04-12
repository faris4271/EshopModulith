using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Dtos
{
    public class UserRoleDto
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool Enabled { get; set; }
    }
}
