using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Feature.Users.GetUser;
using Shared.Contract.ResultPattern;
using System.Net;

namespace IdentityModule.Feature.Users.GetUserById;

public  class GetUserByIdEndpoint : ICarterModule 
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/users/{id:guid}", async (string id, ISender sender, CancellationToken cancellationToken) => {

             var result=await sender.Send(new GetUserQuery(id));

           return  result.Match(Results.Ok, Results.BadRequest);

         })
         .WithName("GetUser")
         .WithSummary("Get user by ID")
         .WithDescription("Retrieve a user's profile details by unique user identifier.");
    }
}
