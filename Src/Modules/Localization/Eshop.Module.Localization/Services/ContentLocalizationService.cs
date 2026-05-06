using Eshop.Module.Localization.Contract.Services;
using Eshop.Module.Localization.Data;
using Eshop.Module.Localization.Models;
using Shared.Abstraction;
using Shared.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace Eshop.Module.Localization.Services
{
    internal class ContentLocalizationService : IContentLocalizationService
    {
        private readonly IGenericeRepository<LocalizedContentProperty,LocalizationDbContext> _entityRepository;

        private readonly bool _isLocalizedConentEnable;

        private readonly ICacheService _cacheService;

        public ContentLocalizationService(
            IGenericeRepository<LocalizedContentProperty, 
            LocalizationDbContext> entityRepository,
            bool isLocalizedConentEnable, ICacheService cacheService)
        {
            _entityRepository = entityRepository;
            _isLocalizedConentEnable = isLocalizedConentEnable;
            _cacheService = cacheService;
        }

        public async Task<Func<Guid, string, string, string>> GetLocalizationFunction<TEntity>()
        {
            return await GetLocalizationFunction(typeof(TEntity).Name);
        }

        public async Task<Func<Guid, string, string, string>> GetLocalizationFunction(string entityType)
        {
            var localizedContentProperties =await GetLocalizationFromDb(entityType);

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

        public async Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue) where TEntity : global::Shared.DDD.EntityBase<Guid>
        {
            var culture=CultureInfo.CurrentCulture.Name;

           return await GetLocalizedProperty(entity, propertyName, propertyValue, culture);

        }

        public async Task<string> GetLocalizedProperty<TEntity>(TEntity entity, string propertyName, string propertyValue, string cultureId) where TEntity : global::Shared.DDD.EntityBase<Guid>
        {
           
            if(entity == null) throw new ArgumentNullException(nameof(entity));

          return await GetLocalizedProperty(entity.GetType().Name, entity.Id,propertyName, propertyValue,cultureId);
        }

        public async Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue)
        {
            var colure = CultureInfo.CurrentCulture.Name;

           return await  GetLocalizedProperty(entityType, entityId, propertyName, propertyValue, colure);
        }

        public async Task<string> GetLocalizedProperty(string entityType, Guid entityId, string propertyName, string propertyValue, string cultureId)
        {
            if (!_isLocalizedConentEnable)
            {
                return propertyValue;
            }

            var localizationProperty =await _cacheService.GetOrSetAsync(GetLocalizationCacheKey(entityId.ToString()), async () =>
            {

             var localiz= await GetLocalizedPropertiesFromDbAsync(entityType, entityId,cultureId);
                return localiz;

            });    

           var localizProperty= localizationProperty.FirstOrDefault(x=>x.ProperyName==propertyName);

            if(localizProperty != null) 
                return localizProperty.Value;

            return propertyValue;


        }
        private async Task<List<LocalizedContentProperty>> GetLocalizationFromDb(string entityType)
        {
            return await GetLocalizationFromDb(entityType, CultureInfo.CurrentCulture.Name);
        }

        private async Task<List<LocalizedContentProperty>> GetLocalizationFromDb(string entityType, string cultureId)
        {
            if (!_isLocalizedConentEnable)
            {
                return new List<LocalizedContentProperty>();
            }

            var localizedProperties = await GetLocalizedPropertiesFromDbAsync(entityType, null, cultureId);

            return  localizedProperties;
        }
        private async Task<List<LocalizedContentProperty>> GetLocalizedPropertiesFromDbAsync(string entityType, Guid? entityId, string cultureId)
        {
            Expression<Func<LocalizedContentProperty, bool>> expression = localizedContentProperty => entityId == null
            ? localizedContentProperty.EntityType == entityType
                && localizedContentProperty.CultureId == cultureId
            : localizedContentProperty.EntityId == entityId
                && localizedContentProperty.EntityType == entityType
                && localizedContentProperty.CultureId == cultureId;

            var query = await _entityRepository.Query();

            var localizedProperties = query.Where(expression).ToList();
            return localizedProperties;
        }

        public static string GetLocalizationCacheKey(string userId)
        {
            return $"loc:{userId}";
        }
    }
}
