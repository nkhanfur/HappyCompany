using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseManagement.API.Application.V1.Queries;

public class WarehouseQuery
{
    [FromQuery(Name = "pageNumber")]
    public int? PageNumber { get; set; }

    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; set; }
}

public class WarehouseQueryValidator : AbstractValidator<WarehouseQuery>
{
    public WarehouseQueryValidator()
    {
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).GreaterThanOrEqualTo(1);
    }
}