using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Dtos
{
    internal class CartRuleGridDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? StartOn { get; set; }
        public DateTimeOffset? EndOn { get; set; }

        public bool IsActive { get; set; }
    }
}
