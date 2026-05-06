using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.AssignUserRoles;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityModule.Feature.Users.AssignUserRoles
{
    internal class AssignUserRoleEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/user/assign-role{Id}", async (ISender sender,string Id,List<UserRoleDto> userRolesDto) =>
            {
                var result =await  sender.Send(new AssignUserRolesCommand { UserId = Id, UserRoles = userRolesDto });

                result.Match(Results.Ok, Results.BadRequest);
            });
        }
    }
}
