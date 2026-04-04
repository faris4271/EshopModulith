using System.ComponentModel.DataAnnotations;

namespace EShop.Module.Core.Contract.Dtos
{
    public class EntityTypeDto
    {
        public string Name { get; set; }

        public bool IsMenuable { get; set; }

        [StringLength(450)]
        public string AreaName { get; set; }

        [StringLength(450)]
        public string RoutingController { get; set; }

        [StringLength(450)]
        public string RoutingAction { get; set; }
    }
}