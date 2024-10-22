using WarehouseManagement.API.Core.Interfaces;
using WarehouseManagement.API.Infrastructure.Data.Repositories;
using WarehouseManagement.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagement.API.Infrastructure;

public static class DefaultInfrastructureModule
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        // DB context SQL lite
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b
                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return builder;
    }
}