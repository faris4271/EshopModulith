using System;
using System.Collections.Generic;
using System.Text;

namespace CatalogContract.Dtos
{
    public class GetBrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }
    }
}
