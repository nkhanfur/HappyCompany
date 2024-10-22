using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseManagement.API.Application.V1.Queries;

public class WarehouseItemQuery
{
    //TODO: Add more filters

    [FromQuery(Name = "pageNumber")]
    public int? PageNumber { get; set; }

    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; set; }
}

public class WarehouseItemQueryValidator : AbstractValidator<WarehouseItemQuery>
{
    public WarehouseItemQueryValidator()
    {
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).GreaterThanOrEqualTo(1);
    }
}