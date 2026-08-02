using Shared.Contract.CQRS;
using Module.Tax.Models;

namespace Module.Tax.Feature.GetTax;

public sealed record GetTaxQuery(Guid Id) : IQuery<TaxClassDto>;

public sealed record TaxClassDto(Guid Id, string Name);
