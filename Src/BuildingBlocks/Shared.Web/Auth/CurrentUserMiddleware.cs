using Microsoft.AspNetCore.Http;
using Shared.Contract.Context;
using Shared.Contract.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shared.Web.Auth
{
    internal class CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) : IMiddleware
    {
        private readonly ICurrentUserInitializer _currentUserInitializer = currentUserInitializer;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(next, nameof(next));

            _currentUserInitializer.SetCurrentUser(context.User);

            var activity=Activity.Current;

            if (activity is not null&&context.User.Identity?.IsAuthenticated == true)
            {

                var userId = context.User.GetUserId();
                
                var correlationId = context.Request.HttpContext.TraceIdentifier;

                if (!string.IsNullOrEmpty(userId))
                    activity.SetTag("fsh.user_id", userId);

                if (!string.IsNullOrEmpty(correlationId))
                    activity.SetTag("fsh.correlation_id", correlationId);
            }

            await next(context);
        }
    }
}
