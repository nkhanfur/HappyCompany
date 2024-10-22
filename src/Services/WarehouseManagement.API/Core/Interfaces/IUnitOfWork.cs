using HappyCompany.Data.Abstractions;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;

namespace WarehouseManagement.API.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }

    IRepository<Country> Countries { get; }

    IRepository<Warehouse> Warehouses { get; }

    IRepository<WarehouseItem> WarehouseItems { get; }

    Task<int> SaveAsync(CancellationToken cancellationToken);
}