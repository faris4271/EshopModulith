using Microsoft.EntityFrameworkCore;
using Module.Tax.Data;
using Module.Tax.Models;
using Shared.Abstraction;

namespace Module.Tax.Services
{
    public class TaxService : ITaxService
    {
        private readonly IGenericeRepository<TaxRate, TaxDbContext> _taxRateRepository;

        public TaxService(IGenericeRepository<TaxRate, TaxDbContext> taxRateRepository)
        {
            _taxRateRepository = taxRateRepository;
        }

        public async Task<decimal> GetTaxPercent(Guid? taxClassId, Guid countryId, Guid? stateOrProvinceId, string zipCode)
        {
            if (!taxClassId.HasValue)
            {
                return 0;
            }

            var query = await _taxRateRepository.Query();
            query = query
                           .Where(x => x.CountryId == countryId
                           && (x.StateOrProvinceId == stateOrProvinceId || x.StateOrProvinceId == null)
                           && x.TaxClassId == taxClassId.Value);
            if (!string.IsNullOrEmpty(zipCode))
            {
                query = query.Where(x => x.ZipCode == zipCode || string.IsNullOrWhiteSpace(x.ZipCode));
            }

            var taxRate = await query.FirstOrDefaultAsync();
            if (taxRate != null)
            {
                return taxRate.Rate;
            }

            return 0;
        }
    }
}
