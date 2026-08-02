using Shared.Contract.CQRS;

namespace Module.Tax.Feature.DeleteTax;

public sealed record DeleteTaxCommand(Guid Id) : ICommand;
