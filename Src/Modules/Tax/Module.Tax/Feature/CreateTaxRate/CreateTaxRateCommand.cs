using Shared.Contract.CQRS;

namespace Module.Tax.Feature.CreateTaxRate;

public sealed record CreateTaxRateCommand(Guid TaxClassId, Guid CountryId, Guid? StateOrProvinceId, decimal Rate, string ZipCode) : ICommand<Guid>;
