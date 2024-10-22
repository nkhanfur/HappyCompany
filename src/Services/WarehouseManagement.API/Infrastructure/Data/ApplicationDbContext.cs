using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate.Enums;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;
using WarehouseManagement.API.Infrastructure.Services;

namespace WarehouseManagement.API.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var passwordService = new PasswordService();
        builder.Entity<User>().HasData(
           new User
           {
               Id = 1,
               Email = "admin@happywarehouse.com",
               UserName = "Admin",
               Password = passwordService.HashPassword(new(), "P@ssw0rd"),
               Role = RoleType.Admin,
               Active = true
           });

        //TODO: prefer to create Seed better than this way
        builder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Jordan" },
            new Country { Id = 2, Name = "USA" },
            new Country { Id = 3, Name = "Canada" }
        );

        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(19, 4);
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<WarehouseItem> WarehouseItems => Set<WarehouseItem>();
}
