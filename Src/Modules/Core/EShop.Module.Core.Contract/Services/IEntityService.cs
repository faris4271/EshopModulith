using EShop.Module.Core.Contract.Dtos;

namespace EShop.Module.Core.Contract.Services
{
    public interface IEntityService
    {
        Task<string> ToSafeSlug(string slug, Guid entityId, string entityTypeId);

        Task<EntityDto> Get(Guid entityId);

        Task Add(string name, string slug, Guid entityId, string entityTypeId);

        Task Update(string newName, string newSlug, Guid entityId, string entityTypeId);

        Task Remove(Guid entityId, string entityTypeId);
    }
}
