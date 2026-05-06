using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Roles.UpsertRole;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Roles.UpsertRole;

public  class CreateOrUpdateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapPost("/roles", async (ISender sender, [FromBody] UpsertRoleCommand request, CancellationToken cancellationToken) =>
    {
        var result = await sender.Send(request, cancellationToken);
        return result.Match(Results.Ok, Results.BadRequest);
    })
 .WithName("CreateOrUpdateRole")
 .WithSummary("Create or update role")
 //.RequirePermission(IdentityPermissionConstants.Roles.Create)
 .WithDescription("Create a new role or update an existing role's name and description.");
    }
}
