using Module.Identity.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Module.Identity.Contract.Services
{
    public interface ITokenServic
    {
    Task<TokenResponse> IssueAsync(string subject,IEnumerable<Claim> claims,CancellationToken ct = default);
    }
}
