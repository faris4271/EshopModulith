using Shared.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Eshop.Module.Shipping.Models;

public sealed class ShippingProvider : EntityBase<Guid>
{
    public Name Name { get; private set; } = default!;

    public bool IsEnabled { get; private set; }

    public string? ConfigureUrl { get; private set; }

    public bool ToAllShippingEnabledCountries { get; private set; }

    [StringLength(1000)]
    public string? OnlyCountryIdsString { get; private set; }

    [NotMapped]
    public IList<string> OnlyCountryIds
    {
        get
        {
            if (string.IsNullOrWhiteSpace(OnlyCountryIdsString))
            {
                return new List<string>();
            }

            return OnlyCountryIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }
    }

    public bool ToAllShippingEnabledStatesOrProvinces { get; private set; }

    public string? OnlyStateOrProvinceIdsString { get; private set; }

    [NotMapped]
    public IList<Guid> OnlyStateOrProvinceIds
    {
        get
        {
            if (string.IsNullOrWhiteSpace(OnlyStateOrProvinceIdsString))
            {
                return new List<Guid>();
            }

            return OnlyStateOrProvinceIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(Guid.Parse)
            .ToList();
        }
    }

    /// <summary>
    /// Additional setting for specific provider. Stored as json string.
    /// </summary>
    public string? AdditionalSettings { get; private set; }

    [StringLength(450)]
    public string? ShippingPriceServiceTypeName { get; private set; }

    private ShippingProvider() { }

    public static ShippingProvider Create(
        string name,
        bool isEnabled = true,
        string? configureUrl = null,
        bool toAllShippingEnabledCountries = true,
        string? onlyCountryIdsString = null,
        bool toAllShippingEnabledStatesOrProvinces = true,
        string? onlyStateOrProvinceIdsString = null,
        string? additionalSettings = null,
        string? shippingPriceServiceTypeName = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (name.Length > 450)
            throw new ArgumentException("Name cannot exceed 450 characters", nameof(name));

        var provider = new ShippingProvider
        {
            Id = Guid.NewGuid(),
            Name = new Name(name),
            IsEnabled = isEnabled,
            ConfigureUrl = string.IsNullOrWhiteSpace(configureUrl) ? null : configureUrl,
            ToAllShippingEnabledCountries = toAllShippingEnabledCountries,
            OnlyCountryIdsString = string.IsNullOrWhiteSpace(onlyCountryIdsString) ? null : onlyCountryIdsString,
            ToAllShippingEnabledStatesOrProvinces = toAllShippingEnabledStatesOrProvinces,
            OnlyStateOrProvinceIdsString = string.IsNullOrWhiteSpace(onlyStateOrProvinceIdsString) ? null : onlyStateOrProvinceIdsString,
            AdditionalSettings = string.IsNullOrWhiteSpace(additionalSettings) ? null : additionalSettings,
            ShippingPriceServiceTypeName = string.IsNullOrWhiteSpace(shippingPriceServiceTypeName) ? null : shippingPriceServiceTypeName
        };

        provider.AddDomainEvent(ShippingProviderCreatedEvent.Create(provider));
        return provider;
    }

    public void Update(
        string name,
        bool isEnabled,
        string? configureUrl = null,
        bool toAllShippingEnabledCountries = true,
        string? onlyCountryIdsString = null,
        bool toAllShippingEnabledStatesOrProvinces = true,
        string? onlyStateOrProvinceIdsString = null,
        string? additionalSettings = null,
        string? shippingPriceServiceTypeName = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (name.Length > 450)
            throw new ArgumentException("Name cannot exceed 450 characters", nameof(name));

        Name = new Name(name);
        IsEnabled = isEnabled;
        ConfigureUrl = string.IsNullOrWhiteSpace(configureUrl) ? null : configureUrl;
        ToAllShippingEnabledCountries = toAllShippingEnabledCountries;
        OnlyCountryIdsString = string.IsNullOrWhiteSpace(onlyCountryIdsString) ? null : onlyCountryIdsString;
        ToAllShippingEnabledStatesOrProvinces = toAllShippingEnabledStatesOrProvinces;
        OnlyStateOrProvinceIdsString = string.IsNullOrWhiteSpace(onlyStateOrProvinceIdsString) ? null : onlyStateOrProvinceIdsString;
        AdditionalSettings = string.IsNullOrWhiteSpace(additionalSettings) ? null : additionalSettings;
        ShippingPriceServiceTypeName = string.IsNullOrWhiteSpace(shippingPriceServiceTypeName) ? null : shippingPriceServiceTypeName;

        AddDomainEvent(ShippingProviderUpdatedEvent.Create(this));
    }

    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
        AddDomainEvent(ShippingProviderStatusChangedEvent.Create(this, enabled));
    }

    public void MarkDeleted()
    {
        AddDomainEvent(ShippingProviderDeletedEvent.Create(this));
    }
}
