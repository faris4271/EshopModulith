using Shared.Contract.CQRS;

namespace Module.Tax.Feature.UpdateTaxRate;

public sealed record UpdateTaxRateCommand(Guid Id, decimal Rate) : ICommand;
