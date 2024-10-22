using HappyCompany.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HappyCompany.Data;

public class PaginatedList<T> : List<T>, IPaginatedList<T>
{
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
        TotalItemCount = count;

        AddRange(items);
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int PageCount { get; }

    public int TotalItemCount { get; }

    public int FirstItemOnPage
    {

        get { return (PageNumber - 1) * PageSize + 1; }
    }

    public int LastItemOnPage
    {
        get { return Math.Min(PageNumber * PageSize, TotalItemCount); }
    }

    public bool IsFirstPage
    {
        get { return PageNumber == 1; }
    }

    public bool IsLastPage
    {
        get { return PageNumber == PageCount; }
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
