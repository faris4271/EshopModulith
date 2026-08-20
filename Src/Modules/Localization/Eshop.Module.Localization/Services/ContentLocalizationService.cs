using Eshop.Module.Localization.Contract.Services;
using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Globalization;
using System.Linq.Expressions;

namespace Eshop.Module.Localization.Services
{
    internal class ContentLocalizationService : IContentLocalizationService
    {
        private readonly LocalizationDbContext _DbContext;

        private readonly bool _isLocalizedConentEnable;

        private readonly HybridCache _cacheService;

        private static readonly HybridCacheEntryOptions LocalizationEntryOptions = new()
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(2),
        };

        public ContentLocalizationService(

            LocalizationDbContext dbContext,
            bool isLocalizedConentEnable, HybridCache cacheService)
        {
            _DbContext = dbContext;
            _isLocalizedConentEnable = isLocalizedConentEnable;
            _cacheService = cacheService;
        }

        public async Task<Func<Guid, string, string, string>> GetLocalizationFunction<TEntity>()
        {
            return await GetLocalizationFunction(typeof(TEntity).Name);
        }

        public async Task<Func<Guid, string, string, string>> GetLocalizationFunction(string entityType)
        {
            var localizedContentProperties = await GetLocalizationFromDb(entityType);

            Func<Guid, string, string, string> localizationFunction = (entityId, propertyName, defaultValue) =>
            {
                if (!_isLocalizedConentEnable)
                {
                    return defaultValue;
                }

                var localizedProperty = localizedContentProperties.FirstOrDefault(x => x.EntityId == entityId && x.ProperyName == propertyName);
                return localizedProperty != null ? localizedProperty.Value : defaultValue;
            };

            return localizationFunction;
        }

        public async Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, CancellationToken ct) where TEntity : global::Shared.DDD.EntityBase<Guid>
        {
            var culture = CultureInfo.CurrentCulture.Name;

            return await GetLocalizedProperty(entity, propertyName, propertyValue, culture, ct);

        }

        public async Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, string cultureId, CancellationToken ct) where TEntity : global::Shared.DDD.EntityBase<Guid>
        {

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return await GetLocalizedProperty(entity.GetType().Name, entity.Id, propertyName, propertyValue, cultureId, ct);
        }

        public async Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, CancellationToken ct)
        {
            var colure = CultureInfo.CurrentCulture.Name;

            return await GetLocalizedProperty(entityType, entityId, propertyName, propertyValue, colure, ct);
        }

        public async Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, string cultureId, CancellationToken ct)
        {
            if (!_isLocalizedConentEnable)
            {
                return propertyValue;
            }

            var state = new LocalizationFactoryState(_DbContext, cultureId, entityId, entityType);

            var localizationProperty = await _cacheService.GetOrCreateAsync(GetLocalizationCacheKey(entityId.ToString()),
                state,
              GetLocalizedPropertiesFromDbAsync,

                LocalizationEntryOptions
              );

            var localizProperty = localizationProperty.FirstOrDefault(x => x.ProperyName == propertyName);

            if (localizProperty != null)
                return localizProperty.Value;

            return propertyValue;


        }
        private async Task<List<LocalizedContentProperty>> GetLocalizationFromDb(string entityType)
        {
            return await GetLocalizationFromDb(entityType, CultureInfo.CurrentCulture.Name);
        }

        private async Task<List<LocalizedContentProperty>> GetLocalizationFromDb(
         string entityType,
         string cultureId,
         Guid entityId = default,
         CancellationToken ct = default)
        {
            if (!_isLocalizedConentEnable)
            {
                return new List<LocalizedContentProperty>();
            }

            var state = new LocalizationFactoryState
            {
                EntityType = entityType,
                CultureId = cultureId,
                EntityId = entityId
            };

            // Correctly await the async delegate execution with required state and cancellation token
            var localizedProperties = await GetLocalizedPropertiesFromDbAsync(state, ct);

            return localizedProperties;
        }

        private async ValueTask<List<LocalizedContentProperty>> GetLocalizedPropertiesFromDbAsync(
            LocalizationFactoryState state,
            CancellationToken ct)
        {
            Expression<Func<LocalizedContentProperty, bool>> expression = localizedContentProperty =>
                state.EntityId == null
                    ? localizedContentProperty.EntityType == state.EntityType
                        && localizedContentProperty.CultureId == state.CultureId
                    : localizedContentProperty.EntityId == state.EntityId
                        && localizedContentProperty.EntityType == state.EntityType
                        && localizedContentProperty.CultureId == state.CultureId;

            // Use ToListAsync to perform a non-blocking database query
            var localizedProperties = await _DbContext.Set<LocalizedContentProperty>()
                .Where(expression)
                .ToListAsync(ct);

            return localizedProperties;
        }
        public static string GetLocalizationCacheKey(string userId)
        {
            return $"loc:{userId}";
        }



        private readonly record struct LocalizationFactoryState(LocalizationDbContext DbContext, string CultureId, Guid EntityId, string EntityType);
    }
}
