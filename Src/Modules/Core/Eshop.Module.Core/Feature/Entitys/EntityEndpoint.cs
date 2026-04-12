using Carter;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Eshop.Module.Core.Feature.Entitys
{
    internal class EntityEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/slug-resolver/{slug}", async (string slug,IEntityService service) =>
            {

                var entity = await service.Get(Guid.Parse(slug));

                if (entity == null) 
                    return Results.NotFound();

                return Results.Ok(new
                {
                    entityId = entity.EntityId,
                    entityTypeId = entity.EntityTypeId, 
                    apiUrl = $"/api/{entity.EntityTypeId.ToLower()}s/{entity.EntityId}"
                });


            });
        }
    }
}
