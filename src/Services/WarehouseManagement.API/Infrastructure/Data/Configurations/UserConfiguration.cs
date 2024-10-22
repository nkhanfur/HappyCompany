using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;

namespace WarehouseManagement.API.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(t => t.UserName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(t => t.Email)
               .HasMaxLength(100)
               .IsRequired();


        builder.Property(t => t.Role)
               .HasMaxLength(25)
               .IsUnicode(false)
               .IsRequired()
               .HasConversion<string>();
    }
}
