using System.Collections.Generic;

namespace CatalogContract.Dtos
{
    public class ProductDetailOption
    {
        public Guid OptionId { get; set; }

        public string OptionName { get; set; }

        public IList<string> Values { get; set; } = new List<string>();
    }
}
