using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Localization.Contract.Services
{
    public interface IContentLocalizationService
    {
        Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue) where TEntity : EntityBase<Guid>;

       Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, string cultureId) where TEntity : EntityBase<Guid>;

        Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue);

        Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, string cultureId);

        Task<Func<Guid, string, string, string>> GetLocalizationFunction<TEntity>();

        Task<Func<Guid, string, string, string>> GetLocalizationFunction(string entityType);
    }
}
