using Shared.Identity;

namespace MultiTenancyModule.Authorization;

public static class MultiTenancyPermissions
{
    public static class Tenants
    {
        public const string View = "Permissions.Tenants.View";
        public const string Create = "Permissions.Tenants.Create";
        public const string Update = "Permissions.Tenants.Update";
        public const string Delete = "Permissions.Tenants.Delete";
        public const string Activate = "Permissions.Tenants.Activate";
        public const string Deactivate = "Permissions.Tenants.Deactivate";
    }

    public static class TenantSettings
    {
        public const string View = "Permissions.TenantSettings.View";
        public const string Update = "Permissions.TenantSettings.Update";
    }

    public static readonly AppPermission[] All =
    [
        new("View Tenants", "View", "Tenants", IsBasic: true),
        new("Create Tenants", "Create", "Tenants", IsRoot: true),
        new("Update Tenants", "Update", "Tenants", IsRoot: true),
        new("Delete Tenants", "Delete", "Tenants", IsRoot: true),
        new("Activate Tenants", "Activate", "Tenants", IsRoot: true),
        new("Deactivate Tenants", "Deactivate", "Tenants", IsRoot: true),
        new("View Tenant Settings", "View", "TenantSettings", IsBasic: true),
        new("Update Tenant Settings", "Update", "TenantSettings"),
    ];
}