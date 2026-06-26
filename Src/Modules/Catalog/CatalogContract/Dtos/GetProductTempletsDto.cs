using System;

namespace SimplCommerce.Module.Catalog.Areas.Catalog.Controllers
{
    public record GetProductTempletsDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }
    }
}
