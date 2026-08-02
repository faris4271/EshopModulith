using Module.Tax.Contract.Features.GetTaxPercents;
using Module.Tax.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Module.Tax.Feature.GetTaxPercents
{
    internal sealed class GetTaxPercentHandler(ITaxService _taxService) : IQueryHandler<GetTaxPercentQuery, decimal>
    {
        public async Task<Result<decimal>> Handle(GetTaxPercentQuery request, CancellationToken cancellationToken)
        {
            var taxPercent = await _taxService.GetTaxPercent(request.taxClassId, request.countryId, request.stateOrProvinceId, request.zipCode);

            if (taxPercent == null)
            {
                return Result.Failure<decimal>(Error.Failure("404", "Tax percent not found."));
            }


            return Result.Success(taxPercent);
        }
    }
}
