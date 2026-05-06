using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogContract.Dtos
{
    public class GetCartRuleProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}
