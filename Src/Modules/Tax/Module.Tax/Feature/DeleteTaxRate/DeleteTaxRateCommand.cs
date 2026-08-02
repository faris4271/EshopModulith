using Shared.Contract.CQRS;

namespace Module.Tax.Feature.DeleteTaxRate;

public sealed record DeleteTaxRateCommand(Guid Id) : ICommand;
