namespace Shared.DDD;

/// <summary>
/// Defines audit metadata for an entity.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets the UTC timestamp when the entity was created.
    /// </summary>
    DateTimeOffset CreatedOn { get; }

    /// <summary>
    /// Gets the identifier of the creator.
    /// </summary>
    string? CreatedById { get; }

    /// <summary>
    /// Gets the UTC timestamp when the entity was last modified.
    /// </summary>
    DateTimeOffset? LatestUpdatedOn { get; }

    /// <summary>
    /// Gets the identifier of the last modifier.
    /// </summary>
    string? LatestUpdatedById { get; }
}
