using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Tokens.TokenGeneration;

public record GenerateTokenCommand(
    string Email,
    string Password)
    : ICommand<TokenResponse>;