using Carter;
using Catalog.Category.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.Categorys.CreatCategory
{
    internal class CreatCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/product{id}", async (Guid id, CategoryDto Dto, ISender sender) =>
            {

            });

        }
    }
}
