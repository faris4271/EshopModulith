using System.ComponentModel.DataAnnotations;

namespace MultiTenancyModule.Contract.Dtos;

public record TenantDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime ValidUpto { get; set; }
    public string? Issuer { get; set; }
}

public record CreateTenantDto(
    [Required][MaxLength(100)] string Name,
    [Required][MaxLength(100)] string Identifier,
    string? ConnectionString,
    string AdminEmail,
    string? Issuer,
    string? SubscriptionPlan,
    DateTime? TrialEndsAt
);

public record UpdateTenantDto(
    [MaxLength(100)] string? Name,
    string? ConnectionString,
    string? SubscriptionPlan,
    DateTime? TrialEndsAt,
    DateTime? SubscriptionEndsAt
);

public record TenantSettingsDto(
    Guid Id,
    Guid TenantId,
    string? CompanyName,
    string DefaultCurrency,
    string DefaultLanguage,
    string TimeZone,
    bool EnableEmailNotifications,
    bool EnableSmsNotifications,
    string? LogoUrl,
    string PrimaryColor,
    string SecondaryColor,
    string? CustomSettings,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public record UpdateTenantSettingsDto(
    [MaxLength(200)] string? CompanyName,
    [MaxLength(100)] string? DefaultCurrency,
    [MaxLength(10)] string? DefaultLanguage,
    [MaxLength(50)] string? TimeZone,
    bool? EnableEmailNotifications,
    bool? EnableSmsNotifications,
    [MaxLength(1000)] string? LogoUrl,
    [MaxLength(7)] string? PrimaryColor,
    [MaxLength(7)] string? SecondaryColor,
    string? CustomSettings
);