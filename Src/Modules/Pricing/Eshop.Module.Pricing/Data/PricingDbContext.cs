using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;

public sealed class PricingDbContext(DbContextOptions<PricingDbContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<ProductTierPrice> ProductTierPrices { get; set; }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("pricing");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PricingDbContext).Assembly);
    }
}