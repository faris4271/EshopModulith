using System;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductTemplet.DeleteProductTemplet
{
    public record DeleteProductTempletCommand(Guid Id) : ICommand;
}