using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenancyModule.Provisioning;
using Shared.Multitenancy;

namespace MultiTenancyModule.Data.Configurations;

public class TenantProvisioningConfiguration : IEntityTypeConfiguration<TenantProvisioning>
{
    public void Configure(EntityTypeBuilder<TenantProvisioning> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("TenantProvisionings", MultitenancyConstants.Schema);

        builder.HasMany(p => p.Steps)
            .WithOne(s => s.Provisioning!)
            .HasForeignKey(s => s.ProvisioningId);
    }
}