using Eshop.Module.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Core.Data.Configurations
{
    internal class EntityTypeConfiguration : IEntityTypeConfiguration<EntityType>
    {
        public void Configure(EntityTypeBuilder<EntityType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasData(
              new EntityType("Category") { AreaName = "Catalog", RoutingController = "Category", RoutingAction = "CategoryDetail", IsMenuable = true },
              new EntityType("Brand") { AreaName = "Catalog", RoutingController = "Brand", RoutingAction = "BrandDetail", IsMenuable = true },
              new EntityType("Product") { AreaName = "Catalog", RoutingController = "Product", RoutingAction = "ProductDetail", IsMenuable = false },
              new EntityType("Vendor") { AreaName = "Catalog", RoutingController = "Vendor", RoutingAction = "VendorDetail", IsMenuable = false }
          );
        }
    }
}
