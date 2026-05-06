using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    internal record AddToBasketCommand(Guid productId,int quantity) : ICommand<string>;
    
}
