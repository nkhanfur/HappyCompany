using HappyCompany.Data;
using HappyCompany.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.API.Core.AggregatesModel.UserAggregate;
using WarehouseManagement.API.Core.AggregatesModel.WarehouseAggregate;
using WarehouseManagement.API.Core.Interfaces;

namespace WarehouseManagement.API.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public DbContext Context => _context;

    public IRepository<User> Users { get; }

    public IRepository<Country> Countries { get; }

    public IRepository<Warehouse> Warehouses { get; }

    public IRepository<WarehouseItem> WarehouseItems { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Users = new Repository<User>(context);
        Countries = new Repository<Country>(context);
        Warehouses = new Repository<Warehouse>(context);
        WarehouseItems = new Repository<WarehouseItem>(context);
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}