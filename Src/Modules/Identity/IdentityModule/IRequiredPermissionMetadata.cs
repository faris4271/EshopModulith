namespace IdentityModule;

public interface IRequiredPermissionMetadata
{
    HashSet<string> RequiredPermissions { get; }
}
