using Eshop.Module.Basket.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;


namespace Eshop.Module.Basket.Feature.Commands.CreatCartRule
{
    internal record CreatCartRuleCommand(CartRuleDto CartRuleDto) : ICommand;
   
}
