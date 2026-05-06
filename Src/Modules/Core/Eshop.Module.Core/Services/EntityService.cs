using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;


namespace Eshop.Module.Core.Services
{
    internal class EntityService : IEntityService
    {
        private readonly IGenericeRepository<Entity, CoreDbContext> _repository;
        private readonly IMediator _mediator;

        public EntityService(IGenericeRepository<Entity, CoreDbContext> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<string> ToSafeSlug(string slug, Guid entityId, string entityTypeId)
        {
            var i = 2;
            while (true)
            {
                var query =await _repository.GetAllAsQuerable();
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

        public async Task<EntityDto> Get(Guid entityId)
        {
            var query = await _repository.GetAllAsQuerable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId);
            var result = entity.Adapt<EntityDto>();
            return result;
        }

        public async Task Add(string name, string slug, Guid entityId, string entityTypeId)
        {
            var entity = Entity.Creat(name, slug, entityId, entityTypeId);
            await _repository.AddAsync(entity);            // if available
            var saved = await _repository.SaveChangesAsync();
            if (saved == 0) throw new InvalidOperationException("No rows were saved.");
        }

        public async Task Update(string newName, string newSlug, Guid entityId, string entityTypeId)
        {
            var query =await _repository.GetAllAsQuerable() ;
            var entity = query.FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);

            if(entity is null)
                throw new InvalidOperationException("can not find entity");

            entity.Update(newName, newSlug, entityId, entityTypeId);

              _repository.Update(entity);

            await _repository.SaveChangesAsync();

        }

        public async Task Remove(Guid entityId, string entityTypeId)
        {
            var query =await _repository.GetAllAsQuerable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            _repository.Delete(entity);
            //if (entity != null)
            //{
            //    await _mediator.Publish(new EntityDeleting { EntityId = entity.Id });
            //    _entityRepository.Remove(entity);
            //}
            await _repository.SaveChangesAsync();
        }

        
    }
}
