using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTaxList
{
    internal class GetTaxClassListEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tax/classes", async (ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetTaxClassListQuery(), ct);

                return result.Match(Results.Ok, Results.NotFound);
            }).WithTags("Tax");
        }
    }
}
