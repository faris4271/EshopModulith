using Shared.Contract.CQRS;

namespace Module.Tax.Feature.CreatTax;

public sealed record CreateTaxCommand(string Name, decimal Percentage) : ICommand<Guid>;
