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
        private readonly CoreDbContext _context;
        private readonly IMediator _mediator;

        public EntityService(CoreDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<string> ToSafeSlug(string slug, Guid entityId, string entityTypeId)
        {
            var i = 2;
            while (true)
            {
                var entity =  _context.entities.AsQueryable()
               .FirstOrDefault(x => x.Slug == slug);

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
            var query = _context.entities.AsQueryable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId);
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

            _context.entities.Add(entity);
        }

        public async Task Update(string newName, string newSlug, Guid entityId, string entityTypeId)
        {
            var query = _context.entities.AsQueryable();
            var entity = query.First(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            entity.Name = newName;
            entity.Slug = newSlug;

        }

        public async Task Remove(Guid entityId, string entityTypeId)
        {
            var query = _context.entities.AsQueryable();
            var entity = query.FirstOrDefault(x => x.EntityId == entityId && x.EntityTypeId == entityTypeId);
            _context.entities.Remove(entity);
            //if (entity != null)
            //{
            //    await _mediator.Publish(new EntityDeleting { EntityId = entity.Id });
            //    _entityRepository.Remove(entity);
            //}
        }






    }
}
