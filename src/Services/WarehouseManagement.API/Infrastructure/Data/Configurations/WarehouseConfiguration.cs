using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Infrastructure.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.Property(t => t.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(t => t.Address)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(t => t.City)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasOne(t => t.Country)
            .WithMany()
            .HasForeignKey(t => t.CountryId);

        builder
             .HasMany(t => t.Items)
             .WithOne(t => t.Warehouse)
             .HasForeignKey(t => t.WarehouseId);
    }
}
