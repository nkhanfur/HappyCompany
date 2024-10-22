using Ardalis.Specification;

namespace HappyCompany.Data.Abstractions;
public interface IRepository<T> : IReadRepositoryBase<T> where T : class
{
    T Add(T entity);

    T Update(T entity);

    T Remove(T entity);

    Task<T?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<IPaginatedList<T>> PaginatedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}