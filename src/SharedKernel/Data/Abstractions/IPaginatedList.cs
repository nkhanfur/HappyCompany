namespace HappyCompany.Data.Abstractions;
public interface IPaginatedList<T> : IReadOnlyList<T>
{
    int PageNumber { get; }

    int PageSize { get; }

    int PageCount { get; }

    int TotalItemCount { get; }

    int FirstItemOnPage { get; }

    int LastItemOnPage { get; }

    bool IsFirstPage { get; }

    bool IsLastPage { get; }
}