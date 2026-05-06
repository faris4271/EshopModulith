using System;
using System.ComponentModel.DataAnnotations;
using Shared.DDD;

namespace Eshop.Module.Core.Models
{
    public class Widget : EntityBase<Guid>,IAuditableEntity
    {


        public string Code
        {
            get
            {
                return Id.ToString();
            }
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        [StringLength(450)]
        public string ViewComponentName { get; set; }

        [StringLength(450)]
        public string CreateUrl { get; set; }

        [StringLength(450)]
        public string EditUrl { get; set; }


        public bool IsPublished { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string? CreatedById {  get; set; }

        public DateTimeOffset? LatestUpdatedOn {  get; set; }

        public string? LatestUpdatedById {  get; set; }
    }
}
