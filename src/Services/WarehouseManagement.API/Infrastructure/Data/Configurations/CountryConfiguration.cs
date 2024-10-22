using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();
    }
}