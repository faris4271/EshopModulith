using Shared.Contract.CQRS;

namespace Module.Tax.Feature.GetTaxRate;

public sealed record GetTaxRateQuery(Guid Id) : IQuery<TaxRateDto>;

public sealed record TaxRateDto(Guid Id, Guid TaxClassId, Guid CountryId, decimal Rate);
