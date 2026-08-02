using Shared.Contract.CQRS;

namespace Module.Tax.Feature.UpdateTax;

public sealed record UpdateTaxCommand(Guid Id, string Name) : ICommand;
