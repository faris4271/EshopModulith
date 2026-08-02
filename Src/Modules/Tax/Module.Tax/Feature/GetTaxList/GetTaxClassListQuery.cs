using Module.Tax.Feature.GetTax;
using Shared.Contract.CQRS;

namespace Module.Tax.Feature.GetTaxList
{
    public record GetTaxClassListQuery : IQuery<List<TaxClassDto>>;

}
