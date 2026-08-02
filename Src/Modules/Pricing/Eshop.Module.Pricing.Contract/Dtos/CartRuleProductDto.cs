using System;

namespace Eshop.Module.Pricing.Contract.Dtos
{
    public class CartRuleProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}