using Shared.Contract.CQRS;

namespace Module.Tax.Contract.Features.GetTaxPercents
{
    public record GetTaxPercentQuery(Guid? taxClassId, Guid countryId, Guid? stateOrProvinceId, string zipCode) : IQuery<decimal>;


}
