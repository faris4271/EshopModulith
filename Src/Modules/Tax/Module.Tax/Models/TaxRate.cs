using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Module.Tax.Models
{
    public class TaxRate : EntityBase<Guid>
    {
        public Guid TaxClassId { get; set; }

        public TaxClass TaxClass { get; set; }

        public Guid CountryId { get; set; }


        public Guid? StateOrProvinceId { get; set; }

        public decimal Rate { get; set; }

        [StringLength(450)]
        public string ZipCode { get; set; }
    }
}
