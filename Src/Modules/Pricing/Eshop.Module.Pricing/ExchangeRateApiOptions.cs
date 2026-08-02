namespace Eshop.Module.Pricing
{
    public class ExchangeRateApiOptions
    {
        public const string SectionName = "ExchangeRateApi";
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://v6.exchangerate-api.com/v6/";
        public string DefaultBaseCurrency { get; set; } = "USD";
    }

    // JSON response wrapper for ExchangeRate-API
    public record ExchangeRateApiResponse(
        string result,
        string base_code,
        Dictionary<string, decimal> conversion_rates
    );
}
