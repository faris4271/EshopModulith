using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.GetRole;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Roles.GetRoleById;

public class GetRoleByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/roles/{id:guid}", async (string id, ISender sender, CancellationToken cancellationToken) =>
        {
           var result=await  sender.Send(new GetRoleQuery(id), cancellationToken);

            result.Match(Results.Ok, Results.NotFound);
        })
    .WithName("GetRole")
    .WithSummary("Get role by ID")
    //.RequirePermission(IdentityPermissionConstants.Roles.View)
    .WithDescription("Retrieve details of a specific role by its unique identifier.");
    }
}
