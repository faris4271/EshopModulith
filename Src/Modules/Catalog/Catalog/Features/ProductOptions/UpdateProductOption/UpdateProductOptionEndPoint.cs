using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Features.ProductOptions.UpdateProductOption
{
    internal class UpdateProductOptionEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/product-options/{Id}", async ([FromServices] ISender sender, Guid Id, [FromBody] UpdateProductOptionCommand command, CancellationToken cancellationToken) =>
            {
                if (Id != command.ProductOption.Id)
                    return Results.BadRequest();
                var result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, Results.BadRequest);
            }).WithTags("ProductOptions").AllowAnonymous();
        }
    }
}
