﻿namespace HappyCompany.AspNetCore.Mvc;

public class PaginationResult
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public int TotalItemCount { get; set; }

    public bool FirstPage { get; set; }

    public bool LastPage { get; set; }
}