using Ardalis.Specification.EntityFrameworkCore;
using HappyCompany.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HappyCompany.Data;

public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    protected readonly DbContext _context;

    public Repository(DbContext context)
        : base(context)
    {
        _context = context;
    }

    public T Add(T entity)
    {
        return _context.Set<T>().Add(entity).Entity;
    }

    public T Update(T entity)
    {
        return _context.Set<T>().Update(entity).Entity;
    }

    public T Remove(T entity)
    {
        return _context.Set<T>().Remove(entity).Entity;
    }

    public async Task<T?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IPaginatedList<T>> PaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await PaginatedList<T>.CreateAsync(_context.Set<T>(), pageNumber, pageSize, cancellationToken);
    }
}