namespace Module.Identity.Contract.Feature.Tokens.RefreshToken;

public sealed record RefreshTokenCommandResponse(
    string Token,
    string RefreshToken,
    DateTime RefreshTokenExpiryTime);