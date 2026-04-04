using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Services;
using Mapster;
using MediatR;
using Shared.Abstraction;

namespace Eshop.Module.Core.Services
{
    internal class EntityService : IEntityService
    {
        private readonly IGenericeRepository<Entity, CoreDbContext> _entityRepository;
        private readonly IMediator _mediator;

        public EntityService(IGenericeRepository<Entity, CoreDbContext> entityRepository, IMediator mediator)
        {
            _entityRepository = entityRepository;
            _mediator = mediator;
        }

        public async Task<string> ToSafeSlug(string slug, Guid entityId, string entityTypeId)
        {
            var i = 2;
            while (true)
            {
                var query = await _entityRepository.GetAllAsQuerable();
                var entity = query.FirstOrDefault(x => x.Slug == slug);

                if (entity != null && !(entity.EntityId == entityId && entity.EntityTypeId == entityTypeId))
                {
                    slug = string.Format("{0}-{1}", slug, i);
                    i++;
                }
                else
                {
                    break;
                }
            }

            return slug;
        }

        public async Task<EntityDto> Get(Guid entityId, string entityTypeId)
        {
            var query = await _entityRepository.GetAllAsQuerable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            var result = entity.Adapt<EntityDto>();
            return result;
        }

        public void Add(string name, string slug, Guid entityId, string entityTypeId)
        {
            var entity = new Entity
            {
                Name = name,
                Slug = slug,
                EntityId = entityId,
                EntityTypeId = entityTypeId
            };

            _entityRepository.Add(entity);
        }

        public async Task Update(string newName, string newSlug, Guid entityId, string entityTypeId)
        {
            var query = await _entityRepository.GetAllAsQuerable();
            var entity = query.First(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            entity.Name = newName;
            entity.Slug = newSlug;

        }

        public async Task Remove(Guid entityId, string entityTypeId)
        {
            var query = await _entityRepository.GetAllAsQuerable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            _entityRepository.Delete(entity);
            //if (entity != null)
            //{
            //    await _mediator.Publish(new EntityDeleting { EntityId = entity.Id });
            //    _entityRepository.Remove(entity);
            //}
        }






    }
}
