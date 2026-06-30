using System;

namespace CatalogContract.Dtos
{
    public record GetProductTempletsDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }
    }
}
