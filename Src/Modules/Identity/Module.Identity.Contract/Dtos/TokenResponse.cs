using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Dtos
{
    public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    DateTime AccessTokenExpiresAt);
}
