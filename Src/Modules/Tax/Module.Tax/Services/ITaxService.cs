namespace Module.Tax.Services
{
    public interface ITaxService
    {
        Task<decimal> GetTaxPercent(Guid? taxClassId, Guid countryId, Guid? stateOrProvinceId, string zipCode);
    }
}
