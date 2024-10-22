using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Infrastructure.Data.Configurations;

public class WarehouseItemConfiguration : IEntityTypeConfiguration<WarehouseItem>
{
    public void Configure(EntityTypeBuilder<WarehouseItem> builder)
    {
        builder.Property(t => t.ItemName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(t => t.SKUCode)
               .HasMaxLength(100)
               .IsRequired();

        builder
             .HasOne(t => t.Warehouse)
             .WithMany()
             .HasForeignKey(t => t.WarehouseId)
             .OnDelete(DeleteBehavior.Cascade);
    }
}
