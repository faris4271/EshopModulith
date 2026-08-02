using Module.Tax.Feature.GetTax;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;

namespace Module.Tax.Feature.GetTaxClassGrid
{
    public record GetTaxClassGridQuery(
        SmartTableParam smartTableParam
    ) : IQuery<SmartTableResult<TaxClassDto>>;

}
