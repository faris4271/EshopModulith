using Shared.DDD;

namespace Eshop.Module.Localization.Contract.Services
{
    public interface IContentLocalizationService
    {
        Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, CancellationToken ct) where TEntity : EntityBase<Guid>;

        Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, string cultureId, CancellationToken ct) where TEntity : EntityBase<Guid>;

        Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, CancellationToken ct);

        Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, string cultureId, CancellationToken ct);

        Task<Func<Guid, string, string, string>> GetLocalizationFunction<TEntity>();

        Task<Func<Guid, string, string, string>> GetLocalizationFunction(string entityType);
    }
}
